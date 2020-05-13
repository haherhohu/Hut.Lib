/******************************************************************************
* Interface of Task Action
*
* - Task Action, What to do on task.
*
* Author : Daegung Kim
* Version: 1.0.1
* Update : 2017-09-08
******************************************************************************/

using System;
using System.ComponentModel;

namespace Hut
{
    public interface IHutTaskAction : INotifyPropertyChanged, ICloneable
    {
        IHutTask Parent { get; set; }

        DateTime StartTime { get; }

        DateTime EndTime { get; }

        string Name { get; set; }

        HutTaskActionType ActionType { get; }

        // action status must be update last. this is trigger for update others.
        HutTaskActionStatus ActionStatus { get; set; }

        HutTaskActionResult ActionResult { get; }

        HutTaskResultLog generateResult();

        Action Procedure { get; }

        object Args { get; set; }

        void initStatus(HutTaskActionResult result = HutTaskActionResult.NotApplicable);

        void resetStatus();
    }
}