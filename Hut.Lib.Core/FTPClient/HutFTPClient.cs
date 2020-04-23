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
using System.IO;
using System.Net;
using System.Text;

namespace Hut
{
    public class HutFTPClient : IDisposable
    {
        private string addr;
        private string user;
        private string pass;

        public void init(string addr, string port, string user, string pass, string sourcepath)
        {
            this.addr = @"ftp://" + addr + @":" + port + @"/" + sourcepath;
            this.user = user;
            this.pass = pass;
        }

        public void init(HutFTPServerInfo serverinfo)
        {
            addr = Path.Combine(@"ftp://" + serverinfo.Address + @":" + serverinfo.Port, serverinfo.ServerDir);
            user = serverinfo.User;
            pass = serverinfo.Password;
        }

        public void download(string destinationpath, HutFTPServerInfo serverinfo)
        {
            download(destinationpath, serverinfo.ServerFile, serverinfo.isBinary, serverinfo.isPassive);
        }

        // TODO: FTP exception or NULL file exception
        public void download(string destinationpath, string filename, bool binary = false, bool passive = true)
        {
            // create ftp request
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.addr + filename);

            // set user and pass
            request.Credentials = new NetworkCredential(this.user, this.pass);

            // set ftp method: download
            request.Method = WebRequestMethods.Ftp.DownloadFile;

            // set options
            request.UseBinary = binary;
            request.UsePassive = passive;
            try
            {
                // get ftp response
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                // response stream
                Stream responsestream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responsestream);

                // create output file
                FileStream file = File.Create(destinationpath + @"/" + filename);

                // stream copy
                responsestream.CopyTo(file);

                // clean up
                responsestream.Close();
                reader.Close();
                file.Close();
                response.Close();
            }
            catch (WebException e)
            {
                // file has none
                Console.WriteLine(e.Message);
            }
        }

        public void upload(string sourcepath, HutFTPServerInfo serverinfo)
        {
            upload(sourcepath, serverinfo.ServerFile, serverinfo.isBinary, serverinfo.isPassive);
        }

        public void upload(string sourcepath, string filename, bool binary = false, bool passive = true)
        {
            // create ftp request
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(this.addr + filename);

            // set user and pass
            request.Credentials = new NetworkCredential(this.user, this.pass);

            // set ftp method: download
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // set options
            request.UseBinary = binary;
            request.UsePassive = passive;
            try
            {
                // Copy the contents of the file to the request stream.
                StreamReader sourceStream = new StreamReader(sourcepath);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
            }
            catch (WebException e)
            {
                // file has none
                Console.WriteLine(e.Message);
            }
        }

        public string[] dir()
        {
            // Get the object used to communicate with the server.
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(addr);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

            // This example assumes the FTP site uses anonymous logon. // "anonymous", "janeDoe@contoso.com"
            request.Credentials = new NetworkCredential(user, pass);

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);

            string list = reader.ReadToEnd();
            //Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);

            reader.Close();
            response.Close();

            return list.Split('\n');
        }

        public void Dispose()
        {
            addr = string.Empty;
            user = string.Empty;
            pass = string.Empty;
        }

        public bool isConnected
        {
            get
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(addr);
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.Timeout = 3000;
                request.Credentials = new NetworkCredential(user, pass);

                try
                {
                    request.GetResponse();
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return true;
            }
        }
    }
}