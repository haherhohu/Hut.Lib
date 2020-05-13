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
    public interface IHutFTPClient : IDisposable
    {
        void init(string addr, string port, string user, string pass, string sourcepath);
        void init(HutFTPServerInfo serverinfo);

        // TODO: FTP exception or NULL file exception
        void download(string serverpath, string clientpath, bool createdir = true);
        void upload(string clientpath, string serverpath);
        void dele(string destination = @"/");
        List<string> dir(string destination = @"/");

        List<string> GetFiles(string destination = @"/");

        HutFTPServerInfo Server { get; set; }
        string Addr { get; set; }
        string User { get; set; }
        string Pass { get; set; }

        bool isConnected { get; }
    }
}