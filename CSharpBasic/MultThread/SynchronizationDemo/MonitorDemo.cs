using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.SynchronizationDemo
{
    /// <summary>
    /// 更新一个数值，通知其他需要使用的线程
    /// </summary>
    internal class MonitorDemo
    {
        private Thread _updateThread1;

        private Thread _updateThread2;

        private Thread _showThread;

        private Thread _storeThread;

        private int _value = 0;

        private object _lockObject = new object();

        public static void Test()
        {
            MonitorDemo monitorDemo = new MonitorDemo();
            monitorDemo.Start();
        }

        public void Start()
        {
            this.Stop();
            this._updateThread1 = new Thread(UpdateValue);
            this._updateThread2 = new Thread(UpdateValue);
            this._showThread = new Thread(ShowValue);
            this._storeThread = new Thread(StoreValue);
            this._showThread.Start();
            this._storeThread.Start();
            this._updateThread1.Start();
            this._updateThread2.Start();

        }

        private void UpdateValue()
        {
            int count = 30;
            while (count >= 0)
            {
                Monitor.Enter(this._lockObject);
                this._value++;
                Console.WriteLine($"Value Changed to {this._value} , ThreadID: {Thread.CurrentThread.ManagedThreadId}");
                Monitor.PulseAll(this._lockObject);
                Monitor.Exit(this._lockObject);
                Thread.Sleep(1000);
                count--;
            }
        }

        private void StoreValue(object? obj)
        {
            while (true)
            {
                Monitor.Enter(this._lockObject);
                Console.WriteLine($"Store thread Enter");
                Monitor.Wait(this._lockObject);
                Console.WriteLine($"Storing value {this._value}");
                Monitor.Exit(this._lockObject);
                Console.WriteLine($"Store thread Exit");
            }
        }

        private void ShowValue()
        {
            while (true)
            {
                Monitor.Enter(this._lockObject);
                Console.WriteLine($"Show thread Enter");
                //注意，Wait函数会释放锁，然后等待pulse之后重新排队去获得锁
                Monitor.Wait(this._lockObject);
                Console.WriteLine($"Showing value {this._value}");
                Monitor.Exit(this._lockObject);
                Console.WriteLine($"Show thread Exit");
            }
        }

        private void Stop()
        {
            this._updateThread1?.Join();
            this._updateThread2?.Join();
            this._showThread?.Join();
            this._storeThread?.Join();
        }
    }
}
