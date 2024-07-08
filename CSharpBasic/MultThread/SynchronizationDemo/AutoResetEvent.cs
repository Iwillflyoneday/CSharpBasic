using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultThread.SynchronizationDemo
{
    /*
     1115. 交替打印 FooBar

     给你一个类：

class FooBar {
  public void foo() {
    for (int i = 0; i < n; i++) {
      print("foo");
    }
  }

  public void bar() {
    for (int i = 0; i < n; i++) {
      print("bar");
    }
  }
}
两个不同的线程将会共用一个 FooBar 实例：

线程 A 将会调用 foo() 方法，而
线程 B 将会调用 bar() 方法
请设计修改程序，以确保 "foobar" 被输出 n 次。

 

示例 1：

输入：n = 1
输出："foobar"
解释：这里有两个线程被异步启动。其中一个调用 foo() 方法, 另一个调用 bar() 方法，"foobar" 将被输出一次。
示例 2：

输入：n = 2
输出："foobarfoobar"
解释："foobar" 将被输出两次。
 

提示：

1 <= n <= 1000
    */
    public class AutoResetEvent
    {
        private int n;

        public AutoResetEvent(int n)
        {
            this.n = n;
        }

        private System.Threading.AutoResetEvent _fooEvent = new System.Threading.AutoResetEvent(true);

        private System.Threading.AutoResetEvent _barEvent = new System.Threading.AutoResetEvent(false);

        public static void Test(int count = 3)
        {
            Console.WriteLine("进入Foobar");
            AutoResetEvent fooBar = new AutoResetEvent(count);
            Thread threadA = new Thread(() =>
            {
                fooBar.Foo(() => Console.Write("Foo"));
            });
            Thread threadB = new Thread(() =>
            {
                fooBar.Bar(() => Console.WriteLine("Bar"));
            });
            threadA.Start();
            threadB.Start();
        }

        public void Foo(Action printFoo)
        {

            for (int i = 0; i < n; i++)
            {
                this._fooEvent.WaitOne();
                // printFoo() outputs "foo". Do not change or remove this line.
                printFoo();
                this._barEvent.Set();
            }
        }

        public void Bar(Action printBar)
        {
            for (int i = 0; i < n; i++)
            {
                this._barEvent.WaitOne();
                // printBar() outputs "bar". Do not change or remove this line.
                printBar();
                this._fooEvent.Set();
            }
        }
    }
}
