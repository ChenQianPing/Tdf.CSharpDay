using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter6
{
    public class TestThreadSync01
    {
        private static Task[] CookTasks { get; set; }
        private static Barrier Barrier { get; set; }

        /// <summary>
        /// 获取当前计算机处理器数
        /// </summary>
        private static readonly int Particpants = Environment.ProcessorCount;

        public static void TestMethod1()
        {
            Console.WriteLine($@"定义{Particpants}个人煮饭3次");
            CookTasks = new Task[Particpants];
            Barrier = new Barrier(Particpants, (barrier) =>
            {
                Console.WriteLine($"当前阶段:{barrier.CurrentPhaseNumber}");
            });

            var swTask1 = new Stopwatch();
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
                        CookStepTask2(index, cookCount);
                        CookStepTask3(index, cookCount);
                        CookStepTask4(index, cookCount);
                        CookStepTask5(index, cookCount);
                    }
                }, cookPerson);
            }

            /*ContinueWhenAll 提供一组任务完成后 延续方法*/
            var finalTask = Task.Factory.ContinueWhenAll(CookTasks, (tasks) =>
            {
                /*等待任务完成*/
                Task.WaitAll(CookTasks);
                swTask1.Stop();
                Console.WriteLine(@"采用并发 {1}个人煮3次饭耗时:{0}", swTask1.ElapsedMilliseconds, Particpants);
                /*释放资源*/
                Barrier.Dispose();
            });

            Console.ReadLine();

        }

        public static void TestMethod2()
        {
            Console.WriteLine($@"定义{Particpants}个人煮饭3次");

            Thread.Sleep(1000);
            var swTask = new Stopwatch();
            swTask.Start();

            /*定义N个人*/
            for (var cookPerson = 0; cookPerson < Particpants; cookPerson++)
            {
                /*每个人煮3次饭*/
                for (var cookCount = 0; cookCount < 3; cookCount++)
                {
                    CookStep1(cookPerson, cookCount); CookStep2(cookPerson, cookCount); CookStep3(cookPerson, cookCount); CookStep4(cookPerson, cookCount); CookStep5(cookPerson, cookCount);
                }
            }
            swTask.Stop();
            Console.WriteLine("不采用并发 {1}个人煮3次饭耗时:{0}", swTask.ElapsedMilliseconds, Particpants);
            Thread.Sleep(2000);

            Console.ReadLine();

        }

        /// <summary>
        /// 1.打水
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStepTask1(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 打水... 耗时2分钟");
            Thread.Sleep(200);
            /*存在线程暂停 所以需要将 _barrier.SignalAndWait();放在方法中 */
            Barrier.SignalAndWait();
        }

        /// <summary>
        /// 2.淘米
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStepTask2(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 淘米... 耗时3分钟");
            Thread.Sleep(300);
            /*存在线程暂停 所以需要将 _barrier.SignalAndWait();放在方法中 */
            Barrier.SignalAndWait();
        }

        /// <summary>
        /// 3.放入锅中
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStepTask3(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 放入锅中... 耗时1分钟");
            Thread.Sleep(100);
            /*存在线程暂停 所以需要将 _barrier.SignalAndWait();放在方法中 */
            Barrier.SignalAndWait();
        }

        /// <summary>
        /// 4.盖上锅盖
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStepTask4(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 盖上锅盖... 耗时1分钟");
            Thread.Sleep(100);
            /*存在线程暂停 所以需要将 _barrier.SignalAndWait();放在方法中 */
            Barrier.SignalAndWait();
        }

        /// <summary>
        /// 5.生火煮饭
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStepTask5(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次  生火煮饭... 耗时30分钟");
            Thread.Sleep(500);
            /*存在线程暂停 所以需要将 _barrier.SignalAndWait();放在方法中 */
            Barrier.SignalAndWait();
        }

        /// <summary>
        /// 1.打水
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStep1(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 打水... 耗时2分钟");
            Thread.Sleep(200);
        }

        /// <summary>
        /// 2.淘米
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStep2(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 淘米... 耗时3分钟");
            Thread.Sleep(300);
        }

        /// <summary>
        /// 3.放入锅中
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStep3(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 放入锅中... 耗时1分钟");
            Thread.Sleep(100);
        }

        /// <summary>
        /// 4.盖上锅盖
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStep4(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次 盖上锅盖... 耗时1分钟");
            Thread.Sleep(100);
        }

        /// <summary>
        /// 5.生火煮饭
        /// </summary>
        /// <param name="pesronIndex"></param>
        /// <param name="index"></param>
        private static void CookStep5(int pesronIndex, int index)
        {
            Console.WriteLine($"{pesronIndex} 第{index}次  生火煮饭... 耗时30分钟");
            Thread.Sleep(500);
        }
    }
}


/*
 * 采用并发 8个人煮3次饭耗时:8116
 * 而采用并发处理中，使用 Barrier，不仅保证了任务的有序进行，
 * 还在性能损耗上得到了最大程度的降低。


 * 
不采用并发 8个人煮3次饭耗时:28803

    如代码所示，在串行代码中，虽然任务是有序进行，但是等待的时间很长，
    因为只是在一个处理器下进行处理。



 * 
 */
