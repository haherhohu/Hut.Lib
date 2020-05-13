/******************************************************************************
* Hut Task Scheduler
*
* - Time-Base Task Manager
*
* Author : Daegung Kim
* Version: 1.0.2
* Update : 2017-04-12
******************************************************************************/

using System;
using System.Timers;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Hut
{
    public class HutTaskScheduler : IHutTaskManager
    {
        public List<HutTimeTask> tasks;
        //        private Timer timer;

        public HutTaskScheduler()
        {
            tasks = new List<HutTimeTask>();
            //            timer = new Timer(300);
        }

        //public HutTaskScheduler2(Timer timer)
        //{
        //    this.timer = timer;
        //    //            timer.Elapsed += updateStatus;
        //}

        public List<IHutTask> Tasks { get { return tasks.OfType<IHutTask>().ToList(); } }

        public void register(List<IHutTask> selectedtasks, PropertyChangedEventHandler logging)
        {
            //            timer.Elapsed += updateStatus;

            foreach (var task in selectedtasks)
            {
                tasks.Add(initActionStatus(task));
                task.Actions.ForEach(f => f.PropertyChanged += logging);
            }
        }

        public void unregister(PropertyChangedEventHandler logging)
        {
            //            timer.Elapsed -= updateStatus;

            foreach (HutTimeTask task in tasks)
            {
                task.Actions.ForEach(f => f.PropertyChanged -= logging);
                task.Dispose();
            }
            tasks.Clear();
        }

        public void create(IHutTask task)
        {
            tasks.Add(initActionStatus(task) as HutTimeTask);
        }

        public void create(List<IHutTask> tasks)
        {
            tasks.ForEach(f => this.tasks.Add(initActionStatus(f) as HutTimeTask));
        }

        public void delete(string taskname)
        {
            foreach (HutTimeTask task in tasks.Where(w => w.Name.Equals(taskname)))
            {
                task.Dispose();
            }
            tasks.RemoveAll(r => r.Name.Equals(taskname));
        }

        public IHutTask find(string taskname)
        {
            return tasks.First(f => f.Name.Equals(taskname));
        }

        public List<IHutTask> findAll(string taskname)
        {
            return tasks.Where(w => w.Name.Equals(taskname)).OfType<IHutTask>().ToList();
        }

        public HutTimeTask initActionStatus(IHutTask task)
        {
            (task as HutTimeTask).setTask();
            return (task as HutTimeTask);
        }

        public void updateStatus()
        {
            foreach (HutTimeTask task in tasks.Where(w => (!w.isComplete) && w.isTimeToReady))
            {
                if (task.RemainTime <= TimeSpan.Zero)
                {
                    task.ActionRaised();
                }
            }
        }
    }
}