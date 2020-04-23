/******************************************************************************
* Hut Sending Log
*
* - Task Sending Log for Sending View
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
    public class HutTransferLog : IHutLog
    {
        private DateTime timetag;

        public HutTransferLog()
        {
            timetag = DateTime.Now;
        }

        [JsonProperty]
        public HutTaskActionStatus Status { get; set; }

        [JsonProperty]
        public string Source { get; set; }

        [JsonProperty]
        public string Destination { get; set; }

        [JsonProperty]
        public DateTime Time { get { return timetag; } }

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