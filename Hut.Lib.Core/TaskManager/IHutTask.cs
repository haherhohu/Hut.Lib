/******************************************************************************
* Interface of Task
*
* - Basic Task Interface
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using System;
using System.Collections.Generic;

namespace Hut
{
    public interface IHutTask : IDisposable
    {
        // general Task Info
        bool Enable { get; set; }

        int ID { get; set; }

        string Name { get; set; }

        string Group { get; set; }

        string Description { get; set; }

        HutTaskType TaskType { get; }

        uint RepeatCount { get; }

        void resetCount();

        List<IHutTaskResult> Results { get; set; }

        HutTaskActionResult LastResult { get; set; }

        // deprecated
        List<HutTaskResultLog> generateResults();

        // Task Actions
        List<IHutTaskAction> Actions { get; set; }

        List<HutTaskExecuteAction> ExecuteActions { get; }
        List<HutTaskTransferAction> TransferActions { get; }
        List<HutTaskArchiveAction> ArchiveActions { get; }
    }
}