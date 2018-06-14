using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync05
    {
        private static Task[] CookTasks { get; set; }
        private static object o = new object();
        private static StringBuilder _appendStrUnLock = new StringBuilder();
        private static StringBuilder _appendStrLock = new StringBuilder();
        private static readonly StringBuilder AppendStrMonitorLock = new StringBuilder();
        /*获取当前计算机处理器数*/
        private static readonly int Particpants = Environment.ProcessorCount;

        public static void TestMethod1()
        {
            var sl = new SpinLock();
            CookTasks = new Task[Particpants];
            Thread.Sleep(4000);
            Stopwatch swTask1 = new Stopwatch();
            swTask1.Start();
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    Parallel.For(1, 200000, (i) =>
                    {
                        var str = "append message " + i;
                        var lockTaken = false;
                        try
                        {
                            Monitor.Enter(o, ref lockTaken);
                            AppendStrMonitorLock.Append(str);
                        }
                        finally
                        {
                            if (lockTaken)
                                Monitor.Exit(o);
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
                Console.WriteLine($"采用Monitor操作,字符串长度：{AppendStrMonitorLock.Length}，耗时：{swTask1.ElapsedMilliseconds}");
                /*释放资源*/
            });

            Console.ReadLine();
        }

        public static void TestMethod2()
        {
            /*
             * 在实际的编程中需要注意的是：不要将SpinLock声明为只读字段，
             * 如果声明为只读字段，会导致每次调用都会返回一个SpinLock新副本，
             * 在多线程下，每个方法都会成功获得锁，而受到保护的临界区不会按照预期进行串行化。

             */
            SpinLock sl = new SpinLock();

            CookTasks = new Task[Particpants];
            Thread.Sleep(4000);
            Stopwatch swTask1 = new Stopwatch();
            swTask1.Start();
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    Parallel.For(1, 200000, (i) =>
                    {
                        var str = "append message " + i;
                        bool lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            AppendStrMonitorLock.Append(str);
                        }
                        finally
                        {
                            if (lockTaken)
                                sl.Exit();
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
                Console.WriteLine($"采用SpinLock操作,字符串长度：{AppendStrMonitorLock.Length}，耗时：{swTask1.ElapsedMilliseconds}");
                /*释放资源*/
            });

            Console.ReadLine();
        }

    }
}


/*
  * 采用Monitor操作,字符串长度：32710992，耗时：480
 * 采用SpinLock操作,字符串长度：32710992，耗时：360
 * 
 * 自旋锁 - System.Threading.SpinLock

如果持有锁的时间非常短，锁的粒度很精细，那么自旋锁可以获得比其他锁机制更好的性能，
互斥锁System.Threading.Monitor的开销非常大。



 */
