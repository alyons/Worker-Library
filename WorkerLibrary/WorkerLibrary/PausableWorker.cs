using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerLibrary
{
    public abstract class PausableWorker : Worker
    {
        #region Events
        public event PauseWorkerEventHandler Paused;
        public event ResumeWorkerEventHandler Resumed;
        #endregion

        #region Delegates
        public delegate void PauseWorkerEventHandler(object sender, PauseWorkerEventArgs e);
        public delegate void ResumeWorkerEventHandler(object sender, ResumeWorkerEventArgs e);
        #endregion

        #region Constructor
        public PausableWorker() : base() { }
        #endregion

        #region Methods
        protected void OnPaused()
        {
            if (Paused != null) Paused(this, new PauseWorkerEventArgs(Status, Errors));
        }
        protected void OnResume()
        {
            if (Resumed != null) Resumed(this, new ResumeWorkerEventArgs(Status, Errors));
        }
        public virtual void Pause()
        {
            if (Status == WorkerStatus.Working)
                Status = WorkerStatus.ShouldPause;
        }
        public virtual void Resume()
        {
            if (Status == WorkerStatus.Paused)
                Status = WorkerStatus.ShouldResume;
        }
        #endregion
    }
}
