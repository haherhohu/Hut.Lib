/******************************************************************************
* Hut Task Monitor
*
* - File-Base Task Manager
*
* Author : Daegung Kim
* Version: 1.0.2
* Update : 2017-04-12
******************************************************************************/

using System.Collections.Generic;
using System.Linq;

namespace Hut
{
    public class HutTaskMonitor : IHutTaskManager
    {
        public List<HutFileTask> tasks;

        public event NotifyStatusChangeHandler StatusChanged;

        public HutTaskMonitor()
        {
            tasks = new List<HutFileTask>();
        }

        public List<IHutTask> Tasks { get { return tasks.OfType<IHutTask>().ToList(); } }

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
                task.Actions.ForEach(f => f.PropertyChanged -= ActionStatusPropertyChanged);
                task.Dispose();
            }
            tasks.RemoveAll(r => r.Name.Equals(taskname));
        }

        public void deleteAll()
        {
            foreach (HutFileTask task in tasks)
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
            (task as HutFileTask).setTask();
            task.Actions.ForEach(f => { f.PropertyChanged += ActionStatusPropertyChanged; f.ActionStatus = HutTaskActionStatus.Ready; f.Parent = task; });
            return task;
        }

        private void ActionStatusPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // maybe sender is action.
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