/******************************************************************************
* Hut Result Log
*
* - Task Result Log for Result View
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
    public class HutTaskResultLog : IHutLog
    {
        private DateTime timetag;

        public HutTaskResultLog()
        {
            timetag = DateTime.Now;
        }

        [JsonProperty]
        public int ID { get; set; }

        [JsonProperty]
        public HutTaskType TaskType { get; set; }

        [JsonProperty]
        public string Group { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public int ActionID { get; set; }

        [JsonProperty]
        public HutTaskActionType ActionType { get; set; }

        [JsonProperty]
        public HutTaskActionStatus Status { get; set; }

        [JsonProperty]
        public HutTaskActionResult Result { get; set; }

        [JsonProperty]
        public DateTime ExecuteTime { get; set; }

        [JsonProperty]
        public DateTime CompleteTime { get; set; }

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

        public void updateResult(IHutTaskAction action)
        {
            Status = action.ActionStatus;
            Result = action.ActionResult;
            ExecuteTime = action.StartTime;
            CompleteTime = action.EndTime;
        }

        public void updateResultWithoutTime(IHutTaskAction action)
        {
            Status = action.ActionStatus;
            Result = action.ActionResult;
        }
    }
}