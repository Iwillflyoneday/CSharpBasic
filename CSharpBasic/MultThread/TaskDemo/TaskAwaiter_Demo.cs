using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.CompilerServices.ConfiguredTaskAwaitable;

namespace MultThread.TaskDemo
{
    internal class TaskAwaiter_Demo
    {

        private void TaskAwaiter_Test()
        {
            Task t1 = new Task(() =>
            {
                Console.WriteLine("主任务开始！");
                Thread.Sleep(4000);
                Console.WriteLine("主任务结束！");
            });

            TaskAwaiter taskAwaiter = t1.GetAwaiter();
            taskAwaiter.OnCompleted(() =>
            {
                Console.WriteLine(" task OnComplete!");
            });

            taskAwaiter.UnsafeOnCompleted(() =>
            {
                Console.WriteLine(" UnsafeOnCompleted OnComplete!");
            });


            t1.Start();
            //在此等待
            taskAwaiter.GetResult();
            Console.WriteLine("任务完成！");
        }

        private void ConfiguredTaskAwaiter_Test()
        {
            Task t1 = new Task(() =>
            {
                Console.WriteLine($"主任务开始！线程ID：{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(4000);
                Console.WriteLine($"主任务结束！线程ID：{Thread.CurrentThread.ManagedThreadId}");
            });

            ConfiguredTaskAwaitable taskAwaitable = t1.ConfigureAwait(true);
            ConfiguredTaskAwaiter taskAwaiter=taskAwaitable.GetAwaiter();
            /*.ConfigureAwait(false) 方法可以实现异步，前驱方法完成后，可以不理会后续任务，而且后续任务可以在任意一个线程上运行。这个特性在 UI 界面程序上特别有用。*/
            taskAwaiter.OnCompleted(() =>
            {
                Console.WriteLine($"ConfigureAwait(false)  task OnComplete! 线程ID：{Thread.CurrentThread.ManagedThreadId}");
            });

            taskAwaiter.UnsafeOnCompleted(() =>
            {
                Console.WriteLine($"ConfigureAwait(false)  UnsafeOnCompleted OnComplete! 线程ID：{Thread.CurrentThread.ManagedThreadId}");
            });


            t1.Start();
            //在此等待
            taskAwaiter.GetResult();
            Console.WriteLine("任务完成！");
        }

        public static void Test()
        {
            TaskAwaiter_Demo awaiter_Demo= new TaskAwaiter_Demo();
            awaiter_Demo.ConfiguredTaskAwaiter_Test();
        }


    }
}
