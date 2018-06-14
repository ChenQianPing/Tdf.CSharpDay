using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync03
    {
        private static Task[] CookTasks { get; set; }
        private static readonly object O = new object();
        private static readonly StringBuilder AppendStrUnLock = new StringBuilder();
        private static readonly StringBuilder AppendStrLock = new StringBuilder();
        private static readonly StringBuilder AppendStrMonitorLock = new StringBuilder();

        /// <summary>
        /// 获取当前计算机处理器数
        /// </summary>
        private static readonly int Particpants = Environment.ProcessorCount;

        public static void TestMethod1()
        {
            CookTasks = new Task[Particpants];
            var swTask1 = new Stopwatch();
            swTask1.Start();
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    Parallel.For(1, 1000, (i) =>
                    {
                        var str = "append message " + i;
                        AppendStrUnLock.Append(str);
                    });
                }, taskIndex);
            }

            /*ContinueWhenAll 提供一组任务完成后 延续方法*/
            var finalTask = Task.Factory.ContinueWhenAll(CookTasks, (tasks) =>
            {
                /*等待任务完成*/
                Task.WaitAll(CookTasks);
                swTask1.Stop();
                Console.WriteLine($"不采用Lock操作,字符串长度：{AppendStrUnLock.Length}，耗时：{swTask1.ElapsedMilliseconds}");
                /*释放资源*/
            });

            Console.ReadLine();
        }

        public static void TestMethod2()
        {
            // 采用Lock机制代码如下
            CookTasks = new Task[Particpants];
            Stopwatch swTask1 = new Stopwatch();
            swTask1.Start();
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    Parallel.For(1, 1000, (i) =>
                    {
                        var str = "append message " + i;
                        lock (O)
                        {
                            AppendStrLock.Append(str);
                        }
                    });
                }, taskIndex);
            }

            /*ContinueWhenAll 提供一组任务完成后 延续方法*/
            var finalTask = Task.Factory.ContinueWhenAll(CookTasks, (tasks) =>
            {
                /*等待任务完成*/
                Task.WaitAll(CookTasks);
                swTask1.Stop();
                Console.WriteLine($"采用Lock操作,字符串长度：{AppendStrLock.Length}，耗时：{swTask1.ElapsedMilliseconds}");
                /*释放资源*/
            });

            Console.ReadLine();
        }

        public static void TestMethod3()
        {
            CookTasks = new Task[Particpants];
            var swTask1 = new Stopwatch();
            swTask1.Start();
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    Parallel.For(1, 1000, (i) =>
                    {
                        var str = "append message " + i;
                        var lockTaken = false;
                        try
                        {
                            Monitor.Enter(O, ref lockTaken);
                            AppendStrMonitorLock.Append(str);
                        }
                        finally
                        {
                            if (lockTaken)
                                Monitor.Exit(O);
                        }
                    });
                }, taskIndex);
            }

            /*ContinueWhenAll 提供一组任务完成后 延续方法*/
            var finalTask = Task.Factory.ContinueWhenAll(CookTasks, (tasks) =>
            {
                /*等待任务完成*/
                Task.WaitAll(CookTasks);
                swTask1.Stop();
                Console.WriteLine($"采用互斥锁操作,字符串长度：{AppendStrMonitorLock.Length}，耗时：{swTask1.ElapsedMilliseconds}");
                /*释放资源*/
            });

            Console.ReadLine();
        }
    }
}

/*
 * 锁的特性

互斥和可见性。互斥指的是一次只允许一个线程持有某个特定的锁，因此可以保证共享数据内容的一致性；
可见性指的是必须确保锁被释放之前对共享数据的修改，随后获得锁的另一个线程能够知道该行为。

    互斥锁-System.Threading.Monitor
如果有一个临界区，一次只有一个任务能够访问这个临界区，
但是这个临界区需要被很多任务循环访问，那么使用任务延续并不是一个好的选择，
那么另一种替换方案就是采用互斥锁原语。

下面已操作字符串为示意，看下不采用锁，采用传统的LOCK和采用互斥锁的区别

 * 不采用Lock操作,字符串长度：112432，耗时：108
 *   采用Lock操作,字符串长度：142992，耗时：267
 *  采用互斥锁操作,字符串长度：142992，耗时：225
 *  
 *  System.Threading.Monitor 类通过使用互斥锁提供了对象的同步访问机制，
 *  使用Lock关键字的等价代码使用起来更加简洁，不需要额外的异常捕获和处理代码。

但是System.Threading.Monitor好处是提供了些其他的方法(Lock中却没有)，
通过这些方法可以对锁的过程有更多的控制。
需要注意的是 Lock关键字和System.Threading.Monitor类仍然是提供互斥访问的首选方法，
不过在某些情形下，其他互斥锁原语可能会提供更好的性能和更小的开销，
如SpinLock，Lock和System.Threading.Monitor类智能锁定对象，即引用类型。
 */
