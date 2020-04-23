/******************************************************************************
* Interface of Task Manager
*
* - Task manager for tasks
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using System.Collections.Generic;

namespace Hut
{
    public delegate void NotifyStatusChangeHandler(object sender, IHutTaskAction task);

    public interface INotifyStatusChange
    {
        event NotifyStatusChangeHandler StatusChanged;
    }

    public interface IHutTaskManager : INotifyStatusChange
    {
        void create(IHutTask element);

        void create(List<IHutTask> tasks);

        void delete(string taskname);

        void deleteAll();

        IHutTask find(string taskname);

        List<IHutTask> findAll(string taskname);

        List<HutTaskResultLog> generateResults();

        List<IHutTask> Tasks { get; }
    }
}