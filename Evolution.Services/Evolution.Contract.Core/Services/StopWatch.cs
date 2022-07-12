
using System;
using System.Diagnostics;

namespace Evolution.Contract.Core.Services
{

    public static class StopWatch
    {
        public static Stopwatch Start()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            return stopWatch;
        }

        public static void Stop(Stopwatch stopWatch)
        {
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds,ts.Milliseconds / 10);
            Console.WriteLine("Process Time taken for saving complete Contract without Get Operation " + elapsedTime);
        }
    }
}
