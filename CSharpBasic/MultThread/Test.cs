using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread
{
    internal class Test
    {
        static readonly object _locker = new object();
        static bool _go;

        public static void Main()
        {                                // 新线程会阻塞
            new Thread(Work).Start();     // 因为 _go==false

            Console.ReadLine();            // 等待用户敲回车

            lock (_locker)                 // 现在唤醒线程
            {                              // 通过设置 _go=true 然后 Pulse
                _go = true;
                Monitor.Pulse(_locker);
            }
        }

        static void Work()
        {
            lock (_locker)
                while (!_go)
                    Monitor.Wait(_locker);    // 当等待时锁会被释放

            Console.WriteLine("Woken!!!");
        }
    }
}
