using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter7
{
    public class TestParallelLinq01
    {
        public static void TestMethod1()
        {
            ConcurrentQueue<Product> products = new ConcurrentQueue<Product>();
            /*向集合中添加多条数据  可以修改数据量查看Linq和Plinq的性能*/
            Parallel.For(0, 600000, (num) =>
            {
                products.Enqueue(new Product() { Category = "Category" + num, Name = "Name" + num, SellPrice = num });
            });


            /*采用LINQ查询符合条件的数据*/
            var sw = new Stopwatch();
            sw.Restart();
            var productListLinq = from product in products
                                  where (product.Name.Contains("1") && product.Name.Contains("2") && product.Category.Contains("1") && product.Category.Contains("2"))
                                  select product;
            Console.WriteLine($"采用Linq 查询得出数量为:{productListLinq.Count()}");
            sw.Stop();
            Console.WriteLine($"采用Linq 耗时:{sw.ElapsedMilliseconds}");


            /*采用PLINQ查询符合条件的数据*/
            sw.Restart();
            var productListPLinq = from product in products.AsParallel() /*AsParallel 试图利用运行时所有可用的逻辑内核，从而使运行的速度比串行的版本要快 但是需要注意开销所带来的性能损耗*/
                                   where (product.Name.Contains("1") && product.Name.Contains("2") && product.Category.Contains("1") && product.Category.Contains("2"))
                                   select product;
            Console.WriteLine($"采用PLinq 查询得出数量为:{productListPLinq.Count()}");
            sw.Stop();
            Console.WriteLine($"采用PLinq 耗时:{sw.ElapsedMilliseconds}");
            Console.ReadLine();
        }
    }
}


/*
 * 采用Linq 查询得出数量为:140582
采用Linq 耗时:214

采用PLinq 查询得出数量为:140582
采用PLinq 耗时:49

    AsParallel() 启用查询的并行化
    当前模拟的数据量比较少，数据量越多，采用并行化查询的效果越明显

通过LINQ可以方便的查询并处理不同的数据源，
使用Parallel LINQ (PLINQ)来充分获得并行化所带来的优势。

PLINQ不仅实现了完整的LINQ操作符，而且还添加了一些用于执行并行的操作符，
与对应的LINQ相比，通过PLINQ可以获得明显的加速，但是具体的加速效果还要取决于具体的场景，
不过在并行化的情况下一段会加速。

如果一个查询涉及到大量的计算和内存密集型操作，而且顺序并不重要，
那么加速会非常明显，然而，如果顺序很重要，那么加速就会受到影响。


 */
