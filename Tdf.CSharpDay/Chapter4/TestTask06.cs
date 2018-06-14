using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter4
{
    public class TestTask06
    {
        public static void TestMethod1()
        {
            /*创建任务t1*/
            Task t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("执行 t1 任务");
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, 2000);

            });

            /*创建任务t2   t2任务的执行 依赖与t1任务的执行完成*/
            Task t2 = t1.ContinueWith((t) =>
            {
                Console.WriteLine("执行 t2 任务");
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, 2000);

            });

            /*创建任务t3   t3任务的执行 依赖与t2任务的执行完成*/
            Task t3 = t2.ContinueWith((t) =>
            {
                Console.WriteLine("执行 t3 任务");
            });
            Console.ReadLine();
        }
    }
}


/*
 * 通过延续串联多个任务
 ContinueWith:创建一个目标Task完成时，异步执行的延续程序，await
 * 
 * 执行 t1 任务
执行 t2 任务
执行 t3 任务

 */
