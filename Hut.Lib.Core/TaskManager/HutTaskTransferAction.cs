/******************************************************************************
* Hut File Transfer Action
*
* - Action for File Transfer. this Action send File(s) by FTP
*
* Author : Daegung Kim
* Version: 1.1.0
* Update : 2017-07-25
******************************************************************************/

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

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
        protected string transferstatus;
        protected HutFTPServerInfo serverinfo;

        protected HutTaskTransferOption option; // file transfer options

        #endregion variables

        public HutTaskTransferAction()
        {
            initStatus();
            Procedure += () =>
            {
                List<string> exceptions = new List<string>();
                ActionStatus = HutTaskActionStatus.Processing;
                TransferStatus = @"Start Transfer Action: " + Name;
                start = DateTime.Now;

                if (serverinfo.isSSL || ServerInfo.Address.StartsWith(@"s"))
                {
                    using (HutSFTPClient client = new HutSFTPClient())
                    {
                        TransferStatus = @"Check SFTP Connection";
                        client.init(serverinfo);
                        if (!client.isConnected)
                        {
                            TransferStatus = @"SFTP Connection Failed";
                            exceptions.Add(TransferStatus);
                            updateStatus(exceptions);
                            TransferStatus = @"End Transfer Action: " + Name;
                            return;
                        }
                        TransferStatus = @"SFTP Init";
                        TransferStatus = @"Search Option: " + Option.SearchRule.ToString() + @"- " + option.SearchText;
                        TransferStatus = @"SFTP Connected";
                        TransferFiles(client, exceptions);
                    }
                }
                else
                {
                    using (HutFTPClient client = new HutFTPClient())
                    {
                        TransferStatus = @"Check FTP Connection";
                        client.init(serverinfo);
                        if (!client.isConnected)
                        {
                            TransferStatus = @"FTP Connection Failed";
                            exceptions.Add(TransferStatus);
                            updateStatus(exceptions);
                            TransferStatus = @"End Transfer Action: " + Name;
                            return;
                        }
                        TransferStatus = @"FTP Init";
                        TransferStatus = @"Search Option: " + Option.SearchRule.ToString() + @"- " + option.SearchText;
                        TransferStatus = @"FTP Connected";
                        TransferFiles(client, exceptions);
                    }
                }
            };
        }

        public List<string> getUploadFiles(string srcpath, ref List<string> exceptions)// , bool latest = false
        {
            List<string> files = Directory.GetFiles(srcpath).ToList();

            switch (option.SearchRule)
            {
                case HutSearchRule.Specified:
                    files = files.Where(w => Path.GetFileName(w).ToLower().Equals(option.SearchText.ToLower())).ToList();

                    break;

                case HutSearchRule.Contains:
                    files = files.Where(w => Path.GetFileName(w).ToLower().Contains(option.SearchText.ToLower())).ToList();
                    break;

                case HutSearchRule.Extension:
                    files = files.Where(w => Path.GetExtension(w).Substring(1).ToLower().Equals(option.SearchText.ToLower())).ToList();
                    break;

                case HutSearchRule.StartWith:
                    files = files.Where(w => Path.GetFileName(w).ToLower().StartsWith(option.SearchText.ToLower())).ToList();
                    break;

                case HutSearchRule.RegularExpression:

                    try
                    {
                        files = files.Where(w => Regex.IsMatch(w, option.SearchText)).ToList();
                    }
                    catch (Exception e)
                    {
                        exceptions.Add(@"Wrong Regex: " + e.Message);
                        files.Clear();
                    }

                    break;

                default:
                    break;
            }
            //if (files.Count == 0 && latest)
            //{
            //    exceptions.Add(Name + @": , No Matched File in Option = " + Option.SearchRule.ToString());
            //}

            return files;
        }

        public List<string> getDownloadFiles(IHutFTPClient client, string srcpath, ref List<string> exceptions)
        {
            List<string> files = client.GetFiles(srcpath).ToList();

            if (files.Count == 0)
            {
                exceptions.Add(@"No File(s) in target Directory");
            }

            return files;
        }

        public void updateStatus(List<string> exceptionmessages)
        {
            result = exceptionmessages.Count == 0 ? HutTaskActionResult.Success : HutTaskActionResult.Fail;
            end = DateTime.Now;

            if (exceptionmessages.Count > 0)
            {
                TransferStatus = @"Summary of Errors in Task: " + Name;
                foreach (string message in exceptionmessages)
                {
                    TransferStatus = @"  " + message;
                }
            }

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
            return new HutTaskTransferAction()
            {
                Parent = this.Parent,
                ActionStatus = this.ActionStatus,
                Destination = this.Destination,
                Name = this.Name,
                ServerInfo = this.ServerInfo,
                Source = this.Source,
                Option = this.Option
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

        public string TransferStatus
        {
            get
            {
                return transferstatus;
            }
            private set
            {
                if (transferstatus != value)
                {
                    transferstatus = value;
                    if (PropertyChanged != null)
                        PropertyChanged.Invoke(this, new PropertyChangedEventArgs(@"TransferStatus"));
                }
            }
        }

        private void TransferFiles(IHutFTPClient client, List<string> exceptions)
        {
            int transferfiles = 1;

            try
            {
                if (serverinfo.Direction == HutFTPTransferDirection.Upload)
                {
                    // check local path
                    if (!Directory.Exists(Path.GetDirectoryName(srcpath)))
                    {
                        TransferStatus = @"source path is not exists";
                        end = DateTime.Now;
                        result = HutTaskActionResult.Fail;
                        ActionStatus = HutTaskActionStatus.Complete;
                        return;
                    }

                    if (client is HutFTPClient && ServerInfo.Address.StartsWith(@"s"))
                    {
                        transferstatus = @"can't open server sftp:// without SSL: " + serverinfo.Address;
                        return;
                    }

                    List<string> uploaded = new List<string>();

                    do
                    {
                        // upload
                        foreach (string srcfile in getUploadFiles(srcpath, ref exceptions).Where(w => !uploaded.Contains(Path.GetFileName(w))))
                        {
                            string dstfile = string.Format(@"{0}/{1}", dstpath, Option.rename(Path.GetFileName(srcfile))).Replace(@"//", @"/");
                            TransferStatus = @"(" + ((uploaded.Count + 1).ToString()) + @")Upload Start   : " + srcfile;

                            client.upload(srcfile, dstfile);
                            TransferStatus = @"(" + ((uploaded.Count + 1).ToString()) + @")Upload Complete: " + client.Addr + dstfile;
                            uploaded.Add(Path.GetFileName(srcfile));
                        }

                        // check one more
                        if (getUploadFiles(srcpath, ref exceptions).Count <= uploaded.Count)
                            break;
                        Thread.Sleep(300);
                    } while (true);

                    TransferStatus = @"Total Sent Files: " + (uploaded.Count).ToString();

                    // cannot sent.
                    if (uploaded.Count == 0)
                    {
                        exceptions.Add(Name + @": , No Matched File in Option = " + Option.SearchRule.ToString());
                    }

                    // options
                    if ((option.StorageMethod & HutStorageMethod.DeleteFile) == HutStorageMethod.DeleteFile)
                    {
                        foreach (var src in uploaded.Select((s, i) => new { File = Path.Combine(srcpath, s), Index = (i + 1) }))
                        {
                            File.Delete(src.File);
                            TransferStatus = @"(" + (src.Index) + @")Delete Uploaded File: " + src.File;
                        }
                    }
                }
                // DOWNLOAD
                else
                {
                    foreach (string srcfile in getDownloadFiles(client, srcpath, ref exceptions))
                    {
                        string dstfile = Path.Combine(dstpath, Option.rename(Path.GetFileName(srcfile)));
                        TransferStatus = @"(" + (transferfiles.ToString()) + @")Download Start   : " + client.Addr + srcpath + srcfile;
                        client.download(string.Format(@"{0}/{1}", srcpath, srcfile), dstfile);
                        TransferStatus = @"(" + (transferfiles.ToString()) + @")Download Complete: " + dstfile;
                        client.dele(string.Format(@"{0}/{1}", srcpath, srcfile));
                        TransferStatus = @"(" + (transferfiles.ToString()) + @")Delete Complete: " + srcfile;
                        transferfiles++;
                    }
                    TransferStatus = @"Total Received Files: " + (transferfiles - 1).ToString();
                }
                TransferStatus = @"FTP Disconnected";
            }
            catch (Exception e)
            {
                //string dstfile = string.Format(@"{0}\{1}", dstpath, Option.rename(Path.GetFileName(srcfile)));
                TransferStatus = @"File Transfer Error Raised: ";
                exceptions.Add(TransferStatus);
                TransferStatus = @" Source Path: " + srcpath;
                exceptions.Add(TransferStatus);
                TransferStatus = @" Target Path: " + serverinfo.Address + @"/" + dstpath;
                exceptions.Add(TransferStatus);
                TransferStatus = @" FTP Error(s): " + e.Message;
                TransferStatus = @"               " + e.StackTrace;
                exceptions.Add(TransferStatus);
            }
            finally
            {
                TransferStatus = @"End Transfer Action: " + Name;
                updateStatus(exceptions);
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

        public string Server { get { return ServerInfo.Address; } }

        public Action Procedure { get; set; }

        public object Args { get; set; }

        #endregion properties
    }
}