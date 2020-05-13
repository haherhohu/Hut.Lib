using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Hut
{
    public enum HutFTPTransferDirection
    {
        Upload,
        Download,
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class HutFTPServerInfo
    {
        private string name;
        private string addr;
        private string port;
        private string user;
        private string pass;
        private string path;

        private int timeout;

        private bool sslmode;
        private bool passivemode;
        private bool binarymode;
        private HutFTPTransferDirection transferdirection;

        public HutFTPServerInfo()
        {
            name = string.Empty;
            addr = string.Empty;
            port = @"21";
            user = string.Empty;
            pass = string.Empty;

            timeout = 3000;

            passivemode = false;
            binarymode = false;
            sslmode = false;
        }

        public string makeURL(string addr)
        {
            if (addr.ToLower().StartsWith(@"ftp") || addr.ToLower().StartsWith(@"sftp"))
            {
                return addr;
            }

            return @"ftp://" + addr;
        }

        public string changePort(string port)
        {
            return makeURL(addr) + @":" + port;
        }

        public string addDirectory(string dir)
        {
            return changePort(port) + @"/" + dir;
        }

        [JsonProperty]
        [XmlElement(@"SiteName")]
        public string Name { get { return name; } set { name = value; } }

        [JsonProperty]
        [XmlElement(@"IPAddress")]
        public string Address { get { return addr; } set { addr = makeURL(value); } }

        public string IPv4
        {
            get
            {
                return addr.Contains(@"//") ?
                (addr.Substring(addr.IndexOf(@"//") + 2)) :
                (addr);
            }
        }

        [JsonProperty]
        [XmlElement(@"Port")]
        public string Port { get { return port; } set { port = value; } }

        [JsonProperty]
        [XmlElement(@"ID")]
        public string User { get { return user; } set { user = value; } }

        [JsonProperty]
        [XmlElement(@"Password")]
        public string Password { get { return pass; } set { pass = value; } }

        public string Directory { get { return path; } set { path = value; } }

        public string AddressWithDirectory { get { return addr + @"/" + path; } }

        [JsonProperty]
        [XmlElement(@"Timeout")]
        public int Timeout { get { return timeout; } set { timeout = value; } }

        [JsonProperty]
        [XmlElement(@"IsPassive")]
        public bool isPassive { get { return passivemode; } set { passivemode = value; } }

        [JsonProperty]
        [XmlElement(@"IsBinary")]
        public bool isBinary { get { return binarymode; } set { binarymode = value; } }

        [JsonProperty]
        [XmlElement(@"IsSSL")]
        public bool isSSL { get { return sslmode; } set { sslmode = value; } }

        [JsonProperty]
        public HutFTPTransferDirection Direction { get { return transferdirection; } set { transferdirection = value; } }
    }
}