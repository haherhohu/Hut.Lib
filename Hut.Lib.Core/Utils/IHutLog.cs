/******************************************************************************
* Interface of Log
*
* - Basic Log
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using System;

namespace Hut
{
    public enum HutLogLevel
    {
        Debug,
        Info,
        Warning,
        Error,
        Fatal
    }

    public interface IHutLog
    {
        DateTime Time { get; }

        HutLogLevel Level { get; set; }

        string Message { get; set; }

        string PrintableLog { get; }
    }
}