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
using System.ComponentModel;
using System.Threading;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutFileTask : IHutTask
    {
        #region variables

        protected List<string> targetpaths;
        protected List<IHutTaskAction> actions;
        private FileSystemWatcher filewatcher;
        public HutTaskActionResult LastResult { get; set; }

        #endregion variables

        // constructor
        public HutFileTask()
        {
            targetpaths = new List<string>();
            actions = new List<IHutTaskAction>();
            Results = new List<IHutTaskResult>();
        }

        public void setTask()
        {
            setTask(targetpaths);
            //Actions.ForEach(f => { f.Parent = this; });
        }

        public void setTask(List<string> targetfullpaths)
        {
            foreach (string targetfullpath in targetfullpaths)
                setTask(Path.GetDirectoryName(targetfullpath), Path.GetFileName(targetfullpath));
        }

        public void setTask(string targetpath, string targetfile)
        {
            List<IHutTaskAction> actioninstances = new List<IHutTaskAction>();
            actions.ForEach(f => { f.Parent = this; actioninstances.Add(f.Clone() as IHutTaskAction); });
            actioninstances.ForEach(f => f.initStatus(HutTaskActionResult.Waiting));
            Actions = actioninstances;

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
            BackgroundWorker work = new BackgroundWorker();

            work.DoWork += (s, ex) =>
            {
                if (!(sender as FileSystemWatcher).EnableRaisingEvents)
                    return;

                (sender as FileSystemWatcher).EnableRaisingEvents = false;

                waitFileWriteComplete(Directory.GetFiles((sender as FileSystemWatcher).Path, (sender as FileSystemWatcher).Filter));

                foreach (IHutTaskAction action in actions.Where(w => w.ActionStatus != HutTaskActionStatus.Processing))
                {
                    action.Procedure();
                    if (LastResult == HutTaskActionResult.Fail)
                    {
                        break;
                    }
                }
                (sender as FileSystemWatcher).EnableRaisingEvents = true;
            };
            work.RunWorkerAsync();

            //repeatcount++; => Results.Add();
        }

        private void waitFileWriteComplete(string[] targets)
        {
            foreach (string target in targets)
            {
                FileStream stream = null;

                try
                {
                    stream = File.Open(target, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                }
                catch (IOException)
                {
                    Thread.Sleep(300);
                    continue;
                }
                finally
                {
                    if (stream != null)
                        stream.Close();
                }
            }
        }

        public void Dispose()
        {
            // filewatcher has only one instance, so delete second time has null point error
            if (filewatcher != null)
                filewatcher.Dispose();
        }

        public void resetCount()
        {
            Results.Clear();
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
        public bool Enable { get; set; }

        public int ID { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Group { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        [JsonProperty]
        public List<string> TargetPaths { get { return targetpaths; } set { targetpaths = value; } }

        [JsonProperty]
        public HutTaskType TaskType { get { return HutTaskType.File; } }

        [JsonProperty]
        public uint RepeatCount { get { return (uint)Results.Count; } }

        [JsonProperty]
        public List<IHutTaskAction> Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        public List<IHutTaskResult> Results { get; set; }

        public List<HutTaskExecuteAction> ExecuteActions { get { return actions.Where(w => w.ActionType == HutTaskActionType.Execute).OfType<HutTaskExecuteAction>().ToList(); } }

        public List<HutTaskTransferAction> TransferActions { get { return actions.Where(w => w.ActionType == HutTaskActionType.Transfer).OfType<HutTaskTransferAction>().ToList(); } }

        public List<HutTaskArchiveAction> ArchiveActions { get { return actions.Where(w => w.ActionType == HutTaskActionType.Archive).OfType<HutTaskArchiveAction>().ToList(); } }

        #endregion Properties
    }
}