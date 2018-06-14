using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter5
{
    public class TestConcurrency01
    {
        private static object _o = new object();
        private static List<Product> Products { get; set; }

        public static void TestMethod1()
        {
            Products = new List<Product>();
            /*创建任务 t1  t1 执行 数据集合添加操作*/
            Task t1 = Task.Factory.StartNew(AddProducts);
            /*创建任务 t2  t2 执行 数据集合添加操作*/
            Task t2 = Task.Factory.StartNew(AddProducts);
            /*创建任务 t3  t3 执行 数据集合添加操作*/
            Task t3 = Task.Factory.StartNew(AddProducts);

            Task.WaitAll(t1, t2, t3);
            Console.WriteLine(Products.Count);
            Console.ReadLine();
        }

        public static void TestMethod2()
        {
            Products = new List<Product>();
            /*创建任务 t1  t1 执行 数据集合添加操作*/
            Task t1 = Task.Factory.StartNew(AddProducts2);
            /*创建任务 t2  t2 执行 数据集合添加操作*/
            Task t2 = Task.Factory.StartNew(AddProducts2);
            /*创建任务 t3  t3 执行 数据集合添加操作*/
            Task t3 = Task.Factory.StartNew(AddProducts2);

            Task.WaitAll(t1, t2, t3);
            Console.WriteLine(Products.Count);
            Console.ReadLine();
        }

        /*执行集合数据添加操作*/
        static void AddProducts()
        {
            Parallel.For(0, 1000, (i) =>
            {
                Product product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };
                Products.Add(product);
            });

        }

        static void AddProducts2()
        {
            Parallel.For(0, 1000, (i) =>
            {
                Product product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };

                lock (_o)
                {
                    Products.Add(product);
                }

            });

        }
    }
}


/*
 * 2961
 * 
 * 代码中开启了三个并发操作，每个操作都向集合中添加1000条数据，
 * 在没有保障线程安全和串行化的运行下，实际得到的数据并没有3000条
 * 
 * 但是锁的引入，带来了一定的开销和性能的损耗，并降低了程序的扩展性，在并发编程中显然不适用。
 * 
 * .NET Framework 4提供了新的线程安全和扩展的并发集合，
 * 它们能够解决潜在的死锁问题和竞争条件问题，
 * 因此在很多复杂的情形下它们能够使得并行代码更容易编写，
 * 这些集合尽可能减少需要使用锁的次数，
 * 从而使得在大部分情形下能够优化为最佳性能，不会产生不必要的同步开销。

需要注意的是：

线程安全并不是没有代价的，
比起System.Collenctions和System.Collenctions.Generic命名空间中的列表、集合和数组来说，
并发集合会有更大的开销。
因此，应该只在需要从多个任务中并发访问集合的时候才使用并发几个，
在串行代码中使用并发集合是没有意义的，因为它们会增加无谓的开销。

 */
