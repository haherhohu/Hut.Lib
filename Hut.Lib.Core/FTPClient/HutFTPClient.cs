/******************************************************************************
 * Hut FTP Client
 *
 * - FTP Client
 *
 * Author : Daegung Kim
 * Version: 1.0.0
 * Update : 2016-12-22
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace Hut
{
    public class HutFTPClient : IHutFTPClient
    {
        private bool connectionstatus;
        public const int MAX_RETRY = 3;

        public HutFTPClient()
        {
            Server = new HutFTPServerInfo();
        }

        public void init(string addr, string port, string user, string pass, string sourcepath)
        {
            Addr = @"ftp://" + addr + @":" + port + @"/" + sourcepath;
            User = user;
            Pass = pass;
        }

        public void init(HutFTPServerInfo serverinfo)
        {
            Server = serverinfo;
            checkConnection();
        }

        // TODO: FTP exception or NULL file exception
        public void download(string serverpath, string clientpath, bool createdir = true)
        {
            // create ftp request
            string uri = (Server.Address + @"/" + serverpath).Replace(@"\", @"/");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);

            //sftp
            request.EnableSsl = Server.Address.StartsWith(@"s");
            // set user and pass
            request.Credentials = new NetworkCredential(User, Pass);

            // set ftp method: download
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //            request.Method = WebRequestMethods.Ftp.GetFileSize;

            // set options
            request.UseBinary = Server.isBinary;
            request.UsePassive = Server.isPassive;
            request.Timeout = Server.Timeout;
            //request.EnableSsl = false;
            FtpWebResponse response = null;

            for (int retry = 0; retry < MAX_RETRY; retry++)
            {
                try
                {
                    // get ftp response
                    response = (FtpWebResponse)request.GetResponse();

                    // response stream
                    using (Stream responsestream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(responsestream);
                        if (createdir)
                            Directory.CreateDirectory(Path.GetDirectoryName(clientpath));

                        // create output file
                        FileStream file = File.Create(clientpath);

                        // stream copy
                        responsestream.CopyTo(file);

                        // clean up
                        reader.Close();
                        file.Close();
                    }
                    break;
                }
                catch (WebException webex)
                {
                    if ((response != null) && (((response.StatusCode & FtpStatusCode.ActionAbortedLocalProcessingError) == FtpStatusCode.ActionAbortedLocalProcessingError) || // 451
                                               ((response.StatusCode & FtpStatusCode.ActionNotTakenFileUnavailableOrBusy) == FtpStatusCode.ActionNotTakenFileUnavailableOrBusy) || // 450
                                               ((response.StatusCode & FtpStatusCode.CantOpenData) == FtpStatusCode.CantOpenData) || //425
                                               ((response.StatusCode & FtpStatusCode.ConnectionClosed) == FtpStatusCode.ConnectionClosed))) //426
                    {
                        Console.WriteLine(webex.Message);
                        Thread.Sleep(1000);
                        continue;
                    }
                }
                finally
                {
                    if (response != null)
                        response.Close();
                    request.Abort();
                }
            }
        }

        public void upload(string clientpath, string serverpath)
        {
            // check file size
            //int size = (int)new FileInfo(clientpath).Length;

            // (serverpath.StartsWith(@"\") || serverpath.StartsWith(@"/") ? serverpath :
            string uri = (Server.Address + @"/" + serverpath).Replace(@"\", @"/");

            FtpWebResponse response = null;
            //string uri = Path.Combine(Server.Address, serverpath);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            //sftp
            request.EnableSsl = Server.Address.StartsWith(@"s");
            // set user and pass
            request.Credentials = new NetworkCredential(this.User, this.Pass);
            // set options
            request.UseBinary = Server.isBinary;
            request.UsePassive = Server.isPassive;
            request.Timeout = Server.Timeout;
            //request.KeepAlive = true;
            // set ftp method: download
            request.Method = WebRequestMethods.Ftp.UploadFile;
            // Copy the contents of the file to the request stream.

            byte[] filecontents = null;
            try
            {
                //FileStream file = new FileStream(clientpath, FileMode.Open, FileAccess.Read, FileShare.Read);
                filecontents = File.ReadAllBytes(clientpath);
            }
            catch(Exception e)
            {
                throw e;
            }
            request.ContentLength = filecontents.Length;

            for (int retry = 0; retry < MAX_RETRY; retry++)
            {
                try
                {
                    using (Stream s = request.GetRequestStream())
                    {
                        s.Write(filecontents, 0, filecontents.Length);
                    }

                    /* //previous version. with text
                    StreamReader sourceStream = new StreamReader(clientpath);
                    byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                    sourceStream.Close();
                    request.ContentLength = fileContents.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(fileContents, 0, fileContents.Length);
                    requestStream.Close();
                    */
                    response = (FtpWebResponse)request.GetResponse();
                    break;
                }
                catch (WebException webex)
                {
                    if ((response != null) && (((response.StatusCode & FtpStatusCode.ActionAbortedLocalProcessingError) == FtpStatusCode.ActionAbortedLocalProcessingError) || // 451
                                               ((response.StatusCode & FtpStatusCode.ActionNotTakenFileUnavailableOrBusy) == FtpStatusCode.ActionNotTakenFileUnavailableOrBusy) || // 450
                                               ((response.StatusCode & FtpStatusCode.CantOpenData) == FtpStatusCode.CantOpenData) || //425
                                               ((response.StatusCode & FtpStatusCode.ConnectionClosed) == FtpStatusCode.ConnectionClosed))) //426
                    {
                        Console.WriteLine(webex.Message);
                        Thread.Sleep(1000);
                        continue;
                    }
                    else
                    {
                        throw webex;
                    }
                }
                finally
                {
                    if (response != null)
                        response.Close();
                    request.Abort();
                }
            }
        }

        public List<string> dir(string destination = @"/")
        {
            string uri = (Server.Address + @"/" + destination).Replace(@"\", @"/");
            //string uri = Server.Address;
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            // This example assumes the FTP site uses anonymous logon. // "anonymous", "janeDoe@contoso.com"
            request.Credentials = new NetworkCredential(User, Pass);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string list = reader.ReadToEnd();

            //Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);

            reader.Close();
            response.Close();

            return list.Split('\n').Where(w => !string.IsNullOrEmpty(w)).ToList();
        }

        public List<string> GetFiles(string destination)
        {
            List<string> fileinfo = dir(destination).Where(w => w.StartsWith("-")).ToList();

            if (fileinfo != null && fileinfo.Count > 0)
            {
                string tester = fileinfo.First();
                string check = findExpression(fileinfo.First(), @" [0-9]+:[0-9]+ ");
                int idx = tester.IndexOf(check) + check.Length;

                return fileinfo.Select(s => s.Substring(idx).Replace("\r", "")).ToList();
            }

            return fileinfo;
        }

        public void dele(string destination = @"/")
        {
            string uri = (Server.Address + @"/" + destination).Replace(@"\", @"/");

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);

            //If you need to use network credentials
            request.Credentials = new NetworkCredential(User, Pass);
            //additionally, if you want to use the current user's network credentials, just use:
            //System.Net.CredentialCache.DefaultNetworkCredentials

            request.Method = WebRequestMethods.Ftp.DeleteFile;
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Delete status: {0}", response.StatusDescription);
            response.Close();
        }

        public bool checkConnection()
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Server.Address);
            request.Credentials = new NetworkCredential(User, Pass);
            request.Timeout = Server.Timeout;
            request.ReadWriteTimeout = Server.Timeout;
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.EnableSsl = Server.Address.StartsWith(@"s");
            try
            {
                request.GetResponse();
                connectionstatus = true;
            }
            catch (WebException e)
            {
                Console.WriteLine(e.Message);
                connectionstatus = false;
            }

            request.Abort();
            return connectionstatus;
        }

        public void Dispose()
        {
            Server = null;
        }

        public HutFTPServerInfo Server { get; set; }
        public string Addr { get { return Server.Address; } set { Server.Address = value; } }
        public string User { get { return Server.User; } set { Server.User = value; } }
        public string Pass { get { return Server.Password; } set { Server.Password = value; } }

        public bool isConnected
        {
            get
            {
                return connectionstatus;
            }
        }

        // regex utils from deep-south
        protected string findExpression(string token, string regularexpression)
        {
            string result = string.Empty;
            Match match = new Regex(regularexpression).Match(token);

            if (match.Success)
                result = match.Value;

            return result;
        }

        protected string removeSpaces(string token, string replace = @" ")
        {
            return Regex.Replace(token, @"\s+", replace);
        }

        protected string[] splitTextBySpaces(string text)
        {
            return removeSpaces(text).Split(' ');
        }

        protected string[] splitTextByColons(string text)
        {
            return removeSpaces(text).Split(':');
        }
    }
}