using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq08
    {
        public static void TestMethod1()
        {
            Console.WriteLine("当前计算机处理器数：{0}", Environment.ProcessorCount);
            ConcurrentQueue<Product> products = new ConcurrentQueue<Product>();
            /*向集合中添加多条数据*/
            Parallel.For(0, 600000, (num) =>
            {
                products.Enqueue(new Product() { Category = "Category" + num, Name = "Name" + num, SellPrice = num });
            });

            Stopwatch sw = new Stopwatch();
            Thread.Sleep(1000);
            sw.Restart();
            var count = 0;
            Task tk1 = Task.Factory.StartNew(() =>
            {
                var result = products.AsParallel().WithMergeOptions(ParallelMergeOptions.AutoBuffered).Where(p => p.Name.Contains("1") && p.Name.Contains("2") && p.Category.Contains("1") && p.Category.Contains("2"));
                count = result.Count();
            });
            Task.WaitAll(tk1);
            sw.Stop();
            Console.WriteLine("ParallelMergeOptions.AutoBuffered 耗时：{0},数量：{1}", sw.ElapsedMilliseconds, count);

            sw.Restart();
            var count1 = 0;
            Task tk2 = Task.Factory.StartNew(() =>
            {
                var result = products.AsParallel().WithMergeOptions(ParallelMergeOptions.Default).Where(p => p.Name.Contains("1") && p.Name.Contains("2") && p.Category.Contains("1") && p.Category.Contains("2"));
                count1 = result.Count();
            });
            Task.WaitAll(tk2);
            sw.Stop();
            Console.WriteLine("ParallelMergeOptions.Default 耗时：{0},数量：{1}", sw.ElapsedMilliseconds, count1);


            sw.Restart();
            var count2 = 0;
            Task tk3 = Task.Factory.StartNew(() =>
            {
                var result = products.AsParallel().WithMergeOptions(ParallelMergeOptions.FullyBuffered).Where(p => p.Name.Contains("1") && p.Name.Contains("2") && p.Category.Contains("1") && p.Category.Contains("2"));
                count2 = result.Count();
            });
            Task.WaitAll(tk3);
            sw.Stop();
            Console.WriteLine("ParallelMergeOptions.FullyBuffered 耗时：{0},数量：{1}", sw.ElapsedMilliseconds, count2);


            sw.Restart();
            var count3 = 0;
            Task tk4 = Task.Factory.StartNew(() =>
            {
                var result = products.AsParallel().WithMergeOptions(ParallelMergeOptions.NotBuffered).Where(p => p.Name.Contains("1") && p.Name.Contains("2") && p.Category.Contains("1") && p.Category.Contains("2"));
                count3 = result.Count();
            });
            Task.WaitAll(tk4);
            sw.Stop();
            Console.WriteLine("ParallelMergeOptions.NotBuffered 耗时：{0},数量：{1}", sw.ElapsedMilliseconds, count3);

            tk4.Dispose();
            tk3.Dispose();
            tk2.Dispose();
            tk1.Dispose();
            Console.ReadLine();
        }
    }
}

/*
 * 当前计算机处理器数：8
ParallelMergeOptions.AutoBuffered 耗时：56,数量：140582
ParallelMergeOptions.Default 耗时：43,数量：140582
ParallelMergeOptions.FullyBuffered 耗时：47,数量：140582
ParallelMergeOptions.NotBuffered 耗时：47,数量：140582

    WithMergeOptions

通过WithMergeOptions扩展方法提示PLINQ应该优先使用哪种方式合并并行结果片段
需要注意的是：每一个选项都有其优点和缺点，
因此一定奥测量显示第一个结果的时间以及完成整个查询所需要的时间，这点很重要 。


 */
