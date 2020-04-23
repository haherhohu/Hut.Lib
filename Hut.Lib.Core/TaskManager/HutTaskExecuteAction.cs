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
            status = HutTaskActionStatus.Ready;
            result = HutTaskActionResult.NotApplicable;
            Procedure += () =>
            {
                if (File.Exists(executablepath))
                {
                    // change status for processing
                    ActionStatus = HutTaskActionStatus.Processing;
                    Process process = Process.Start(executablepath);
                    start = process.StartTime;
                    process.WaitForExit();

                    // update final status
                    updateStatus(process.ExitCode, process.ExitTime);
                }
            };
        }

        public void updateStatus(int exitcode, DateTime exittime)
        {
            result = exitcode == 0 ? HutTaskActionResult.Success : HutTaskActionResult.Fail;
            end = exittime;
            ActionStatus = HutTaskActionStatus.Complete;
        }

        public HutTaskActionStatus ActionStatus
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(@"ActionStatus"));
                }
            }
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

        #endregion properties
    }
}