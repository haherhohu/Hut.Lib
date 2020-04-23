/******************************************************************************
* Hut Time Task
*
* - Task Info for Time-Based Action
*
* Author : Daegung Kim
* Version: 1.0.2
* Update : 2017-04-12
******************************************************************************/

using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTimeTask : IHutTask
    {
        #region variables

        public static string TASK_DIR = @"Hut";

        protected bool enable;
        protected int id;
        protected string taskdir;
        protected string taskname;
        protected string taskgroup;
        protected DateTime starttime;
        protected DateTime endtime;
        protected string description;
        protected uint repeatcount;

        // will be deprecated
        protected HutTaskActionStatus status; // for check trigger is fired.

        protected List<IHutTaskAction> actions;
        protected HutTimeTaskRepeatOption option;

        #endregion variables

        // constructor
        public HutTimeTask()
        {
            endtime = DateTime.MaxValue - TimeSpan.FromDays(1);
            actions = new List<IHutTaskAction>();
            status = HutTaskActionStatus.Ready;
            taskdir = TASK_DIR;
        }

        public void setTask()
        {
            using (TaskService service = new TaskService())
            {
                actions.ForEach(f => f.Parent = this);

                if (service.GetFolder(taskdir) == null)
                    service.RootFolder.CreateFolder(taskdir);

                // Task Definition
                TaskDefinition taskdefinition = service.NewTask();
                taskdefinition.RegistrationInfo.Description = description;
                taskdefinition.Principal.LogonType = TaskLogonType.InteractiveToken;
                taskdefinition.Settings.Enabled = true;
                taskdefinition.Triggers.Add(option.makeTrigger(starttime, endtime));
                // 현재 수행중인 프로그램의 이름으로 등록
                taskdefinition.Actions.Add(new ExecAction(Assembly.GetEntryAssembly().GetName().Name + @".exe", @"Trigging", Environment.CurrentDirectory));
                service.GetFolder(taskdir).RegisterTaskDefinition(taskname, taskdefinition);
            }
        }

        #region deprecated. but using until update with HwndMessage. Not Today

        // 계속 확인해야 됨.
        public void updateStatus(Task laststatus)
        {
            HutTaskActionStatus lateststatus = checkStatus(status, laststatus.State, laststatus.LastTaskResult, option.Repeat);

            if (lateststatus == HutTaskActionStatus.Ready)
                actions.ForEach(f => { f.ActionStatus = HutTaskActionStatus.Ready; });

            // 계속 수행됨.
            if (status == HutTaskActionStatus.Ready && lateststatus == HutTaskActionStatus.Processing)
            {
                status = lateststatus;

                BackgroundWorker worker = new BackgroundWorker();
                worker.DoWork += ActionRaised;
                worker.RunWorkerAsync();
            }
        }

        private HutTaskActionStatus checkStatus(HutTaskActionStatus currentstate, TaskState lateststate, int taskresult, HutTaskRepetition repeat)
        {
            if (repeat == HutTaskRepetition.Once)
            {
                return checkStatusForNonRepeatationTask(currentstate, lateststate, taskresult);
            }
            else
                return checkStatusForRepeatationTask(currentstate, lateststate, taskresult);
        }

        private HutTaskActionStatus checkStatusForNonRepeatationTask(HutTaskActionStatus currentstate, TaskState lateststate, int taskresult)
        {
            if (currentstate != HutTaskActionStatus.Complete)
            {
                if (taskresult == 0x41301 || lateststate == TaskState.Running)
                {
                    currentstate = HutTaskActionStatus.Processing;
                }
                else if ((taskresult == 0 || taskresult == 1) && currentstate == HutTaskActionStatus.Processing)
                {
                    currentstate = HutTaskActionStatus.Complete;
                }
            }

            return currentstate;
        }

        private HutTaskActionStatus checkStatusForRepeatationTask(HutTaskActionStatus currentstate, TaskState lateststate, int taskresult)
        {
            switch (lateststate)
            {
                case TaskState.Ready:
                    if (currentstate == HutTaskActionStatus.Processing)
                        currentstate = HutTaskActionStatus.Complete;
                    else
                        currentstate = HutTaskActionStatus.Ready;
                    break;

                case TaskState.Running:
                    currentstate = HutTaskActionStatus.Processing;
                    break;

                // unused
                case TaskState.Disabled:
                case TaskState.Queued:
                case TaskState.Unknown:
                default:
                    break;
            }
            return currentstate;
        }

        #endregion deprecated. but using until update with HwndMessage. Not Today

        public void ActionRaised(object sender, DoWorkEventArgs e)
        {
            // filetask 는 이벤트 스레드에서 수행
            // timetask 는 지금 수행중인 스레드에서 수행됨. 별도 스레드 분리 필요.
            repeatcount++;
            actions.ForEach(f => { f.ActionStatus = HutTaskActionStatus.Processing; f.Procedure(); });
        }

        public void Dispose()
        {
            using (TaskService service = new TaskService())
            {
                service.GetFolder(taskdir).DeleteTask(taskname);
            }
            //            actions.Clear();
        }

        public void resetCount()
        {
            repeatcount = 0;
        }

        private HutTaskResultLog generateResult(IHutTaskAction action, DateTime starttime)
        {
            HutTaskActionResult result = action.ActionResult;
            //if (result == HutTaskActionResult.NotApplicable && starttime > DateTime.Today && starttime < DateTime.Now)
            //    result = HutTaskActionResult.Fail;

            return new HutTaskResultLog()
            {
                Level = HutLogLevel.Info,
                Message = string.Format(@"Task {0}({1}) of {2} is {3}.", Name, Group, TaskType.ToString(), action.ActionStatus.ToString()),
                ID = ID,
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
            // multiple results
            else
            {
                int interval = option.Interval;
                if (option.Repeat == HutTaskRepetition.Hourly)
                    interval *= 60;

                List<HutTaskResultLog> results = new List<HutTaskResultLog>();
                for (DateTime time = StartTime; time < DateTime.Now.Date.AddDays(1); time += TimeSpan.FromMinutes(interval))
                {
                    results.AddRange(actions.Select(s => generateResult(s, time)));
                }
                return results;
            }
        }

        #region Properties

        // properties
        [JsonProperty]
        public bool Enable { get { return enable; } set { enable = value; } }

        [JsonProperty]
        public int ID { get { return id; } set { id = value; } }

        [JsonProperty]
        public string Name { get { return taskname; } set { taskname = value; } }

        [JsonProperty]
        public string Group { get { return taskgroup; } set { taskgroup = value; } }

        [JsonProperty]
        public DateTime StartTime { get { return starttime; } set { starttime = value; } }

        [JsonProperty]
        public DateTime EndTime { get { return endtime; } set { endtime = value; } }

        [JsonProperty]
        public string Description { get { return description; } set { description = value; } }

        [JsonProperty]
        public string TaskDir { get { return taskdir; } set { taskdir = value; } }

        [JsonProperty]
        public HutTaskType TaskType { get { return HutTaskType.Time; } }

        [JsonProperty]
        public uint RepeatCount { get { return repeatcount; } }

        [JsonProperty]
        public HutTaskActionStatus Status { get { return status; } set { status = value; } }

        [JsonProperty]
        public List<IHutTaskAction> Actions { get { return actions; } set { actions = value; } }

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