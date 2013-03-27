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
