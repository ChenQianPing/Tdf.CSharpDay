using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter5
{
    public class TestConcurrency02
    {
        private static readonly object O = new object();
        /*定义 Queue*/
        private static Queue<Product> Products { get; set; }
        private static ConcurrentQueue<Product> ConcurrenProducts { get; set; }

        public static void TestMethod1()
        {
            Thread.Sleep(1000);
            Products = new Queue<Product>();
            var swTask = new Stopwatch();
            swTask.Start();

            /*创建任务 t1  t1 执行 数据集合添加操作*/
            var t1 = Task.Factory.StartNew(AddProducts);
            /*创建任务 t2  t2 执行 数据集合添加操作*/
            var t2 = Task.Factory.StartNew(AddProducts);
            /*创建任务 t3  t3 执行 数据集合添加操作*/
            var t3 = Task.Factory.StartNew(AddProducts);

            Task.WaitAll(t1, t2, t3);
            swTask.Stop();
            Console.WriteLine("List<Product> 当前数据量为：" + Products.Count);
            Console.WriteLine("List<Product> 执行时间为：" + swTask.ElapsedMilliseconds);

            Thread.Sleep(1000);
            ConcurrenProducts = new ConcurrentQueue<Product>();
            var swTask1 = new Stopwatch();
            swTask1.Start();

            /*创建任务 tk1  tk1 执行 数据集合添加操作*/
            var tk1 = Task.Factory.StartNew(AddConcurrenProducts);
            /*创建任务 tk2  tk2 执行 数据集合添加操作*/
            var tk2 = Task.Factory.StartNew(AddConcurrenProducts);
            /*创建任务 tk3  tk3 执行 数据集合添加操作*/
            var tk3 = Task.Factory.StartNew(AddConcurrenProducts);

            Task.WaitAll(tk1, tk2, tk3);
            swTask1.Stop();

            Console.WriteLine("ConcurrentQueue<Product> 当前数据量为：" + ConcurrenProducts.Count);
            Console.WriteLine("ConcurrentQueue<Product> 执行时间为：" + swTask1.ElapsedMilliseconds);
            Console.ReadLine();
        }

        /*执行集合数据添加操作*/
        static void AddProducts()
        {
            Parallel.For(0, 30000, (i) =>
            {
                var product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };

                lock (O)
                {
                    Products.Enqueue(product);
                }
            });

        }

        /*执行集合数据添加操作*/
        static void AddConcurrenProducts()
        {
            Parallel.For(0, 30000, (i) =>
            {
                var product = new Product
                {
                    Name = "name" + i,
                    Category = "Category" + i,
                    SellPrice = i
                };
                ConcurrenProducts.Enqueue(product);
            });

        }

    }
}


/*
 * 
 * 需要注意的是，代码中的输出时间并不能够完全正确的展示出并发代码下的ConcurrentQueue性能，
 * 采用ConcurrentQueue在一定程度上也带来了损耗
 * 
 * List<Product> 当前数据量为：90000
List<Product> 执行时间为：91
ConcurrentQueue<Product> 当前数据量为：90000
ConcurrentQueue<Product> 执行时间为：109
 */
