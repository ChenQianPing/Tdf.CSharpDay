using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq05
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
            /*创建tk1 任务  查询 符合 条件的数据*/
            Task<ParallelQuery<Product>> tk1 = new Task<ParallelQuery<Product>>((ct) =>
            {
                Console.WriteLine("开始执行 tk1 任务", products.Count);
                Console.WriteLine("tk1 任务中 数据结果集数量为：{0}", products.Count);
                var result = products.AsParallel().Where(p => p.Name.Contains("1") && p.Name.Contains("2"));
                return result;
            }, cts.Token);

            /*创建tk2 任务，在执行tk1任务完成  基于tk1的结果查询 符合 条件的数据*/
            Task<ParallelQuery<Product>> tk2 = tk1.ContinueWith<ParallelQuery<Product>>((tk) =>
            {
                Console.WriteLine("开始执行 tk2 任务", products.Count);
                Console.WriteLine("tk2 任务中 数据结果集数量为：{0}", tk.Result.Count());
                var result = tk.Result.Where(p => p.Category.Contains("1") && p.Category.Contains("2"));
                return result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            /*创建tk3 任务，在执行tk1任务完成  基于tk1的结果查询 符合 条件的数据*/
            Task<ParallelQuery<Product>> tk3 = tk1.ContinueWith<ParallelQuery<Product>>((tk) =>
            {
                Console.WriteLine("开始执行 tk3 任务", products.Count);
                Console.WriteLine("tk3 任务中 数据结果集数量为：{0}", tk.Result.Count());
                var result = tk.Result.Where(p => p.SellPrice > 1111 && p.SellPrice < 222222);
                return result;
            }, TaskContinuationOptions.OnlyOnRanToCompletion);

            tk1.Start();

            Task.WaitAll(tk1, tk2, tk3);
            Console.WriteLine("tk2任务结果输出，筛选后记录总数为:{0}", tk2.Result.Count());
            Console.WriteLine("tk3任务结果输出，筛选后记录总数为:{0}", tk3.Result.Count());

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
开始执行 tk3 任务
开始执行 tk2 任务
tk3 任务中 数据结果集数量为：140582
tk2 任务中 数据结果集数量为：140582
tk2任务结果输出，筛选后记录总数为:140582
tk3任务结果输出，筛选后记录总数为:70388

并发PLINQ任务

如代码所示tk1,tk2,tk3三个任务，
tk2,tk3任务的运行需要基于tk1任务的结果，
因此，参数中指定了TaskContinuationOptions.OnlyOnRanToCompletion，
通过这种方式，每个被串联的任务都会等待之前的任务完成之后才开始执行，
tk2,tk3在tk1执行完成后，这两个任务的PLINQ查询可以并行运行，并将会可能地使用多个逻辑内核。


 */
