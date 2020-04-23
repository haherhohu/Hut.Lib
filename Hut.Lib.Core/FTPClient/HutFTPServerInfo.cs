using Newtonsoft.Json;
using System.Xml.Serialization;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutFTPServerInfo
    {
        private string name;
        private string addr;
        private string port;
        private string user;
        private string pass;

        private bool passivemode;
        private bool binarymode;

        private string serverdir;
        private string serverfile;

        public HutFTPServerInfo()
        {
            name = string.Empty;
            addr = string.Empty;
            port = string.Empty;
            user = string.Empty;
            pass = string.Empty;

            passivemode = false;
            binarymode = false;

            serverdir = string.Empty;
            serverfile = string.Empty;
        }

        [JsonProperty]
        [XmlElement("SiteName")]
        public string Name { get { return name; } set { name = value; } }

        [JsonProperty]
        [XmlElement("IPAddress")]
        public string Address { get { return addr; } set { addr = value; } }

        [JsonProperty]
        [XmlElement("Port")]
        public string Port { get { return port; } set { port = value; } }

        [JsonProperty]
        [XmlElement("ID")]
        public string User { get { return user; } set { user = value; } }

        [JsonProperty]
        [XmlElement("Password")]
        public string Password { get { return pass; } set { pass = value; } }

        [JsonProperty]
        [XmlElement("IsPassive")]
        public bool isPassive { get { return passivemode; } set { passivemode = value; } }

        [JsonProperty]
        [XmlElement("IsBinary")]
        public bool isBinary { get { return binarymode; } set { binarymode = value; } }

        [JsonProperty]
        [XmlElement("ServerDir")]
        public string ServerDir { get { return serverdir; } set { serverdir = value; } }

        [JsonProperty]
        [XmlElement("ServerFile")]
        public string ServerFile { get { return serverfile; } set { serverfile = value; } }
    }
}