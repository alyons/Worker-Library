using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkerLibrary
{
    public class WorkerEventArgs : EventArgs
    {
        #region Properties
        public List<Exception> Exceptions { get; private set; }
        public WorkerStatus Status;
        #endregion

        #region Constructors
        public WorkerEventArgs()
            : base()
        {
            Exceptions = new List<Exception>();
            Status = WorkerStatus.Undefined;
        }
        public WorkerEventArgs(WorkerStatus status)
            : this ()
        {
            Status = status;
        }
        public WorkerEventArgs(WorkerStatus status, Exception e)
            : this(status)
        {
            Exceptions.Add(e);
        }
        public WorkerEventArgs(WorkerStatus status, List<Exception> e)
         : this(status)
        {
            Exceptions.AddRange(e);
        }
        #endregion
    }
}
