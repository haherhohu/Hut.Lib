/******************************************************************************
* Hut Common Log
*
* - Task Common Log for Common View
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutCommonLog : IHutLog
    {
        private DateTime timetag;

        public HutCommonLog()
        {
            timetag = DateTime.Now;
        }

        [JsonProperty]
        public DateTime Time { get { return timetag; } }

        [JsonProperty]
        public string Code { get; set; }

        [JsonProperty]
        public string System { get; set; }

        [JsonProperty]
        public HutLogLevel Level { get; set; }

        [JsonProperty]
        public string Message { get; set; }

        public string PrintableLog
        {
            get { return string.Empty; }
        }
    }
}