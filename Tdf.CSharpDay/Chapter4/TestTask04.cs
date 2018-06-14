using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter4
{
    public class TestTask04
    {
        private static ConcurrentQueue<Product> _queue = null;
        public static void TestMethod1()
        {
            _queue = new ConcurrentQueue<Product>();
            System.Threading.CancellationTokenSource token = new CancellationTokenSource();
            var tk1 = Task.Factory.StartNew(() => SetProduct(token.Token), token.Token);
            Thread.Sleep(2000);
            if (tk1.IsFaulted)
            {
                /*  循环输出异常    */
                if (tk1.Exception != null)
                    foreach (Exception ex in tk1.Exception.InnerExceptions)
                    {
                        Console.WriteLine($"tk1 Exception：{ex.Message}");
                    }
            }
            Console.ReadLine();
        }

        static void SetProduct(System.Threading.CancellationToken ct)
        {
            for (var i = 0; i < 5; i++)
            {
                throw new Exception($"Exception Index {i}");
            }
            Console.WriteLine("SetProduct 执行完成");
        }
    }
}

/*
 * Task异常处理 当很多任务并行运行的时候，可能会并行发生很多异常。
 * Task实例能够处理一组一组的异常，这些异常有System.AggregateException类处理


 */
