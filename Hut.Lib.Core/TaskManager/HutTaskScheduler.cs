/******************************************************************************
* Hut Task Scheduler
*
* - Time-Base Task Manager
*
* Author : Daegung Kim
* Version: 1.0.2
* Update : 2017-04-12
******************************************************************************/

using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hut
{
    public class HutTaskScheduler : IHutTaskManager
    {
        public List<HutTimeTask> tasks;

        public event NotifyStatusChangeHandler StatusChanged;

        // deprecated with update method
        public TaskService service;

        public HutTaskScheduler()
        {
            tasks = new List<HutTimeTask>();
            service = new TaskService();
        }

        public List<IHutTask> Tasks { get { return tasks.OfType<IHutTask>().ToList(); } }

        public void create(IHutTask task)
        {
            tasks.Add(initActionStatus(task) as HutTimeTask);
        }

        public void create(List<IHutTask> tasks)
        {
            tasks.ForEach(f => this.tasks.Add(initActionStatus(f) as HutTimeTask));
        }

        #region deprecated. but using until update with HwndMessage. Not Today

        public Task getTask(string taskname)
        {
            if (getServiceFolder() != null)
                return getServiceFolder().GetTasks().Where(w => w.Name.Equals(taskname)).FirstOrDefault();
            else
                return null;
        }

        public Task[] getTasks()
        {
            using (TaskService service = new TaskService())
            {
                if (getServiceFolder() != null)
                    return getServiceFolder()
                                  .GetTasks()
                                  .Where(w => w.NextRunTime >= DateTime.Now)
                                  .OrderBy(o => o.NextRunTime)
                                  .ToArray();
                else
                    return null;
            }
        }

        private TaskFolder getServiceFolder()
        {
            return service.GetFolder(HutTimeTask.TASK_DIR);
        }

        public void updateStatus()
        {
            foreach (HutTimeTask task in tasks.Where(w => w.Status != HutTaskActionStatus.Complete))
                task.updateStatus(getTask(task.Name));
        }

        #endregion deprecated. but using until update with HwndMessage. Not Today

        public void delete(string taskname)
        {
            foreach (HutTimeTask task in tasks.Where(w => w.Name.Equals(taskname)))
            {
                task.Actions.ForEach(f => f.PropertyChanged -= ActionStatusPropertyChanged);
                task.Dispose();
            }
            tasks.RemoveAll(r => r.Name.Equals(taskname));
        }

        public void deleteAll()
        {
            foreach (HutTimeTask task in tasks)
            {
                task.Actions.ForEach(f => f.PropertyChanged -= ActionStatusPropertyChanged);
                task.Dispose();
            }
            tasks.Clear();
        }

        public IHutTask find(string taskname)
        {
            return tasks.First(f => f.Name.Equals(taskname));
        }

        public List<IHutTask> findAll(string taskname)
        {
            return tasks.Where(w => w.Name.Equals(taskname)).OfType<IHutTask>().ToList();
        }

        public IHutTask initActionStatus(IHutTask task)
        {
            (task as HutTimeTask).setTask();
            task.Actions.ForEach(f => { f.PropertyChanged += ActionStatusPropertyChanged; f.ActionStatus = HutTaskActionStatus.Ready; f.Parent = task; });
            return task;
        }

        private void ActionStatusPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StatusChanged?.Invoke(this, (sender as IHutTaskAction));
        }

        public List<HutTaskResultLog> generateResults()
        {
            List<HutTaskResultLog> results = new List<HutTaskResultLog>();
            tasks.Select(s => s.generateResults()).ToList().ForEach(f => results.AddRange(f));

            return results;
        }

        public List<HutTaskActionStatus> TaskActionStatus
        {
            get
            {
                List<IHutTaskAction> actions = new List<IHutTaskAction>();

                foreach (IHutTask task in tasks)
                    actions.AddRange(task.Actions);

                return actions.Select(s => s.ActionStatus).ToList();
            }
        }
    }
}