/******************************************************************************
* Hut Task Enumerations
*
* - Define Enums
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

namespace Hut
{
    public enum HutTaskActionStatus
    {
        Ready,
        Processing,
        Complete
    }

    public enum HutTaskActionType
    {
        UserDefine, // none
        Archive,    // with src - dst dir (local or nas)
        Transfer,   // with src - dst dir (file transfer)
        Execute     // with execution dir and options and input files.
    }

    public enum HutTaskType
    {
        Time, // Time-Based Task for Scheduler
        File, // File-Based Task for Monitor
    }

    public enum HutTaskActionResult
    {
        Success, // for Time Task Result 0
        Fail,    // for Time Task Result 1
        Waiting,
        NotApplicable = 16,
    }

    public enum HutSearchRule
    {
        None,
        All,
        Extension,
        StartWith,
        Contains,
        RegularExpression,
    }

    public enum HutStorageMethod
    {
        None,
        MoveFile,
        DeleteFile,
    }

    public enum HutTaskRepetition
    {
        Once,
        Minutely,
        Hourly,
        Daily,
        Weekly
        // for later.
        //Monthly,
        //MonthlyDOW
    }

    public enum HutSaveFormat
    {
        Json,
        Xml
    }
}