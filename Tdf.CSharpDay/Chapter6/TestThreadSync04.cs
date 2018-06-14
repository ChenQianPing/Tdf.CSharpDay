using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync04
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
            CookTasks = new Task[Particpants];
            Stopwatch swTask1 = new Stopwatch();
            swTask1.Start();
            for (var taskIndex = 0; taskIndex < Particpants; taskIndex++)
            {
                CookTasks[taskIndex] = Task.Factory.StartNew((num) =>
                {
                    try
                    {
                        Parallel.For(1, 200000, (i) =>
                        {
                            var str = "append message " + i;
                            var lockTaken = false;
                            try
                            {
                                Monitor.TryEnter(o, 2000, ref lockTaken);
                                if (!lockTaken)
                                {
                                    throw new OperationCanceledException("锁超时....");
                                }
                                if (i == 2)
                                {
                                    Thread.Sleep(40000);
                                }
                                AppendStrMonitorLock.Append(str);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                            finally
                            {
                                if (lockTaken)
                                    Monitor.Exit(o);
                            }
                        });
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }, taskIndex);
            }

            /*ContinueWhenAll 提供一组任务完成后 延续方法*/
            var finalTask = Task.Factory.ContinueWhenAll(CookTasks, (tasks) =>
            {
                /*等待任务完成*/
                Task.WaitAll(CookTasks);
                swTask1.Stop();
                foreach (Task task in CookTasks)
                {
                    if (task.Exception != null)
                    {
                        /*任务执行完成后  输出所有异常 打印异常报表*/
                        foreach (Exception exception in task.Exception.InnerExceptions)
                        {
                            Console.WriteLine($"异常信息:{exception.Message}");
                        }
                    }
                }

                Console.WriteLine($"不采用Lock操作,字符串长度：{AppendStrMonitorLock.Length}，耗时：{swTask1.ElapsedMilliseconds}");
                /*释放资源*/
            });

            Console.ReadLine();
        }
    }
}

/*
 * 锁超时  Monitor.TryEnter(o, 2000, ref lockTaken);

在多任务中，很多任务试图获得锁从而进入临界区，如果其中一个参与者不能释放锁，
那么其他所有的任务都要在Monitor.Enter的方法内永久的等待下去。
Monitor.TryEnter方法则提供了超时机制
 */
