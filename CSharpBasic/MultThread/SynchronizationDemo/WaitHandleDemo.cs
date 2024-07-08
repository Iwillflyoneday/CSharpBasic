using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.SynchronizationDemo
{
    /// <summary>
    /// WaitHandle学习Demo
    /// </summary>
    internal class WaitHandleDemo
    {
        public static void Test()
        {
            CarFactory carFactory = new CarFactory();
            carFactory.Start();
        }
    }

    /// <summary>
    /// 小明参加
    /// </summary>
    internal class GetAJob
    {

    }



    /// <summary>
    /// 组装辆车的工厂,有负责制造车轮、底盘、车架、引擎、组装、测试的车间.
    /// 组装车需要在所有配件到齐后开始组装，组装完毕后立即测试。
    /// 模拟车间收到组装一辆汽车订单开始制造到检测的过程
    /// </summary>
    internal class CarFactory
    {
        private Thread _wheelThread;
        private Thread _chassisThread;
        private Thread _carBodyThread;
        private Thread _engineThread;
        private Thread _makeCarThread;
        private Thread _testCarThread;
        private System.Threading.ManualResetEvent _wheelEvent;
        private System.Threading.ManualResetEvent _chassisEvent;
        private System.Threading.ManualResetEvent _carBodyEvent;
        private System.Threading.ManualResetEvent _engineEvent;
        private System.Threading.ManualResetEvent _makeCarEvent;

        public CarFactory()
        {
            _wheelEvent = new System.Threading.ManualResetEvent(false);
            _chassisEvent = new System.Threading.ManualResetEvent(false);
            _carBodyEvent = new System.Threading.ManualResetEvent(false);
            _engineEvent = new System.Threading.ManualResetEvent(false);
            _makeCarEvent = new System.Threading.ManualResetEvent(false);

            this._wheelThread = new Thread(() =>
            {
                Console.WriteLine("Make wheel.");
                Thread.Sleep(1000);
                Console.WriteLine("Finish wheel.");
                _wheelEvent.Set();
            });
            this._chassisThread = new Thread(() =>
            {
                Console.WriteLine("Make chassis.");
                Thread.Sleep(1000);
                Console.WriteLine("Finish chassis.");
                _chassisEvent.Set();
            });
            this._carBodyThread = new Thread(() =>
            {
                Console.WriteLine("Make carBody.");
                Thread.Sleep(1000);
                Console.WriteLine("Finish carBody.");
                _carBodyEvent.Set();
            });
            this._engineThread = new Thread(() =>
            {
                Console.WriteLine("Make engine.");
                Thread.Sleep(1000);
                Console.WriteLine("Finish engine.");
                _engineEvent.Set();
            });
            this._makeCarThread = new Thread(() =>
            {
                WaitHandle.WaitAll(new[] { _wheelEvent, _chassisEvent, _carBodyEvent, _engineEvent});
                Console.WriteLine("Make a car.");
                Thread.Sleep(1000);
                Console.WriteLine("finish a Car.");
                _makeCarEvent.Set();
            });
            this._testCarThread = new Thread(() =>
            {
                _makeCarEvent.WaitOne();
                Console.WriteLine("Test car.");
                Thread.Sleep(1000);
                Console.WriteLine("Car is good!.");
            });
        }
        public void Start()
        {
            Console.WriteLine(" Start making a car");
            _wheelThread.Start();
            _chassisThread.Start();
            _engineThread.Start();
            _carBodyThread.Start();
            _makeCarThread.Start();
            _testCarThread.Start();
        }
    }
}
