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

        protected HutTaskArchiveOption option;

        #endregion variables

        public HutTaskArchiveAction()
        {
            status = HutTaskActionStatus.Ready;
            result = HutTaskActionResult.NotApplicable;
            Procedure += () =>
            {
                if (Directory.Exists(srcpath))
                {
                    if (!Directory.Exists(dstpath))
                        Directory.CreateDirectory(dstpath);

                    // TODO: apply archive option
                    List<string> files = Directory.GetFiles(srcpath).ToList();

                    // change status for processing
                    start = DateTime.Now;
                    ActionStatus = HutTaskActionStatus.Processing;
                    List<IOException> exceptions = new List<IOException>();
                    foreach (string filename in files)
                    {
                        try
                        {
                            File.Move(filename, Path.Combine(dstpath, Path.GetFileName(filename)));
                        }
                        catch (IOException e)
                        {
                            exceptions.Add(e);
                        }
                    }

                    // update final status
                    updateStatus(exceptions.Select(s => s.Message).ToList());
                }
            };
        }

        public void updateStatus(List<string> exceptionmessages)
        {
            result = exceptionmessages.Count == 0 ? HutTaskActionResult.Success : HutTaskActionResult.Fail;
            end = DateTime.Now;

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

        #endregion properties
    }
}