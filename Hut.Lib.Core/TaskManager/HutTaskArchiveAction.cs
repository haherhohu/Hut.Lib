/******************************************************************************
* Hut Archive Action
*
* - Action for Archive Data Files.
*   this Action Move Files from Source Dir to Archiving Dir.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTaskArchiveAction : IHutTaskAction
    {
        #region variables

        public event PropertyChangedEventHandler PropertyChanged;

        protected DateTime start;
        protected DateTime end;
        protected HutTaskActionStatus status;
        protected HutTaskActionResult result;
        protected string srcpath;
        protected string dstpath;
        protected string archivestatus;

        protected HutTaskArchiveOption option;

        #endregion variables

        public HutTaskArchiveAction()
        {
            initStatus();
            Procedure += () =>
            {
                List<string> exceptions = new List<string>();
                ActionStatus = HutTaskActionStatus.Processing;
                try
                {
                    if (Directory.Exists(srcpath))
                    {
                        if (!Directory.Exists(dstpath))
                            Directory.CreateDirectory(dstpath);

                        // TODO: apply archive option
                        List<string> files = Directory.GetFiles(srcpath).ToList();

                        // change status for processing
                        start = DateTime.Now;
                        result = HutTaskActionResult.Waiting;
                        foreach (var pair in files.Select(s => new { src = s, dst = Path.Combine(dstpath, Path.GetFileName(s)) }))
                        {
                            if (File.Exists(pair.dst))
                            {
                                File.Delete(pair.dst);
                            }

                            File.Move(pair.src, pair.dst);
                            ArchiveStatus = pair.src + @" -> " + pair.dst;
                        }
                    }
                    else
                    {
                        exceptions.Add(@"Directory" + srcpath + @" is not exist.");
                    }
                }
                catch (IOException e)
                {
                    exceptions.Add(e.Message);
                    ArchiveStatus = e.Message;
                }
                finally
                {
                    // update final status
                    updateStatus(exceptions);
                }
            };
        }

        public void updateStatus(List<string> exceptionmessages)
        {
            result = exceptionmessages.Count == 0 ? HutTaskActionResult.Success : HutTaskActionResult.Fail;
            end = DateTime.Now;

            if (exceptionmessages.Count > 0)
            {
                ArchiveStatus = @"Summary of Errors in Task: " + Name;
                foreach (string message in exceptionmessages)
                {
                    ArchiveStatus = @"  " + message;
                }
            }

            ActionStatus = HutTaskActionStatus.Complete;
        }

        public HutTaskActionStatus ActionStatus
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    if (PropertyChanged != null)
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ActionStatus"));
                }
            }
        }

        public string ArchiveStatus
        {
            get
            {
                return archivestatus;
            }
            set
            {
                if (archivestatus != value)
                {
                    archivestatus = value;
                    if (PropertyChanged != null)
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"ArchiveStatus"));
                }
            }
        }

        public HutTaskResultLog generateResult()
        {
            return new HutTaskResultLog()
            {
                Level = HutLogLevel.Info,
                Message = string.Format(@"Task {0}({1}) of {2} is {3}.", Name, Parent.Group, Parent.TaskType.ToString(), ActionStatus.ToString()),
                ID = Parent.ID,
                Name = Parent.Name,
                Group = Parent.Group,
                Status = ActionStatus,
                Result = ActionResult,
                ExecuteTime = StartTime,
                CompleteTime = EndTime,
                TaskType = Parent.TaskType,
                ActionID = GetHashCode(),
                ActionType = ActionType,
            };
        }

        public object Clone()
        {
            return new HutTaskArchiveAction()
            {
                Parent = this.Parent,
                ActionStatus = this.ActionStatus,
                Destination = this.Destination,
                Name = this.Name,
                Source = this.Source,
                Option = this.Option,
            };
        }

        public void initStatus(HutTaskActionResult r = HutTaskActionResult.NotApplicable)
        {
            status = HutTaskActionStatus.Ready;
            result = r;
            start = default(DateTime);
            end = default(DateTime);
        }

        public void resetStatus()
        {
            status = HutTaskActionStatus.Ready;
            result = HutTaskActionResult.NotApplicable;
            start = default(DateTime);
            end = default(DateTime);
        }

        #region properties

        public IHutTask Parent { get; set; }

        [JsonProperty]
        public DateTime StartTime { get { return start; } }

        [JsonProperty]
        public DateTime EndTime { get { return end; } }

        [JsonProperty]
        public HutTaskActionType ActionType { get { return HutTaskActionType.Archive; } }

        [JsonProperty]
        public HutTaskActionResult ActionResult { get { return result; } }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Source { get { return srcpath; } set { srcpath = value; } }

        [JsonProperty]
        public string Destination { get { return dstpath; } set { dstpath = value; } }

        [JsonProperty]
        public HutTaskArchiveOption Option { get { return option; } set { option = value; } }

        public Action Procedure { get; set; }

        public object Args { get; set; }

        #endregion properties
    }
}