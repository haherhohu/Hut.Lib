using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;

// ALL MEMBERS ARE DEPRECATED
// REFERENCE FOR NEW LOG
namespace Hut
{
    public class ScheduleModel : ICloneable, INotifyPropertyChanged
    {
        private string schName = string.Empty;
        private bool isUse = false;
        private List<string> missions = new List<string>();

        public event PropertyChangedEventHandler PropertyChanged;

        [XmlElement("ScheduleName")]
        public string SchName
        {
            get { return this.schName; }
            set { this.schName = value; this.OnNotifyPropertyChanged("SchName"); }
        }

        [XmlElement("IsUse")]
        public bool IsUse
        {
            get { return this.isUse; }
            set { this.isUse = value; this.OnNotifyPropertyChanged("IsUse"); }
        }

        [XmlElement("Missions")]
        public List<string> Missions
        {
            get { return this.missions; }
            set { this.missions = value; }
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        private void OnNotifyPropertyChanged(string p)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }

    public enum ExcPeriod
    {
        OneTime,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum OperationMode
    {
        Upload,
        Download
    }

    public class ErrorLogText
    {
        private string elogkind;
        private DateTime elogtime;
        private string econtents;

        public string ELogkind
        {
            get { return this.elogkind; }
            set { this.elogkind = value; }
        }

        public DateTime ELogtime
        {
            get { return this.elogtime; }
            set { this.elogtime = value; }
        }

        public string EContents
        {
            get { return this.econtents; }
            set { this.econtents = value; }
        }
    }

    public class EventLogText
    {
        private string logkind;
        private DateTime logtime;
        private string contents;

        public string Logkind
        {
            get { return this.logkind; }
            set { this.logkind = value; }
        }

        public DateTime Logtime
        {
            get { return this.logtime; }
            set { this.logtime = value; }
        }

        public string Contents
        {
            get { return this.contents; }
            set { this.contents = value; }
        }
    }

    public class Settings
    {
        private bool isLogSave = false;
        private bool isLogDelete = false;
        private DateTime saveLogTime;
        private DateTime refreshScheduletime;
        private int timeout;
        private int pollingPeriod;

        [XmlElement]
        public int Timeout
        {
            get { return this.timeout; }
            set { this.timeout = value; }
        }

        [XmlElement]
        public bool IsLogSave
        {
            get { return this.isLogSave; }
            set { this.isLogSave = value; }
        }

        [XmlElement]
        public bool IsLogDelete
        {
            get { return this.isLogDelete; }
            set { this.isLogDelete = value; }
        }

        [XmlElement]
        public int PollingPeriod
        {
            get { return this.pollingPeriod; }
            set { this.pollingPeriod = value; }
        }

        [XmlElement]
        public DateTime SaveLogTime
        {
            get { return this.saveLogTime; }
            set { this.saveLogTime = value; }
        }

        [XmlElement]
        public DateTime RefreshScheduleTime
        {
            get { return this.refreshScheduletime; }
            set { this.refreshScheduletime = value; }
        }
    }
}