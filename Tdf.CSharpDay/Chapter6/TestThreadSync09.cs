using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync09
    {
        public static void TestMethod1()
        {
            CountdownEvent cde = new CountdownEvent(3); // 创建SemaphoreSlim 初始化信号量最多计数为3次 
            Console.WriteLine(" InitialCount={0}, CurrentCount={1}, IsSet={2}", cde.InitialCount, cde.CurrentCount, cde.IsSet);

            // Launch an asynchronous Task that releases the semaphore after 100 ms
            Task t1 = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000);
                    if (!cde.IsSet)
                    {
                        cde.Signal();
                        Console.WriteLine(" InitialCount={0}, CurrentCount={1}, IsSet={2}", cde.InitialCount, cde.CurrentCount, cde.IsSet);
                    }
                }
            });
            cde.Wait();
            /*将 CurrentCount 重置为 InitialCount 的值。*/
            Console.WriteLine("将 CurrentCount 重置为 InitialCount 的值。");
            cde.Reset();

            cde.Wait();
            /*将 CurrentCount 重置为 5*/
            Console.WriteLine("将 CurrentCount 重置为 5");
            cde.Reset(5);
            cde.AddCount(2);

            cde.Wait();
            /*等待任务完成*/
            Task.WaitAll(t1);
            Console.WriteLine("任务执行完成");
            /*释放*/
            cde.Dispose();
            Console.ReadLine();
        }

        public static void TestMethod2()
        {
            CountdownEvent cde = new CountdownEvent(3); // 创建SemaphoreSlim 初始化信号量最多计数为3次 
            Console.WriteLine(" InitialCount={0}, CurrentCount={1}, IsSet={2}", cde.InitialCount, cde.CurrentCount, cde.IsSet);
            /*创建任务执行计数*/
            Task t1 = Task.Factory.StartNew(() =>
            {
                for (var index = 0; index <= 5; index++)
                {
                    /*重置计数器*/
                    cde.Reset();
                    /*创建任务执行计数*/
                    while (true)
                    {
                        Thread.Sleep(1000);
                        if (!cde.IsSet)
                        {
                            cde.Signal();
                            Console.WriteLine("第{0}轮计数  CurrentCount={1}", index, cde.CurrentCount);
                        }
                        else
                        {
                            Console.WriteLine("第{0}轮计数完成", index);
                            break;
                        }
                    }
                    /*等待计数完成*/
                    cde.Wait();
                }
            });
            t1.Wait();
            /*释放*/
            cde.Dispose();
            Console.ReadLine();
        }

    }
}


/*
 *  InitialCount=3, CurrentCount=3, IsSet=False
 InitialCount=3, CurrentCount=2, IsSet=False
 InitialCount=3, CurrentCount=1, IsSet=False
 InitialCount=3, CurrentCount=0, IsSet=True
将 CurrentCount 重置为 InitialCount 的值。
 InitialCount=3, CurrentCount=2, IsSet=False
 InitialCount=3, CurrentCount=1, IsSet=False
 InitialCount=3, CurrentCount=0, IsSet=True
将 CurrentCount 重置为 5
 InitialCount=5, CurrentCount=6, IsSet=False
 InitialCount=5, CurrentCount=5, IsSet=False
 InitialCount=5, CurrentCount=4, IsSet=False
 InitialCount=5, CurrentCount=3, IsSet=False
 InitialCount=5, CurrentCount=2, IsSet=False
 InitialCount=5, CurrentCount=1, IsSet=False
 InitialCount=5, CurrentCount=0, IsSet=True




 InitialCount=3, CurrentCo
第0轮计数  CurrentCount=2
第0轮计数  CurrentCount=1
第0轮计数  CurrentCount=0
第0轮计数完成
第1轮计数  CurrentCount=2
第1轮计数  CurrentCount=1
第1轮计数  CurrentCount=0
第1轮计数完成
第2轮计数  CurrentCount=2
第2轮计数  CurrentCount=1
第2轮计数  CurrentCount=0
第2轮计数完成
第3轮计数  CurrentCount=2
第3轮计数  CurrentCount=1
第3轮计数  CurrentCount=0
第3轮计数完成
第4轮计数  CurrentCount=2
第4轮计数  CurrentCount=1
第4轮计数  CurrentCount=0
第4轮计数完成
第5轮计数  CurrentCount=2
第5轮计数  CurrentCount=1
第5轮计数  CurrentCount=0
第5轮计数完成


CountdownEvent

有时候，需要对数目随时间变化的任务进行跟踪，CountdownEvent是一个非轻量级的同步原语，
与Task.WaitAll或者TaskFactory.ContinueWhenAll 等待其他任务完成执行而运行代码相比，
CountdownEvent的开销要小得多。

CountdownEvent实例带有一个初始的信号计数，在典型的fork/join场景下，
每当一个任务完成工作的时候，这个任务都会发出一个CountdownEvent实例的信号，
并将其信号计数递减1，调用CountdownEvent的wait方法的任务将会阻塞，直到信号计数达到0.
 */
