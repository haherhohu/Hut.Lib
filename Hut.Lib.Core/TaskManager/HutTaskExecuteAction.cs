/******************************************************************************
* Hut Execute Action
*
* - Action for Execute Binary. this Action run Exectable File.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTaskExecuteAction : IHutTaskAction
    {
        #region variables

        public event PropertyChangedEventHandler PropertyChanged;

        protected DateTime start;
        protected DateTime end;
        protected HutTaskActionStatus status;
        protected HutTaskActionResult result;
        protected string executablepath;
        protected string executableoptions;

        #endregion variables

        public HutTaskExecuteAction()
        {
            initStatus();
            Procedure += () =>
            {
                if (File.Exists(executablepath))
                {
                    // change status for processing
                    result = HutTaskActionResult.Waiting;
                    ActionStatus = HutTaskActionStatus.Processing;

                    Process process = Process.Start(executablepath);
                    start = process.StartTime;
                    process.WaitForExit();

                    // update final status
                    updateStatus(process.ExitCode, process.ExitTime);
                }
                else
                {
                    result = HutTaskActionResult.Fail;
                    end = DateTime.Now;
                    ActionStatus = HutTaskActionStatus.Complete;
                    Console.WriteLine(@"file not exists");
                }
            };
        }

        public void updateStatus(int exitcode, DateTime exittime)
        {
            result = exitcode == 0 ? HutTaskActionResult.Success : HutTaskActionResult.Fail;
            end = exittime;
            ActionStatus = HutTaskActionStatus.Complete;
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
            return new HutTaskExecuteAction()
            {
                ActionStatus = this.ActionStatus,
                ExecutablePath = this.ExecutablePath,
                Name = this.Name,
                Parent = this.Parent,
            };
        }

        public void initStatus(HutTaskActionResult r = HutTaskActionResult.NotApplicable)
        {
            status = HutTaskActionStatus.Ready;
            result = r;
            start = default(DateTime);
            end = default(DateTime);
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
        public HutTaskActionType ActionType { get { return HutTaskActionType.Execute; } }

        [JsonProperty]
        public HutTaskActionResult ActionResult { get { return result; } }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string ExecutablePath { get { return executablepath; } set { executablepath = value; } }

        public Action Procedure { get; set; }

        public object Args { get; set; }

        #endregion properties
    }
}