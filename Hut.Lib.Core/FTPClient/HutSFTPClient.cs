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
using Renci.SshNet;


namespace Hut
{
    public class HutSFTPClient : IHutFTPClient
    {
        private bool connectionstatus;
        public const int MAX_RETRY = 3;

        public HutSFTPClient()
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
            using (SftpClient client = new SftpClient(Server.IPv4, Convert.ToInt32(Server.Port), Server.User, Server.Password))
            {
                client.ConnectionInfo.Timeout = new TimeSpan(Server.Timeout * 10000);
                client.Connect();
                for (int retry = 0; retry < MAX_RETRY; retry++)
                {
                    try
                    {
                        if (createdir)
                            Directory.CreateDirectory(Path.GetDirectoryName(clientpath));

                        // response stream
                        using (Stream filestream = File.Create(clientpath))
                        {
                            client.DownloadFile(serverpath, filestream);
                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(@"sftp error:" + ex.Message);
                        Thread.Sleep(1000);
                        continue;
                    }
                }
                client.Disconnect();
            }
        }

        public void upload(string clientpath, string serverpath)
        {
            using (SftpClient client = new SftpClient(Server.IPv4, Convert.ToInt32(Server.Port), Server.User, Server.Password))
            {
                client.ConnectionInfo.Timeout = new TimeSpan(Server.Timeout * 10000);
                client.Connect();
                for (int retry = 0; retry < MAX_RETRY; retry++)
                {
                    try
                    {
                        // response stream
                        using (Stream filestream = new FileStream(clientpath, FileMode.Open))
                        {
                            client.UploadFile(filestream, @"." + serverpath, true);
                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(@"sftp error:" + ex.Message);
                        Thread.Sleep(1000);
                        continue;
                    }
                }
                client.Disconnect();
            }
        }

        public List<string> dir(string destination = @"/")
        {
            using (SftpClient client = new SftpClient(Server.IPv4, Convert.ToInt32(Server.Port), Server.User, Server.Password))
            {
                try
                {
                    client.Connect();
                    var files = client.ListDirectory(@"." + destination);
                    client.Disconnect();

                    return files.Select(s => s.Name).ToList();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"sftp error:" + ex.Message);

                    return new List<string>();
                }
            }
        }

        public List<string> GetFiles(string destination)
        {
            return dir(destination);
        }

        public void dele(string destination = @"/")
        {
            using (SftpClient client = new SftpClient(Server.IPv4, Convert.ToInt32(Server.Port), Server.User, Server.Password))
            {
                try
                {
                    client.Connect();
                    client.DeleteFile(@"." + destination);
                    client.Disconnect();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"sftp error:" + ex.Message);
                }
            }
        }

        public bool checkConnection()
        {
            using (SftpClient client = new SftpClient(Server.IPv4, Convert.ToInt32(Server.Port), Server.User, Server.Password))
            {
                try
                {
                    client.Connect();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                connectionstatus = client.IsConnected;
                client.Disconnect();
            }

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