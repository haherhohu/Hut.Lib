/******************************************************************************
* Interface of Task Action
*
* - Task Action, What to do on task.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using System;
using System.ComponentModel;

namespace Hut
{
    public interface IHutTaskAction : INotifyPropertyChanged
    {
        IHutTask Parent { get; set; }

        DateTime StartTime { get; }

        DateTime EndTime { get; }

        string Name { get; set; }

        HutTaskActionType ActionType { get; }

        // action status must be update last. this is trigger for update others.
        HutTaskActionStatus ActionStatus { get; set; }

        HutTaskActionResult ActionResult { get; }

        Action Procedure { get; }
    }
}