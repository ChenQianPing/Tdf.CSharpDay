using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq06
    {
        public static void TestMethod1()
        {
            ConcurrentQueue<Product> products = new ConcurrentQueue<Product>();
            /*向集合中添加多条数据*/
            Parallel.For(0, 600000, (num) =>
            {
                products.Enqueue(new Product() { Category = "Category" + num, Name = "Name" + num, SellPrice = num });
            });

            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            /*创建tk1 任务  查询 符合 条件的数据*/
            Task<ParallelQuery<Product>> tk1 = new Task<ParallelQuery<Product>>((ct) =>
            {
                var result = products.AsParallel();
                try
                {
                    Console.WriteLine("开始执行 tk1 任务", products.Count);
                    Console.WriteLine("tk1 任务中 数据结果集数量为：{0}", products.Count);

                    result = products.AsParallel().WithCancellation(token).Where(p => p.Name.Contains("1") && p.Name.Contains("2"));


                    /*
                     * 指定查询时所需的并行度 WithDegreeOfParallelism
                        默认情况下，PLINQ总是会试图利用所有的可用逻辑内核达到最佳性能，
                        在程序中我们可以利用WithDegreeOfParallelism方法指定一个不同最大并行度。

                    好处：如果计算机有8个可用的逻辑内核，PLINQ查询最多运行4个并发任务，
                    这样可用使用Parallel.Invoke 加载多个带有不同并行度的PLINQ查询，
                    有一些PLINQ查询的可扩展性有限，因此这些选项可用让您充分利用额外的内核。


                     * tk1任务 采用所有可用处理器*/
                    // result = products.AsParallel().WithCancellation(token).WithDegreeOfParallelism(Environment.ProcessorCount).Where(p => p.Name.Contains("1") && p.Name.Contains("2") && p.Category.Contains("1") && p.Category.Contains("2"));
                    /*tk1任务 采用1个可用处理器*/
                    // result = products.AsParallel().WithCancellation(token).WithDegreeOfParallelism(1).Where(p => p.Name.Contains("1") && p.Name.Contains("2") && p.Category.Contains("1") && p.Category.Contains("2"));


                }
                catch (AggregateException ex)
                {
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        Console.WriteLine("tk3 错误:{0}", e.Message);
                    }
                }
                return result;
            }, cts.Token);

            /*创建tk2 任务，在执行tk1任务完成  基于tk1的结果查询 符合 条件的数据*/
            Task<ParallelQuery<Product>> tk2 = tk1.ContinueWith<ParallelQuery<Product>>((tk) =>
            {
                var result = tk.Result;
                try
                {
                    Console.WriteLine("开始执行 tk2 任务", products.Count);
                    Console.WriteLine("tk2 任务中 数据结果集数量为：{0}", tk.Result.Count());
                    result = tk.Result.WithCancellation(token).Where(p => p.Category.Contains("1") && p.Category.Contains("2"));
                }
                catch (AggregateException ex)
                {
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        Console.WriteLine("tk3 错误:{0}", e.Message);
                    }
                }
                return result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            /*创建tk3 任务，在执行tk1任务完成  基于tk1的结果查询 符合 条件的数据*/
            Task<ParallelQuery<Product>> tk3 = tk1.ContinueWith<ParallelQuery<Product>>((tk) =>
            {
                var result = tk.Result;
                try
                {
                    Console.WriteLine("开始执行 tk3 任务", products.Count);
                    Console.WriteLine("tk3 任务中 数据结果集数量为：{0}", tk.Result.Count());
                    result = tk.Result.WithCancellation(token).Where(p => p.SellPrice > 1111 && p.SellPrice < 222222);
                }
                catch (AggregateException ex)
                {
                    foreach (Exception e in ex.InnerExceptions)
                    {
                        Console.WriteLine("tk3 错误:{0}", e.Message);
                    }
                }
                return result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            tk1.Start();

            try
            {
                Thread.Sleep(10);
                cts.Cancel();//取消任务
                Task.WaitAll(tk1, tk2, tk3);

                Console.WriteLine("tk2任务结果输出，筛选后记录总数为:{0}", tk2.Result.Count());
                Console.WriteLine("tk3任务结果输出，筛选后记录总数为:{0}", tk3.Result.Count());
            }
            catch (AggregateException ex)
            {
                foreach (Exception e in ex.InnerExceptions)
                {
                    Console.WriteLine("错误:{0}", e.Message);
                }
            }

            tk1.Dispose();
            tk2.Dispose();
            tk3.Dispose();
            cts.Dispose();
            Console.ReadLine();
        }
    }
}


/*
 * 开始执行 tk1 任务
tk1 任务中 数据结果集数量为：600000
开始执行 tk2 任务
开始执行 tk3 任务
错误:已通过提供给 WithCancellation 的标记取消了查询。
错误:已通过提供给 WithCancellation 的标记取消了查询。

取消PLINQ WithCancellation
通过WithCancellation取消当前PLINQ正在执行的查询操作

 */
