/******************************************************************************
* Interface of Task Manager
*
* - Task manager for tasks
*
* Author : Daegung Kim
* Version: 1.0.0
* Update : 2017-04-12
******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Hut
{
    //(object sender, IHutTaskAction task);

    //public interface INotifyStatusChange
    //{
    //    event PropertyChanged StatusChanged;
    //}

    public interface IHutTaskManager
    {
        void register(List<IHutTask> tasks, PropertyChangedEventHandler logging);

        void create(IHutTask element);

        void create(List<IHutTask> tasks);

        void delete(string taskname);

        void unregister(PropertyChangedEventHandler logging);

        IHutTask find(string taskname);

        List<IHutTask> findAll(string taskname);

        //        List<HutTaskResultLog> generateResults();

        List<IHutTask> Tasks { get; }
    }
}