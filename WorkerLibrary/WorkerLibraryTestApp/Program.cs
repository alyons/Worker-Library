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
using WorkerLibrary;
using System.Threading;

namespace WorkerLibraryTestApp
{
    class Program
    {
        static List<Worker> workers = new List<Worker>();
        static List<Thread> workerThreads = new List<Thread>();
        static Random random = new Random();
        static bool hasPaused = false;
        static int workerNum = 5;
        static int spinNum = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("WorkerLibrary  Copyright (C) 2013  Alexander Lyons\nTHERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY APPLICABLE LAW. EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT HOLDERS AND/OR OTHER PARTIES PROVIDE THE PROGRAM “AS IS” WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM IS WITH YOU. SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF ALL NECESSARY SERVICING, REPAIR OR CORRECTION.\nThis is free software, and you are welcome to redistribute it under certain conditions; please refer to the GPL included with this project.");
            Console.Write("Press enter to continue...");

            Console.Read();
            Console.Clear();

            Console.WriteLine("Testing Pausable Workers");

            try
            {
                Console.WriteLine("Starting Test Workers");
                for (int i = 0; i < workerNum; i++)
                {
                    workers.Add(new TestWorker(random.Next(int.MaxValue)));
                    (workers.Last() as Worker).PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(Program_PropertyChanged);
                    (workers.Last() as Worker).WorkDone += new Worker.WorkerEventHandler(Program_WorkDone);
                    (workers.Last() as TestWorker).Paused += new PausableWorker.PauseWorkerEventHandler(Program_Paused);
                    (workers.Last() as TestWorker).Resumed += new PausableWorker.ResumeWorkerEventHandler(Program_Resumed);
                    workerThreads.Add(new Thread(new ThreadStart(workers.Last().ExecuteTask)));
                    workerThreads.Last().Start();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        static void Program_WorkDone(object sender, WorkerEventArgs e)
        {
            Console.WriteLine((sender as Worker).WorkerType() + " - Found " + (sender as TestWorker).Maximum + "! " + (sender as TestWorker).Count);
            if(workers.All(w => w.Finished))
            {
                Console.Write("Done!");
                Console.Read();
            }
        }

        static void PauseWorkers()
        {
            Console.WriteLine("Waiting for task to finish...");
            Thread.Sleep(5000);
            Console.WriteLine("Pausing Test Workers");
            lock (workers)
            {
                foreach (PausableWorker worker in workers.FindAll(w => w is PausableWorker))
                {
                    worker.Pause();
                } 
            }
            hasPaused = true;
        }

        static void ResumeWorkers()
        {
            Console.WriteLine("Workers all paused! waiting...");
            Thread.Sleep(5000);
            Console.WriteLine("Resuming all workers");
            lock (workers)
            {
                foreach (PausableWorker worker in workers.FindAll(w => w is PausableWorker))
                {
                    worker.Resume();
                } 
            }
        }

        static void Program_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            lock (workers)
            {
                switch (e.PropertyName)
                {
                    case "Status":
                        Console.WriteLine((sender as Worker).WorkerType() + " changed property " + e.PropertyName + ": "  + (sender as Worker).Status);
                        //if (workers.All(w => w.Status == WorkerStatus.Working) && !hasPaused && workers.Count == workerNum) PauseWorkers();
                        break;
                    default:
                        Console.WriteLine((sender as Worker).WorkerType() + " changed property " + e.PropertyName + ".");
                        break;
                } 
            }
        }

        static void Program_Resumed(object sender, ResumeWorkerEventArgs e)
        {
            Console.WriteLine(String.Format("Worker {0} resumed at time {1}.", (sender as Worker).ID, e.Time));
        }

        static void Program_Paused(object sender, PauseWorkerEventArgs e)
        {
            Console.WriteLine(String.Format("{0} paused at time {1}.", (sender as Worker).WorkerType(), e.Time));
            (sender as TestWorker).Resume();
        }
    }

    class TestWorker : PausableWorker
    {
        int count;
        int maximum;

        public int Count { get { return count; } }
        public int Maximum { get { return maximum; } }

        public TestWorker(int max) : base() 
        {
            maximum = max;
        }

        public override string WorkerType()
        {
            return String.Format("TestWorker[{0}]", ID);
        }

        public override void ExecuteTask()
        {
            Console.WriteLine(WorkerType() + " - Looking for " + maximum + "...");

            Status = WorkerStatus.Working;

            count = 0;

            while (count < maximum)
            {
                while (Status == WorkerStatus.Paused) ;
                count++;
            }

            base.ExecuteTask();
        }

        public override void Pause()
        {
            if (!Finished)
            {
                Status = WorkerStatus.Paused;
                OnPaused();
                Console.WriteLine("Paused at: " + count);
            }
        }

        public override void Resume()
        {
            if (!Finished)
            {
                Console.WriteLine("Resuming at: " + count);
                OnResume();
                Status = WorkerStatus.Working; 
            }
        }

    }
}
