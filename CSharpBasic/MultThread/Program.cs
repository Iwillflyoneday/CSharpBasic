// See https://aka.ms/new-console-template for more information
using MultThread;
using MultThread.ParallelDemo;
using MultThread.SynchronizationDemo;
using MultThread.TaskDemo;

Console.WriteLine($"主线程 ,线程ID：{Thread.CurrentThread.ManagedThreadId}");
//Console.WriteLine("Hello, World!");
//FooBarWithAutoResetEvent.Test(5);
//FooBarWithManualResetEvent.Test(15);
//FooBarWithManualResetEventSlim.Test(5);
//MutexDemo.Test(3);
//SemaphoreDemo.Test();
//SemaphoreSlimDemo.Test();
//WaitHandleDemo.Test();
//MonitorDemo.Test();
//Task_Demo.Test();
//TaskContinue_Demo.Test();
//TaskAwaiter_Demo.Test();
Parallel_Demo.Test();
Console.ReadKey();