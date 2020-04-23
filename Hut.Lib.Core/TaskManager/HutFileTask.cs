/******************************************************************************
 * Hut File Task
 *
 * - Task Info for File-Based Action
 *
 * Author : Daegung Kim
 * Version: 1.0.2
 * Update : 2017-04-11
 ******************************************************************************/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutFileTask : IHutTask
    {
        #region variables

        protected bool enable;
        protected int id;
        protected string description;
        protected List<string> targetpaths;
        protected string taskgroup;
        protected string taskname;
        protected uint repeatcount;
        protected List<IHutTaskAction> actions;
        private FileSystemWatcher filewatcher;

        #endregion variables

        // constructor
        public HutFileTask()
        {
            actions = new List<IHutTaskAction>();
        }

        public void setTask()
        {
            setTask(targetpaths);
        }

        public void setTask(List<string> targetfullpaths)
        {
            foreach (string targetfullpath in targetfullpaths)
                setTask(Path.GetDirectoryName(targetfullpath), Path.GetFileName(targetfullpath));
        }

        public void setTask(string targetpath, string targetfile)
        {
            actions.ForEach(f => f.Parent = this);

            if (targetpath != null && Directory.Exists(targetpath))
            {
                filewatcher = new FileSystemWatcher(targetpath)
                {
                    Filter = targetfile,
                    NotifyFilter = NotifyFilters.FileName,
                    EnableRaisingEvents = true,
                };
                filewatcher.Created += ActionRaised;
            }
            else
            {
                // TODO: exception
                Console.WriteLine(@"Cannot Add Task. directory not exists");
            }
        }

        private void ActionRaised(object sender, FileSystemEventArgs e)
        {
            repeatcount++;
            foreach (IHutTaskAction action in actions)
                action.Procedure();
        }

        public void Dispose()
        {
            filewatcher.Dispose();
            //            actions.Clear();
        }

        public void resetCount()
        {
            repeatcount = 0;
        }

        private HutTaskResultLog generateResult(IHutTaskAction action)
        {
            return new HutTaskResultLog()
            {
                Level = HutLogLevel.Info,
                Message = string.Format(@"Task {0}({1}) of {2} is {3}.", Name, Group, TaskType.ToString(), action.ActionStatus.ToString()),
                ID = ID,
                Name = Name,
                Group = Group,
                Status = action.ActionStatus,
                Result = action.ActionResult,
                ExecuteTime = action.StartTime,
                CompleteTime = action.EndTime,
                TaskType = TaskType,
                ActionID = action.GetHashCode(),
                ActionType = action.ActionType,
            };
        }

        public List<HutTaskResultLog> generateResults()
        {
            return Actions.Select(s => generateResult(s)).ToList();
        }

        #region Properties

        [JsonProperty]
        public bool Enable { get { return enable; } set { enable = value; } }

        [JsonProperty]
        public int ID { get { return id; } set { id = value; } }

        [JsonProperty]
        public string Name { get { return taskname; } set { taskname = value; } }

        [JsonProperty]
        public string Group { get { return taskgroup; } set { taskgroup = value; } }

        [JsonProperty]
        public string Description { get { return description; } set { description = value; } }

        [JsonProperty]
        public List<string> TargetPaths { get { return targetpaths; } set { targetpaths = value; } }

        [JsonProperty]
        public HutTaskType TaskType { get { return HutTaskType.File; } }

        [JsonProperty]
        public uint RepeatCount { get { return repeatcount; } }

        [JsonProperty]
        public List<IHutTaskAction> Actions
        {
            get { return actions; }
            set { actions = value; }
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

        #endregion Properties
    }
}