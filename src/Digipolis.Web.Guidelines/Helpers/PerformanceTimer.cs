using System;
using System.Diagnostics;

namespace Digipolis.Web.Guidelines.Helpers
{
    public class PerformanceTimer : IDisposable
    {
        private readonly Stopwatch _stopwatch;

        public PerformanceTimer()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        public void Dispose()
        {
            _stopwatch.Stop();
            Debug.WriteLine(_stopwatch.Elapsed);
        }
    }
}
