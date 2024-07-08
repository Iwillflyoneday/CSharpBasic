using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.SynchronizationDemo
{
    /// <summary>
    /// 五个线程争用3个线程
    /// </summary>
    internal class SemaphoreDemo
    {
        private Semaphore _cpuSemaphore;
        private Thread[] _workThreads;
        public SemaphoreDemo()
        {
            _cpuSemaphore = new Semaphore(3, 3);
            _workThreads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                this._workThreads[i] = new Thread(() =>
                {
                    while (true)
                    {
                        this._cpuSemaphore.WaitOne();
                        Console.WriteLine($"I get a CPU. ThreadID: {Thread.CurrentThread.ManagedThreadId}.");
                        Thread.Sleep(1000);
                        this._cpuSemaphore.Release();
                    }
                });
            }
        }

        public static void Test()
        {
            SemaphoreDemo semaphoreDemo = new SemaphoreDemo();
            semaphoreDemo.Start();
        }
        public void Start()
        {
            foreach (var thread in _workThreads)
            {
                thread.Start();
            }
        }
    }
}
