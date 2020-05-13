/******************************************************************************
* Hut Time Task
*
* - Task Info for Time-Based Action
*
* Author : Daegung Kim
* Version: 1.0.2
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTimeTask : IHutTask
    {
        #region variables

        // deprecated
        public string DefaultExecutable;

        public static string TASK_DIR;

        protected HutTaskActionStatus status; // for check trigger is fired.
        protected List<IHutTaskAction> actions;

        protected HutTimeTaskRepeatOption option;
        public HutTaskActionResult LastResult { get; set; }

        #endregion variables

        // constructor
        public HutTimeTask()
        {
            EndTime = DateTime.MaxValue - TimeSpan.FromDays(1);
            actions = new List<IHutTaskAction>();
            Results = new List<IHutTaskResult>();
            status = HutTaskActionStatus.Ready;
        }

        public void setTask()
        {
            if (StartTime == default(DateTime))
            {
                Console.WriteLine(@"this task is something wrong for starttime. then check and reset starttime for registration");
                return;
            }

            status = HutTaskActionStatus.Ready;
            List<IHutTaskAction> actioninstances = new List<IHutTaskAction>();
            actions.ForEach(f => { f.Parent = this; actioninstances.Add(f.Clone() as IHutTaskAction); });
            actioninstances.ForEach(f => f.initStatus());
            actions.Clear();
            actions = actioninstances;
        }

        public void ActionRaised()
        {
            BackgroundWorker work = new BackgroundWorker();

            work.DoWork += (s, ex) =>
            {
                foreach (var act in actions)
                {
                    if (act.ActionStatus == HutTaskActionStatus.Ready)
                    {
                        act.ActionStatus = HutTaskActionStatus.Processing;
                        act.Args = null;
                        act.Procedure();
                        if (LastResult == HutTaskActionResult.Fail)
                            break;
                    }
                }
                status = HutTaskActionStatus.Complete;

                if (option.Repeat != HutTaskRepetition.Once && option.Repeat < HutTaskRepetition.Daily)
                    status = HutTaskActionStatus.Ready;
                updateStartTime();
            };

            work.RunWorkerAsync();
        }

        public void Dispose()
        {
        }

        public void resetCount()
        {
            Results.Clear();
        }

        private HutTaskResultLog generateResult(IHutTaskAction action, DateTime starttime, int additionalid = 0)
        {
            HutTaskActionResult result = action.ActionResult;

            return new HutTaskResultLog()
            {
                Level = HutLogLevel.Info,
                Message = string.Format(@"Task {0}({1}) of {2} is {3}.", Name, Group, TaskType.ToString(), action.ActionStatus.ToString()),
                ID = ID + additionalid,
                Name = Name,
                Group = Group,
                Status = action.ActionStatus,
                Result = result,
                ExecuteTime = starttime,
                CompleteTime = action.EndTime,
                TaskType = TaskType,
                ActionID = action.GetHashCode(),
                ActionType = action.ActionType,
            };
        }

        public List<HutTaskResultLog> generateResults()
        {
            if (option.Repeat == HutTaskRepetition.Once || option.Repeat == HutTaskRepetition.Daily || option.Repeat == HutTaskRepetition.Weekly)
                return Actions.Select(s => generateResult(s, StartTime)).ToList();
            // multiple results in one day
            else
            {
                int interval = option.Interval;
                if (option.Repeat == HutTaskRepetition.Hourly)
                    interval *= 60;

                List<HutTaskResultLog> results = new List<HutTaskResultLog>();
                int repeated = 0;
                for (DateTime time = StartTime; time < DateTime.Now.Date.AddDays(1); time += TimeSpan.FromMinutes(interval))
                {
                    results.AddRange(actions.Select(s => generateResult(s, time, repeated)));
                    repeated++;
                }
                return results;
            }
        }

        public void updateStartTime()
        {
            //TimeSpan dt = DateTime.Now - starttime;
            TimeSpan rep = option.makeIntervalWithUnit();

            while (StartTime < DateTime.Now)
            {
                StartTime += rep;
            }
        }

        #region Properties

        // properties
        [JsonProperty]
        public bool Enable { get; set; }

        public int ID { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Group { get; set; }

        [JsonProperty]
        public DateTime StartTime { get; set; }

        [JsonProperty]
        public DateTime EndTime { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        //[JsonProperty]
        //public string TaskDir { get { return taskdir; } set { taskdir = value; } }

        [JsonProperty]
        public HutTaskType TaskType { get { return HutTaskType.Time; } }

        [JsonProperty]
        public uint RepeatCount { get { return (uint)Results.Count; } }

        [JsonProperty]
        public HutTaskActionStatus Status { get { return status; } set { status = value; } }

        [JsonProperty]
        public List<IHutTaskAction> Actions { get { return actions; } set { actions = value; } }

        public List<IHutTaskResult> Results { get; set; }

        public bool isElapsed
        {
            get { return (StartTime.ToUniversalTime() < DateTime.UtcNow); }
        }

        public bool isComplete
        {
            get { return (status & HutTaskActionStatus.Complete) == HutTaskActionStatus.Complete; }
        }

        public bool isTimeToReady
        {
            get { return RemainTime.Duration() < TimeSpan.FromSeconds(5); }
        }

        public TimeSpan RemainTime
        {
            get { return (StartTime.ToUniversalTime() - DateTime.UtcNow); }
        }

        public List<HutTaskExecuteAction> ExecuteActions
        {
            get { return actions.Where(w => w.ActionType == HutTaskActionType.Execute).OfType<HutTaskExecuteAction>().ToList(); }
        }

        public List<HutTaskTransferAction> TransferActions
        {
            get { return actions.Where(w => w.ActionType == HutTaskActionType.Transfer).OfType<HutTaskTransferAction>().ToList(); }
        }

        public List<HutTaskArchiveAction> ArchiveActions
        {
            get { return actions.Where(w => w.ActionType == HutTaskActionType.Archive).OfType<HutTaskArchiveAction>().ToList(); }
        }

        [JsonProperty]
        public HutTimeTaskRepeatOption Option { get { return option; } set { option = value; } }

        #endregion Properties
    }
}