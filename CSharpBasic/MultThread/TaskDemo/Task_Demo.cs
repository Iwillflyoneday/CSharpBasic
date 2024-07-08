using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.TaskDemo
{
    internal class Task_Demo
    {
        private void StartTask()
        {
            Console.WriteLine("1、使用构造函数创建并启动");
            Task t = new Task(() =>
            {
                Console.WriteLine($"我是构造函数启动的Task, 我的ID是{Thread.CurrentThread.ManagedThreadId}");
            });
            t.Start();

            Console.WriteLine("***********************************");
            Console.WriteLine();

            Console.WriteLine("2、使用Factory构造Task，并启动");
            Task t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"我是使用Task.Factory构造Task, 我的ID是{Thread.CurrentThread.ManagedThreadId}");
            });

            Console.WriteLine("***********************************");
            Console.WriteLine();

            /*
             * 1、Task.Run是Task.Facktory.New的轻量级实现，使用了默认参数和TaskCreationOptions.DenyChildAttach，运行于ThreadPool。
             * 2、无参数、不修改任务计划和不长时间运行的线程建议使用Task.Run创建线程。
             */
            Console.WriteLine("2、使用Task.Run构造Task，并启动");
            Task t2 = Task.Run(() =>
            {
                Console.WriteLine($"我是使用Task.Run构造Task, 我的ID是{Thread.CurrentThread.ManagedThreadId}");
            });

            Console.WriteLine("***********************************");
            Console.WriteLine();
        }

        private void TaskWithTaskAwaiter()
        {
            Task t1 = new Task(() =>
            {
                Console.WriteLine("主任务开始！");
                Thread.Sleep(4000);
                Console.WriteLine("主任务结束！");
            });

            TaskAwaiter taskAwaiter=t1.GetAwaiter();
            taskAwaiter.OnCompleted(() =>
            {
                Console.WriteLine(" task OnComplete!");
            });

            t1.Start();
            //t1.Wait();
            taskAwaiter.GetResult();
            Console.WriteLine("任务完成！");
        }

        private void TaskWithRunSynchronously() 
        {
            Console.WriteLine("Application executing on thread {0}",
                        Thread.CurrentThread.ManagedThreadId);
            var asyncTask = Task.Run(() => {
                Console.WriteLine("Task {0} (asyncTask) executing on Thread {1}",
                                                                 Task.CurrentId,
                                                                 Thread.CurrentThread.ManagedThreadId);
                long sum = 0;
                for (int ctr = 1; ctr <= 1000000; ctr++)
                    sum += ctr;
                return sum;
            });
            var syncTask = new Task<long>(() => {
                Console.WriteLine("Task {0} (syncTask) executing on Thread {1}",
                                                                       Task.CurrentId,
                                                                       Thread.CurrentThread.ManagedThreadId);
                long sum = 0;
                for (int ctr = 1; ctr <= 1000000; ctr++)
                    sum += ctr;
                return sum;
            });
            //在当前线程中运行
            syncTask.RunSynchronously();
            Console.WriteLine();
            Console.WriteLine("Task {0} returned {1:N0}", syncTask.Id, syncTask.Result);
            Console.WriteLine("Task {0} returned {1:N0}", asyncTask.Id, asyncTask.Result);
        }

        private void TaskWithCreationOptions(TaskCreationOptions creationOptions)
        {
            Console.WriteLine($"1、使用TaskCreationOptions.{Enum.GetName(creationOptions)}创建并启动");
            CancellationTokenSource cts1 = new CancellationTokenSource();
            CancellationTokenSource cts2 = new CancellationTokenSource();

            CancellationToken token1 = cts1.Token;
            CancellationToken token2 = cts2.Token;
            token1.Register(() =>
            {
                Console.WriteLine("Token1取消了");
            });
            token2.Register(() =>
            {
                Console.WriteLine("Token2取消了");
            });
            cts1.CancelAfter(4000);
            cts2.CancelAfter(8000);

            Action action1 = () =>
            {
                Task.Factory.StartNew(() =>
                {
                    while (!token2.IsCancellationRequested)
                    {
                        Console.WriteLine($"我是使用TaskCreationOptions.AttachedToParent构造的ChildTask, 我的ID是{Thread.CurrentThread.ManagedThreadId}");
                        Thread.Sleep(2000);
                    }
                }, TaskCreationOptions.AttachedToParent);
                while (!token1.IsCancellationRequested)
                {
                    Console.WriteLine($"我是使用TaskCreationOptions.{Enum.GetName(creationOptions)}构造的ParentTask, 我的ID是{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(2000);
                }
            };

            Task t1 = new Task(action1, token1, creationOptions);
            t1.ContinueWith((t) =>
            {
                Console.WriteLine("T3线程开始");
            });

            t1.Start();
            t1.Wait();//等所有子线程完成
            Console.WriteLine("执行完成！");
        }
        private void UsingTaskWithParmerter()
        {
            Console.WriteLine("1、使用无参数Action构造函数创建并启动");
            Task t1 = new Task(() =>
            {
                Console.WriteLine($"我是使用无参数Action构造函数创建并启动, 我的ID是{Thread.CurrentThread.ManagedThreadId}");
            });
            t1.Start();

            Console.WriteLine("2、使用带参数Action构造函数创建并启动");
            Task t2 = new Task((para) =>
            {
                Console.WriteLine($"我是使用带参数Action构造函数创建并启动, 我的ID是:{Thread.CurrentThread.ManagedThreadId}, 参数是:{para}");
            }, "Hello world");
            t2.Start();

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationToken.Register(() =>
            {
                Console.WriteLine("t3 Task 取消了,来自回调!");
            });

            Console.WriteLine("3、使用带参数Action构造函数创建并启动");
            Task t3 = new Task((para) =>
            {
                Console.WriteLine($"我是使用带参数Action构造函数创建并启动, 我的ID是:{Thread.CurrentThread.ManagedThreadId}, 参数是:{para}");
                Thread.Sleep(1000);
                if (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine("t3 Task 正常结束了!");
                }
                else
                {
                    Console.WriteLine("t3 Task 取消了!");
                }
            }, "Hello world", cancellationToken);
            t3.Start();
            cancellationTokenSource.Cancel();
        }

        public static void Test()
        {
            Console.WriteLine($"我是主线程，我的ID是{Thread.CurrentThread.ManagedThreadId}");
            Console.WriteLine();
            Task_Demo task_Demo = new Task_Demo();
            //task_Demo.TaskWithCreationOptions(TaskCreationOptions.AttachedToParent);
            //task_Demo.TaskWithCreationOptions(TaskCreationOptions.DenyChildAttach);
            //Thread.Sleep(6000);
            //task_Demo.TaskWithCreationOptions(TaskCreationOptions.PreferFairness);
            task_Demo.TaskWithRunSynchronously();

            //task_Demo.TaskWithTaskAwaiter();
        }
    }
}
