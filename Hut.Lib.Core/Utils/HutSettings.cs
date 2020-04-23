/******************************************************************************
* Hut Settings
*
* - Settings for K6.MCE.FDS. Move it Later.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System;

namespace Hut
{
    public class HutSettings
    {
        // HutSetting
        [JsonProperty]
        public string SettingPath { get; set; }

        // HutLoggerSettings
        [JsonProperty]
        public DateTime LoggingTime { get; set; }

        [JsonProperty]
        public bool DeleteLog { get; set; }

        // HutResetSettings
        [JsonProperty]
        public DateTime ResetTime { get; set; }

        // HutArchiveSettings
        public string HomePath { get; set; }

        public string LogPath { get; set; }

        public void Load()
        {
        }

        public void Save()
        {
        }
    }
}