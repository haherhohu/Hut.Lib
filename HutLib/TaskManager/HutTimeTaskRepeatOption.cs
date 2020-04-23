/******************************************************************************
* Hut Time Task Repeat Option
*
* - Repeat Option Setting and make Trigger for Repetition
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using System;

namespace Hut
{
    [JsonObject(MemberSerialization.OptIn)]
    public class HutTimeTaskRepeatOption
    {
        [JsonProperty]
        public int Interval { get; set; }

        [JsonProperty]
        public HutTaskRepetition Repeat { get; set; }

        [JsonProperty]
        public DaysOfTheWeek DaysOfWeek { get; set; }

        public HutTimeTaskRepeatOption()
        {
            Interval = 0;
            Repeat = HutTaskRepetition.Once;
            DaysOfWeek = (DaysOfTheWeek)(1 << ((int)DateTime.Today.DayOfWeek));
        }

        public Trigger makeTrigger(DateTime start, DateTime end)
        {
            switch (Repeat)
            {
                case HutTaskRepetition.Minutely:
                    return new TimeTrigger()
                    {
                        StartBoundary = start,
                        EndBoundary = end,
                        Repetition = new RepetitionPattern(TimeSpan.FromMinutes(Interval), TimeSpan.Zero),
                        Enabled = true
                    };

                case HutTaskRepetition.Hourly:
                    return new TimeTrigger()
                    {
                        StartBoundary = start,
                        EndBoundary = end,
                        Repetition = new RepetitionPattern(TimeSpan.FromHours(Interval), TimeSpan.Zero),
                        Enabled = true
                    };

                case HutTaskRepetition.Daily:
                    return new DailyTrigger()
                    {
                        DaysInterval = (short)Interval,
                        StartBoundary = start,
                        EndBoundary = end,
                        Enabled = true
                    };

                case HutTaskRepetition.Weekly:
                    return new WeeklyTrigger()
                    {
                        WeeksInterval = (short)Interval,
                        DaysOfWeek = DaysOfWeek,
                        StartBoundary = start,
                        EndBoundary = end,
                        Enabled = true
                    };

                default:
                    return new TimeTrigger()
                    {
                        StartBoundary = start,
                        EndBoundary = end,
                        Enabled = true,
                    };
            }
        }

        public TimeSpan makeIntervalWithUnit()
        {
            switch (Repeat)
            {
                case HutTaskRepetition.Minutely:
                    return TimeSpan.FromMinutes(Interval);

                case HutTaskRepetition.Hourly:
                    return TimeSpan.FromHours(Interval);

                case HutTaskRepetition.Daily:
                    return TimeSpan.FromDays(Interval);

                case HutTaskRepetition.Weekly:
                    return TimeSpan.FromDays(Interval * 7);

                default:
                    return TimeSpan.Zero;
            }
        }
    }
}