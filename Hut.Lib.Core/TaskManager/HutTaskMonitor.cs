/******************************************************************************
* Hut Task Monitor
*
* - File-Base Task Manager
*
* Author : Daegung Kim
* Version: 1.1.0
* Update : 2017-08-08
******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Hut
{
    public class HutTaskMonitor : IHutTaskManager
    {
        public List<HutFileTask> tasks;

        public HutTaskMonitor()
        {
            tasks = new List<HutFileTask>();
        }

        public List<IHutTask> Tasks { get { return tasks.OfType<IHutTask>().ToList(); } }

        public void register(List<IHutTask> selectedtasks, PropertyChangedEventHandler logging)
        {
            foreach (var task in selectedtasks)
            {
                tasks.Add(initActionStatus(task));
                task.Actions.ForEach(f => f.PropertyChanged += logging);
            }
        }

        public void unregister(PropertyChangedEventHandler logging)
        {
            foreach (HutFileTask task in tasks)
            {
                task.Actions.ForEach(f => f.PropertyChanged -= logging);
                task.Dispose();
            }
            tasks.Clear();
        }

        public void create(IHutTask task)
        {
            tasks.Add(initActionStatus(task) as HutFileTask);
        }

        public void create(List<IHutTask> tasks)
        {
            tasks.ForEach(f => this.tasks.Add(initActionStatus(f) as HutFileTask));
        }

        public void delete(string taskname)
        {
            foreach (HutFileTask task in tasks.Where(w => w.Name.Equals(taskname)))
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

        public HutFileTask initActionStatus(IHutTask task)
        {
            (task as HutFileTask).setTask();
            return (task as HutFileTask);
        }
    }
}