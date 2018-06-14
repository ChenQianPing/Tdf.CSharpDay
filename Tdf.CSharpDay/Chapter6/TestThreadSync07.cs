using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync07
    {
        public static void TestMethod1()
        {
            MRES_SetWaitReset();
        }

        static void MRES_SetWaitReset()
        {
            var mres1 = new ManualResetEventSlim(false);

            var observer = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("阻塞当前线程,使 mres1 处于等待状态...!");
                mres1.Wait();
                while (true)
                {
                    if (mres1.IsSet)
                    {
                        /*等待 mres1 Set()信号 当有信号时 在执行后面代码*/
                        Console.WriteLine("得到mres1信号,执行后续代码....!");
                    }
                    Thread.Sleep(100);
                }

            });

            Thread.Sleep(2000);

            Console.WriteLine("取消mres1等待状态");
            mres1.Set();
            Console.WriteLine("当前信号状态：{0}", mres1.IsSet);
            Thread.Sleep(300);

            mres1.Reset();
            Console.WriteLine("当前信号状态：{0}", mres1.IsSet);
            Thread.Sleep(300);

            mres1.Set();
            Console.WriteLine("当前信号状态：{0}", mres1.IsSet);
            Thread.Sleep(300);

            mres1.Reset();
            Console.WriteLine("当前信号状态：{0}", mres1.IsSet);

            observer.Wait();
            mres1.Dispose();
            Console.ReadLine();
        }
    }
}


/*
 * ManualResetEventSlim

ManualResetEventSlim通过封装手动重置事件等待句柄提供了自旋等待和内核等待的组合。
您可以使用这个类的实例在任务直接发送信息，并等待事件的发送。通过信号机制通知任务开始其工作。

其Set 方法将事件状态设置为有信号，从而允许一个或多个等待该事件的线程继续。 
其 Wait()方法 阻止当前线程，直到设置了当前 ManualResetEventSlim 为止。

如果需要跨进程或者跨AppDomain的同步，那么就必须使用ManualResetEvent，
而不能使用ManualResetEventSlim。
 */
