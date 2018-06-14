using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync08
    {
        public static void TestMethod1()
        {
            SemaphoreSlim ss = new SemaphoreSlim(3); // 创建SemaphoreSlim 初始化信号量最多计数为3次
            Console.WriteLine("创建SemaphoreSlim 初始化信号量最多计数为{0}次", ss.CurrentCount);

            // Launch an asynchronous Task that releases the semaphore after 100 ms
            Task t1 = Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    /*阻止当前线程，直至它可进入 SemaphoreSlim 为止。*/
                    /*阻塞当前任务或线程，直到信号量几首大于0时它才能进入信号量*/
                    ss.Wait();
                    Console.WriteLine("允许进入 SemaphoreSlim 的线程的数量：{0}", ss.CurrentCount);
                    Thread.Sleep(10);
                }
            });

            Thread.Sleep(3000);
            /*当前Task只能进入3次*/
            /*退出一次信号量  并递增信号量的计数*/
            Console.WriteLine("退出一次信号量  并递增信号量的计数");
            ss.Release();

            Thread.Sleep(3000);
            /*退出3次信号量  并递增信号量的计数*/
            Console.WriteLine("退出三次信号量  并递增信号量的计数");
            ss.Release(3);

            /*等待任务完成*/
            Task.WaitAll(t1);

            /*释放*/
            ss.Dispose();
            Console.ReadLine();
        }
    }
}


/*
 * 
 * SemaphoreSlim

有时候,需要对访问一个资源或者一个资源池的并发任务或者线程的数量做限制时，
采用System.Threading.SemaphoreSlim类非常有用。

该了表示一个Windows内核信号量对象，如果等待的时间非常短，
System.Threading.SemaphoreSlim类带来的额外开销会更少，而且更适合对任务处理，
System.Threading.SemaphoreSlim提供的计数信号量没有使用Windows内核的信号量。

计数信号量：通过跟踪进入和离开任务或线程来协调对资源的访问，
信号量需要知道能够通过信号量协调机制所访问共享资源的最大任务数，
然后，信号量使用了一个计数器，根据任务进入或离开信号量控制区对计数器进行加减。

需要注意的是：信号量会降低可扩展型，而且信号量的目的就是如此。
SemaphoreSlim实例并不能保证等待进入信号量的任务或线程的顺序。

 * 
 * 创建SemaphoreSlim 初始化信号量最多计数为3次
允许进入 SemaphoreSlim 的线程的数量：2
允许进入 SemaphoreSlim 的线程的数量：1
允许进入 SemaphoreSlim 的线程的数量：0
退出一次信号量  并递增信号量的计数
允许进入 SemaphoreSlim 的线程的数量：0
退出三次信号量  并递增信号量的计数
允许进入 SemaphoreSlim 的线程的数量：2
允许进入 SemaphoreSlim 的线程的数量：1
允许进入 SemaphoreSlim 的线程的数量：0
 */
