/******************************************************************************
* Hut User Define Action
*
* - Action for User Define. this Action has no default Action.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTaskUserDefineAction : IHutTaskAction
    {
        #region variables

        public event PropertyChangedEventHandler PropertyChanged;

        protected DateTime start;
        protected DateTime end;
        protected HutTaskActionStatus status;
        protected HutTaskActionResult result;

        #endregion variables

        public HutTaskUserDefineAction()
        {
            status = HutTaskActionStatus.Ready;
            result = HutTaskActionResult.NotApplicable;
        }

        public HutTaskActionStatus ActionStatus
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    PropertyChanged?.Invoke(Parent, new PropertyChangedEventArgs(@"ActionStatus"));
                }
            }
        }

        #region properties

        public IHutTask Parent { get; set; }

        [JsonProperty]
        public DateTime StartTime { get { return start; } }

        [JsonProperty]
        public DateTime EndTime { get { return end; } }

        [JsonProperty]
        public HutTaskActionType ActionType { get { return HutTaskActionType.UserDefine; } }

        [JsonProperty]
        public HutTaskActionResult ActionResult { get { return result; } }

        [JsonProperty]
        public string Name { get; set; }

        public Action Procedure { get; set; }

        #endregion properties
    }
}