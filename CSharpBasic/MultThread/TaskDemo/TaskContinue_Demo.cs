using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.TaskDemo
{
    internal class TaskContinue_Demo
    {
        private void ContinueWhenAll()
        {
            Task t_Main = new Task(() =>
            {
                Console.WriteLine("我是主线程！");
                Thread.Sleep(1000);
            });
            Task t_Continue1 = t_Main.ContinueWith((t) =>
            {
                Console.WriteLine("我是延续线程1");
                Thread.Sleep(1000);
            });
            Task t_Continue2 = t_Main.ContinueWith((t) =>
            {
                Console.WriteLine("我是延续线程2");
                Thread.Sleep(1000);
            });
            Task t_Continue3 = Task.Factory.ContinueWhenAll(new Task[] { t_Continue1, t_Continue2 }, (tasks) =>
            {
                Console.WriteLine("我是延续线程3");
            });

            t_Main.Start(); 
        }

        private void ContinueWhenAny()
        {
            Task t_Main = new Task(() =>
            {
                Console.WriteLine("我是主线程！");
                Thread.Sleep(1000);
            });
            Task t_Continue1 = t_Main.ContinueWith((t) =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("我是延续线程1");
            });
            Task t_Continue2 = t_Main.ContinueWith((t) =>
            {
                Thread.Sleep(3000);
                Console.WriteLine("我是延续线程2");
            });
            Task t_Continue3 = Task.Factory.ContinueWhenAny(new Task[] { t_Continue1, t_Continue2 }, (tasks) =>
            {
                Console.WriteLine("我是延续线程3");
            });

            t_Main.Start();
        }

        private void TaskContinueWithException()
        {
            Task task1 = Task.Factory.StartNew(() => { 
                //throw null; 
            });

            Task error = task1.ContinueWith(ant => Console.Write(ant.Exception),
                                             TaskContinuationOptions.OnlyOnFaulted);

            Task ok = task1.ContinueWith(ant => Console.Write("Success!"),
                                          TaskContinuationOptions.NotOnFaulted);
        }


        public static void Test()
        {
            TaskContinue_Demo continue_Demo= new TaskContinue_Demo();
            //continue_Demo.ContinueWhenAll();
            //continue_Demo.ContinueWhenAny();
            continue_Demo.TaskContinueWithException();
        }
    }
}
