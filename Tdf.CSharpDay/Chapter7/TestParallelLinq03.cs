using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq03
    {
        public static void TestMethod1()
        {
            ConcurrentQueue<Product> products = new ConcurrentQueue<Product>();
            /*向集合中添加多条数据*/
            Parallel.For(0, 6000000, (num) =>
            {
                products.Enqueue(new Product() { Category = "Category" + num, Name = "Name" + num, SellPrice = num });
            });


            /*采用并行化整个查询 查询符合条件的数据*/
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            var productListLinq = from product in products.AsParallel().WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                                  where (product.Name.Contains("1") && product.Name.Contains("2") && product.Category.Contains("1") && product.Category.Contains("2"))
                                  select product;
            Console.WriteLine($"采用并行化整个查询 查询得出数量为:{productListLinq.Count()}");
            sw.Stop();
            Console.WriteLine($"采用并行化整个查询 耗时:{sw.ElapsedMilliseconds}");


            /*采用默认设置 由.NET进行决策 查询符合条件的数据*/
            sw.Restart();
            var productListPLinq = from product in products.AsParallel().WithExecutionMode(ParallelExecutionMode.Default)
                                   where (product.Name.Contains("1") && product.Name.Contains("2") && product.Category.Contains("1") && product.Category.Contains("2"))
                                   select product;
            Console.WriteLine($"采用默认设置 由.NET进行决策 查询得出数量为:{productListPLinq.Count()}");
            sw.Stop();
            Console.WriteLine($"采用默认设置 由.NET进行决策 耗时:{sw.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}

/*
 * 采用并行化整个查询 查询得出数量为:1734166
采用并行化整个查询 耗时:858

采用默认设置 由.NET进行决策 查询得出数量为:1734166
采用默认设置 由.NET进行决策 耗时:516

    指定执行模式 WithExecutionMode

对串行化代码进行并行化，会带来一定的额外开销，Plinq查询执行并行化也是如此，
在默认情况下，执行PLINQ查询的时候，.NET机制会尽量避免高开销的并行化算法，
这些算法有可能会将执行的性能降低到地狱串行执行的性能。

.NET会根据查询的形态做出决策，并不开了数据集大小和委托执行的时间，
不过也可以强制并行执行，而不用考虑执行引擎分析的结果，
可以调用WithExecutionMode方法来进行设置。

 */
