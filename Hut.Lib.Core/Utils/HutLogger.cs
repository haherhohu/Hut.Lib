/******************************************************************************
* Hut Logger
*
* - Logger for IHutLog Type.
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Hut
{
    public class HutLogger<HutLogType> : ObservableCollection<HutLogType> where HutLogType : IHutLog
    {
        //protected HutSaveFormat saveformat;

        public HutLogger()
            : base()
        {
        }

        public new void Add(HutLogType log)
        {
            base.Add(log);
        }

        public void Save(string filename)
        {
            if (File.Exists(filename))
                File.Delete(filename);

            HutJsonFile<HutLogger<HutLogType>>.write(filename, this, true);
        }

        public void Load(string filename)
        {
            Logs = HutJsonFile<HutLogger<HutLogType>>.read(filename, true).Logs;
        }

        [JsonProperty]
        [XmlElement(@"Log")]
        public List<HutLogType> Logs
        {
            get { return this.ToList(); }
            set { Clear(); foreach (HutLogType log in value) Add(log); }
        }
    }
}