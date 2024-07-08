using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.ParallelDemo
{
    internal class Parallel_Demo
    {
        public static void Test()
        {
            Parallel_Demo parallel_Demo = new Parallel_Demo();
            parallel_Demo.TestForEach();
        }

        private void TestInvode()
        {
            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.TaskScheduler = TaskScheduler.Default;
            parallelOptions.MaxDegreeOfParallelism = 2;

            Parallel.Invoke(parallelOptions,
                () => { Thread.Sleep(2000); Console.WriteLine($"Parallel 1 ,线程ID：{Thread.CurrentThread.ManagedThreadId}"); },
                () => { Thread.Sleep(2000); Console.WriteLine($"Parallel 2 ,线程ID：{Thread.CurrentThread.ManagedThreadId}"); },
                () => { Thread.Sleep(2000); Console.WriteLine($"Parallel 3 ,线程ID：{Thread.CurrentThread.ManagedThreadId}"); },
                () => { Thread.Sleep(2000); Console.WriteLine($"Parallel 4 ,线程ID：{Thread.CurrentThread.ManagedThreadId}"); });

            //并发执行后才输出，因此可以在此组合输出结果
            Console.WriteLine("并行启动完毕");
        }

        public static void TestFor()
        {
            //计算量大可以使用Paraller并发计算，而计算量小则不建议使用。因为节省的计算时间不如增加的线程开销。
            long end = 5000000000;
            Console.WriteLine($"使用Parallel.For计算0~{end}的平方:");
            Stopwatch stopwatch = Stopwatch.StartNew();
            Parallel.For(1, end, (i) =>
            {
                Math.Sqrt(i);
            });
            stopwatch.Stop();
            Console.WriteLine($"Parallel.For处理速度{stopwatch.ElapsedMilliseconds}毫秒");
            Console.WriteLine();

            Console.WriteLine("使用For输出计算:");
            stopwatch.Restart();
            for (long i = 0; i < end; i++)
            {
                Math.Sqrt(i);
            }
            stopwatch.Stop();
            Console.WriteLine($"For处理速度{stopwatch.ElapsedMilliseconds}毫秒");
            Console.WriteLine();
        }

        private void TestForAsyc()
        {
            Console.WriteLine("使用Parallel.For计算:");
            long end = 5;
            Stopwatch stopwatch = Stopwatch.StartNew();
            Task t1 = Parallel.ForAsync(1, end, async (i, cancellationToken) =>
            {
                await ValueTask.CompletedTask;
                Thread.Sleep(2000);
                Console.WriteLine($"并行任务，线程ID：{Thread.CurrentThread.ManagedThreadId}");
            });
            Thread.Sleep(1000);
            t1.GetAwaiter().GetResult();
            Console.WriteLine("并行完毕");
            stopwatch.Stop();
        }

        private void TestForEach()
        {
            int[] values = { 1, 2, 3 };
            ParallelLoopResult result = Parallel.ForEach(values, (i) => { 
                Thread.Sleep(2000); 
                Console.WriteLine($"Parallel 线程ID：{Thread.CurrentThread.ManagedThreadId}"); 
            });

            //并发执行后才输出，因此可以在此组合输出结果
            Console.WriteLine("并行启动完毕");
        }

        private void TestForQuit()
        {
            Console.WriteLine("并行输出Hello, world直达遇到符号\",\"");
            //不会立即停止，会“尽快”停止。
            Console.WriteLine("测试Break:");
            Parallel.ForEach("Hello, world", (c, loopState) =>
            {
                if (c == ',')
                    loopState.Break();
                else
                    Console.Write(c);
            });
            Console.WriteLine();

            //立即停止
            Console.WriteLine("测试Stop:");
            Parallel.ForEach("Hello, world", (c, loopState) =>
            {
                if (c == ',')
                    loopState.Stop();
                else
                    Console.Write(c);
            });
        }
    }
}
