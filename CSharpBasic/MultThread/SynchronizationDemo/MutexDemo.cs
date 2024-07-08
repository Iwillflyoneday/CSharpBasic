using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.SynchronizationDemo
{
    /// <summary>
    /// Mutex互斥量的Demo，两个线程争用一个CPU
    /// </summary>
    internal class MutexDemo
    {
        private Thread _ioThread;
        private Thread _cpuThread;

        private Mutex _mutex;
        private int _count;

        public MutexDemo(int count)
        {
            this._count = count;
            this._mutex = new Mutex(false);
            this._ioThread = new Thread(this.IOWork);
            this._cpuThread = new Thread(this.CPUWork);
        }

        public static void Test(int count = 5)
        {
            MutexDemo demo = new MutexDemo(count);
            demo.Start();
        }

        public void Start()
        {

            this._ioThread.Start();
            this._cpuThread.Start();
        }

        private void IOWork()
        {
            int index = this._count;
            while (index > 0)
            {
                this._mutex.WaitOne();
                Console.WriteLine("*****I am doing IOWork!");
                //Thread.Sleep(100);
                //注意Mutex为了保证线程安全，只能在拥有Mutex的线程中Release。否则会报不在同步线程的错误。
                this._mutex.ReleaseMutex();
                index--;
            }
        }

        private void CPUWork()
        {
            int index = this._count;
            while (index > 0)
            {
                this._mutex.WaitOne();
                Console.WriteLine("#####I am doing CPUWork!");
                //Thread.Sleep(100);
                this._mutex.ReleaseMutex();
                index--;
            }
        }
    }
}
