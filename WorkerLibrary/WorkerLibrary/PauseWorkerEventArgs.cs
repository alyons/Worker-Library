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
