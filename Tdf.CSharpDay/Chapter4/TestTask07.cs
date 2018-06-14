using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter4
{
    public class TestTask07
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
                throw new Exception("异常");
            });

            /*创建任务t2   t2任务的执行 依赖与t1任务的执行完成*/
            Task t2 = t1.ContinueWith((t) =>
            {
                Console.WriteLine(t.Status);
                Console.WriteLine("执行 t2 任务");
                SpinWait.SpinUntil(() =>
                {
                    return false;
                }, 2000);

                /*定义 TaskContinuationOptions 行为为 NotOnFaulted 在 t1 任务抛出异常后，t1 的任务状态为 Faulted ， 则t2 不会执行里面的方法 但是需要注意的是t3任务*/
                /*t2在不符合条件时 返回Canceled状态状态让t3任务执行*/
            }, TaskContinuationOptions.NotOnFaulted);

            /*创建任务t3   t3任务的执行 依赖与t2任务的执行完成*/
            /*t2在不符合条件时 返回Canceled状态状态让t3任务执行*/
            Task t3 = t2.ContinueWith((t) =>
            {
                Console.WriteLine(t.Status);
                Console.WriteLine("执行 t3 任务");
            });

            Console.ReadLine();
        }
    }
}


/*
 * TaskContinuationOptions
TaskContinuationOptions参数，可以控制延续另一个任的任务调度和执行的可选行为

 * 执行 t1 任务
Canceled
执行 t3 任务

 */
