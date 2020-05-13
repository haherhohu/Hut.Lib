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
            initStatus();
        }

        public HutTaskActionStatus ActionStatus
        {
            get
            {
                return status;
            }
            set
            {
                if (status != value)
                {
                    status = value;
                    if (PropertyChanged != null)
                        PropertyChanged.Invoke(Parent, new PropertyChangedEventArgs(@"ActionStatus"));
                }
            }
        }

        public HutTaskResultLog generateResult()
        {
            return new HutTaskResultLog()
            {
                Level = HutLogLevel.Info,
                Message = string.Format(@"Task {0}({1}) of {2} is {3}.", Name, Parent.Group, Parent.TaskType.ToString(), ActionStatus.ToString()),
                ID = Parent.ID,
                Name = Parent.Name,
                Group = Parent.Group,
                Status = ActionStatus,
                Result = ActionResult,
                ExecuteTime = StartTime,
                CompleteTime = EndTime,
                TaskType = Parent.TaskType,
                ActionID = GetHashCode(),
                ActionType = ActionType,
            };
        }

        public object Clone()
        {
            return new HutTaskUserDefineAction()
            {
                Name = this.Name,
                Parent = this.Parent,
                ActionStatus = this.ActionStatus,
                Procedure = this.Procedure,
            };
        }

        public void initStatus(HutTaskActionResult r = HutTaskActionResult.NotApplicable)
        {
            status = HutTaskActionStatus.Ready;
            result = r;
        }

        public void resetStatus()
        {
            status = HutTaskActionStatus.Ready;
            result = HutTaskActionResult.NotApplicable;
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
        public object Args { get; set; }

        #endregion properties
    }
}