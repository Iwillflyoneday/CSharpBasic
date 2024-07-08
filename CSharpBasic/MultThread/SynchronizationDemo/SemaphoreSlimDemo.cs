using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.SynchronizationDemo
{
    /*c# semaphoreslim和semaphore区别
SemaphoreSlim 和 Semaphore 都是在C#中用于控制多个线程同时访问某个资源的同步构造。SemaphoreSlim 是 Semaphore 的轻量级版本，设计上更适合现代多核处理器架构。

主要区别如下：

性能：Semaphore 是传统的内核对象，在用户模式和内核模式之间进行转换，而 SemaphoreSlim 是基于用户模式的，因此性能更好，更适合于高并发的场景。

兼容性：SemaphoreSlim 是在.NET Framework 4.0中引入的，而Semaphore 在早期版本就存在。

功能：SemaphoreSlim 不支持命名的系统资源，因此它只能用于同一个应用程序的不同线程间同步。而Semaphore 可以用于同步不同应用程序域或进程的线程。

    Semaphore表示一个命名（系统范围内）或本地信号量。它是对 Win32 信号量对象的封装，Win32 信号量是计数信号量，其可用于控制对资源池的访问,可以跨进程的
 SemaphoreSlim 类为一个轻量、快速的信号量，可在等待时间预计很短的情况下，用于在单个进程内的等待

    本质区别是 Semaphore 是基于系统提供的同步原语实现的同步等待，而 SemaphoreSlim 是通过自旋（基于 SpinWait） 实现的同步等待。

Slim 版本在大多数情况下不会出现用户态到系统态的转换，而 Semaphore 则非常可能发生这种情况。但由于 SpinWait 的特点，Slim 版本更加适用于等待时间较短的场景。因为如果出现了长时间的等待（长过一次工作状态的切换），SpinWait 会放弃继续空循环的操作，将执行机会让给其他的线程，这样又会导致上下文的切换

使用示例代码：https://learn.microsoft.com/zh-cn/dotnet/standard/threading/semaphore-and-semaphoreslim
     */
    internal class SemaphoreSlimDemo
    {
        private SemaphoreSlim _cpuSemaphoreSlim;
        private Thread[] _workThreads;
        public SemaphoreSlimDemo()
        {
            _cpuSemaphoreSlim = new SemaphoreSlim(3, 3);
            _workThreads = new Thread[5];
            for (int i = 0; i < 5; i++)
            {
                this._workThreads[i] = new Thread(() =>
                {
                    while (true)
                    {
                        this._cpuSemaphoreSlim.Wait();
                        Console.WriteLine($"I get a CPU. ThreadID: {Thread.CurrentThread.ManagedThreadId}.");
                        Thread.Sleep(1000);
                        this._cpuSemaphoreSlim.Release();
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
