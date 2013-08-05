///The Worker Library gives a set of classes to do threaded work.
///Copyright (C) 2013  Alexander Lyons

///This program is free software: you can redistribute it and/or modify
///it under the terms of the GNU General Public License as published by
///the Free Software Foundation, either version 3 of the License, or
///any later version.

///This program is distributed in the hope that it will be useful,
///but WITHOUT ANY WARRANTY; without even the implied warranty of
///MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
///GNU General Public License for more details.

///You should have received a copy of the GNU General Public License
///along with this program.  If not, see <http://www.gnu.org/licenses/>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WorkerLibrary
{
    public enum WorkerStatus
    {
        Initialized,
        Working,
        ShouldPause,
        Paused,
        ShouldResume,
        ShouldStop,
        CompletedSuccess,
        CompletedStop,
        CompletedError,
        Undefined
    }

    public abstract class Worker : INotifyPropertyChanged
    {
        #region Variables
        private static List<int> activeIDs = new List<int>();
        List<Exception> errors;
        WorkerStatus status;
        #endregion

        #region Properties
        public int ID { get; private set; }
        public WorkerStatus Status
        {
            get { return status; }
            set
            {
                if (status != value)
                {
                    status = value;
                    OnPropertyChanged("Status");
                }
            }
        }
        public List<Exception> Errors { get { return errors; } }
        public bool Finished { get { return Status == WorkerStatus.CompletedSuccess || Status == WorkerStatus.CompletedStop || Status == WorkerStatus.CompletedError; } }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        public event WorkerEventHandler WorkDone;
        #endregion

        #region Delegates
        public delegate void WorkerEventHandler(object sender, WorkerEventArgs e);
        #endregion

        #region Constructors
        public Worker()
        {
            ID = ObtainWorkerID();
            Status = WorkerStatus.Initialized;
            errors = new List<Exception>();
        }
        #endregion

        #region Event Creators
        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
        protected virtual void OnWorkDone()
        {
            OnWorkDone(new WorkerEventArgs(Status, Errors));
        }
        protected void OnWorkDone(WorkerEventArgs args)
        {
            if (WorkDone != null) WorkDone(this, args);
        }
        #endregion

        #region Methods
        public virtual void ExecuteTask()
        {
            Status = WorkerStatus.CompletedSuccess;
            OnWorkDone();
        }
        public virtual void Stop()
        {
            Status = WorkerStatus.ShouldStop;
        }
        public abstract String WorkerType();
        private int ObtainWorkerID()
        {
            int id = 1;

            lock(activeIDs)
            {
                while (activeIDs.Contains(id)) id++;
                activeIDs.Add(id);
            }

            return id;
        }
        public void FreeWorkerID()
        {
            lock (activeIDs)
            {
                activeIDs.RemoveAll(i => i == ID);
            }
        }
        public List<Exception> GetExceptions()
        {
            return errors;
        }
        #endregion
    }
}