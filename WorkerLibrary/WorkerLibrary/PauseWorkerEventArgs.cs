using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerLibrary
{
    public class PauseWorkerEventArgs : WorkerEventArgs
    {
        #region Properties
        public DateTime Time { get; private set; }
        #endregion

        #region Constuctors
        public PauseWorkerEventArgs()
            : base()
        {
            Time = DateTime.UtcNow;
        }
        public PauseWorkerEventArgs(WorkerStatus status)
            : this()
        {
            Status = status;
        }
        public PauseWorkerEventArgs(WorkerStatus status, Exception e)
            : this(status)
        {
            Exceptions.Add(e);
        }
        public PauseWorkerEventArgs(WorkerStatus status, List<Exception> e)
            : this(status)
        {
            Exceptions.AddRange(e);
        }
        #endregion
    }

    public class ResumeWorkerEventArgs : WorkerEventArgs
    {
        #region Properties
        public DateTime Time { get; private set; }
        #endregion

        #region Constuctors
        public ResumeWorkerEventArgs()
            : base()
        {
            Time = DateTime.UtcNow;
        }
        public ResumeWorkerEventArgs(WorkerStatus status)
            : this()
        {
            Status = status;
        }
        public ResumeWorkerEventArgs(WorkerStatus status, Exception e)
            : this(status)
        {
            Exceptions.Add(e);
        }
        public ResumeWorkerEventArgs(WorkerStatus status, List<Exception> e)
            : this(status)
        {
            Exceptions.AddRange(e);
        }
        #endregion
    }
}
