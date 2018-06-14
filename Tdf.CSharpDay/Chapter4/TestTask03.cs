using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter4
{
    public class TestTask03
    {
        private static ConcurrentQueue<Product> _queue = null;

        public static void TestMethod1()
        {
            _queue = new ConcurrentQueue<Product>();
            System.Threading.CancellationTokenSource token = new CancellationTokenSource();
            var tk1 = Task.Factory.StartNew(() => SetProduct(token.Token), token.Token);
            var tk2 = Task.Factory.StartNew(() => SetProduct(token.Token), token.Token);
            Thread.Sleep(10);
            /*取消任务操作*/
            token.Cancel();
            try
            {
                /*等待完成*/
                Task.WaitAll(new Task[] { tk1, tk2 });
            }
            catch (AggregateException ex)
            {
                /*如果当前的任务正在被取消，那么还会抛出一个TaskCanceledException异常，这个异常包含在AggregateException异常中*/
                Console.WriteLine("tk1 Canceled：{0}", tk1.IsCanceled);
                Console.WriteLine("tk1 Canceled：{0}", tk2.IsCanceled);
            }

            Thread.Sleep(2000);
            Console.WriteLine("tk1 Canceled：{0}", tk1.IsCanceled);
            Console.WriteLine("tk1 Canceled：{0}", tk2.IsCanceled);
            Console.ReadLine();
        }

        static void SetProduct(System.Threading.CancellationToken ct)
        {
            /* 每一次循环迭代，都会有新的代码调用 ThrowIfCancellationRequested 
             * 这行代码能够对 OpreationCanceledException 异常进行观察
             * 并且这个异常的标记与Task实例关联的那个标记进行比较，如果两者相同 ，而且IsCancelled属性为True，那么Task实例就知道存在一个要求取消的请求，并且会将状态转变为Canceled状态，中断任务执行。  
             * 如果当前的任务正在被取消，那么还会抛出一个TaskCanceledException异常，这个异常包含在AggregateException异常中
            /*检查取消标记*/
            ct.ThrowIfCancellationRequested();
            for (var i = 0; i < 50000; i++)
            {
                var model = new Product
                {
                    Name = "Name" + i,
                    SellPrice = i,
                    Category = "Category" + i
                };
                _queue.Enqueue(model);

                ct.ThrowIfCancellationRequested();
            }
            Console.WriteLine("SetProduct   执行完成");
        }

    }
}


/*
 * 通过取消标记取消任务
 * 通过取消标记来中断Task实例的执行。 
 * CancellationTokenSource，CancellationToken下的IsCanceled属性标志当前是否已经被取消，
 * 取消任务，任务也不一定会马上取消
 */
