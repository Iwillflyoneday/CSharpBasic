//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System;
//using System.Threading;
//using System.Runtime.Remoting.Contexts;

//namespace MultThread.SynchronizationDemo
//{
//    internal class 同步上下文
//    {
//        public static void Test()
//        {
//            AutoLock safeInstance = new AutoLock();
//            new Thread(safeInstance.Demo).Start();
//            new Thread(safeInstance.Demo).Start();
//            safeInstance.Demo();                        //并发调用 Demo 3 次
//        }
//    }

//    [Synchronization]
//    public class AutoLock : ContextBoundObject
//    {
//        public void Demo()
//        {
//            Console.Write("Start...");
//            Thread.Sleep(1000);           // 这里我们无法抢占
//            Console.WriteLine("end");     // 感谢自动锁
//        }
//    }

//    public class Test
//    {
        
//    }
//}
