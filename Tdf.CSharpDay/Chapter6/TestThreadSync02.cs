using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync02
    {
        private static Task[] CookTasks { get; set; }
        private static Barrier Barrier { get; set; }
        /*获取当前计算机处理器数*/
        private static readonly int Particpants = Environment.ProcessorCount;

        public static void TestMethod1()
        {
            Console.WriteLine($"定义{Particpants}个人煮饭3次");
            CookTasks = new Task[Particpants];
            Barrier = new Barrier(Particpants, (barrier) =>
            {
                Console.WriteLine($"当前阶段:{barrier.CurrentPhaseNumber}");
            });

            Stopwatch swTask1 = new Stopwatch();
            swTask1.Start();

            /*定义N个人*/
            for (var cookPerson = 0; cookPerson < Particpants; cookPerson++)
            {
                CookTasks[cookPerson] = Task.Factory.StartNew((num) =>
                {
                    var index = Convert.ToInt32(num);
                    /*每个人煮3次饭*/
                    for (var cookCount = 0; cookCount < 3; cookCount++)
                    {
                        CookStepTask1(index, cookCount);
                        /*处理等待中的异常 如果等待时间超过300毫秒的话则抛出
                         * 参考方法体1中 模拟了超时操作， 则屏障等待时 如果发现超时 则处理异常
                         */
                        try
                        {
                            /*屏障 等待超过2秒钟 其执行算法有问题 超时  则抛出异常 记录信息 提醒开发人员观察*/
                            if (!Barrier.SignalAndWait(2000))
                            {
                                /*抛出超时异常*/
                                throw new OperationCanceledException("等待超时，抛出异常");
                            }
                        }
                        catch (Exception ex)
                        {
                            /*处理异常*/
                            Console.WriteLine(ex.Message);
                            continue;
                        }
                        CookStepTask2(index, cookCount);
                        Barrier.SignalAndWait();
                        CookStepTask3(index, cookCount);
                        Barrier.SignalAndWait();
                        CookStepTask4(index, cookCount);
                        Barrier.SignalAndWait();
                        CookStepTask5(index, cookCount);
                        Barrier.SignalAndWait();
                    }
                }, cookPerson);
            }

            /*ContinueWhenAll 提供一组任务完成后 延续方法*/
            var finalTask = Task.Factory.ContinueWhenAll(CookTasks, (tasks) =>
            {
                foreach (var task in CookTasks)
                {
                    if (task.Exception != null)
                    {
                        /*任务执行完成后  输出所有异常 打印异常报表*/
                        foreach (Exception exception in task.Exception.InnerExceptions)
                        {
                            Console.WriteLine("异常信息:{0}", exception.Message);
                        }
                    }
                }
                /*等待任务完成*/
                Task.WaitAll(CookTasks);
                swTask1.Stop();
                Console.WriteLine("采用并发 {1}个人煮3次饭耗时:{0}", swTask1.ElapsedMilliseconds, Particpants);
                /*释放资源*/
                Barrier.Dispose();

            });

            Console.ReadLine();
        }

        /*1.打水*/
        private static void CookStepTask1(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 打水... 耗时2分钟");

            /*模拟一个方法体内异常抛出*/
            //throw new Exception("抛出一个代码异常");
            if (pesronIndex == 0)
            {
                /*模拟超时操作*/
                //SpinWait.SpinUntil(() => (_barrier.ParticipantsRemaining == 0), 5000);
                Thread.Sleep(5000);
            }
        }
        /*2.淘米*/
        private static void CookStepTask2(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 淘米... 耗时3分钟");
        }

        /*3.放入锅中*/
        private static void CookStepTask3(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 放入锅中... 耗时1分钟");
        }

        /*4.盖上锅盖*/
        private static void CookStepTask4(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 盖上锅盖... 耗时1分钟");
        }

        /*5.生火煮饭*/
        private static void CookStepTask5(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次  生火煮饭... 耗时30分钟");
        }

    }
}


/*
 * 在 CookStepTask1 方法体中，我模拟了超时和异常，
 * 并在Task任务中，利用Barrier的SignalAndWait方法处理屏障中的超时信息，
 * 和Task中异常记录信息。
 */
