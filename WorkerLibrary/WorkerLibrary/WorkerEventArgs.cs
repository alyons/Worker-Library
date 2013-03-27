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
