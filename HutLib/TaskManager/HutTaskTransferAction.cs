/******************************************************************************
* Hut File Transfer Action
*
* - Action for File Transfer. this Action send File(s) by FTP
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

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTaskTransferAction : IHutTaskAction
    {
        #region variables

        public event PropertyChangedEventHandler PropertyChanged;

        protected DateTime start;
        protected DateTime end;
        protected HutTaskActionStatus status;
        protected HutTaskActionResult result;
        protected string srcpath;
        protected string dstpath;//TODO: change server info
        protected HutFTPServerInfo serverinfo;

        protected HutTaskTransferOption option; // file transfer options

        #endregion variables

        public HutTaskTransferAction()
        {
            status = HutTaskActionStatus.Ready;
            result = HutTaskActionResult.NotApplicable;
            Procedure += () =>
            {
                start = DateTime.Now;
                // TODO: using FTP;
                if (Directory.Exists(Path.GetDirectoryName(srcpath)))
                {
                    using (HutFTPClient client = new HutFTPClient())
                    {
                        client.init(serverinfo);
                        client.upload(srcpath, serverinfo);
                        updateStatus(new List<string>());
                    }
                }
                else
                {
                    Console.WriteLine(@"source path is not exists");
                    result = HutTaskActionResult.Fail;
                    end = DateTime.Now;
                    ActionStatus = HutTaskActionStatus.Complete;
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
        public HutTaskActionType ActionType { get { return HutTaskActionType.Transfer; } }

        [JsonProperty]
        public HutTaskActionResult ActionResult { get { return result; } }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Source { get { return srcpath; } set { srcpath = value; } }

        [JsonProperty]
        public string Destination { get { return dstpath; } set { dstpath = value; } }

        [JsonProperty]
        public HutFTPServerInfo ServerInfo { get { return serverinfo; } set { serverinfo = value; } }

        [JsonProperty]
        public HutTaskTransferOption Option { get { return option; } set { option = value; } }

        public Action Procedure { get; set; }

        #endregion properties
    }
}