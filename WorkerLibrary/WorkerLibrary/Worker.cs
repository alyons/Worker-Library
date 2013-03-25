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
        protected void OnWorkDone()
        {
            if (WorkDone != null) WorkDone(this, new WorkerEventArgs(Status, Errors));
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
        protected void FreeWorkerID()
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