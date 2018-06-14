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
    public class TestConcurrency04
    {
        private static readonly object O = new object();
        /*定义 Stack*/
        private static Stack<Product> Products { get; set; }
        private static ConcurrentStack<Product> ConcurrenProducts { get; set; }

        public static void TestMethod1()
        {
            Thread.Sleep(1000);
            Products = new Stack<Product>();
            var swTask = new Stopwatch();
            swTask.Start();

            /*创建任务 t1  t1 执行 数据集合添加操作*/
            Task t1 = Task.Factory.StartNew(AddProducts);
            /*创建任务 t2  t2 执行 数据集合添加操作*/
            Task t2 = Task.Factory.StartNew(AddProducts);
            /*创建任务 t3  t3 执行 数据集合添加操作*/
            Task t3 = Task.Factory.StartNew(AddProducts);

            Task.WaitAll(t1, t2, t3);
            swTask.Stop();
            Console.WriteLine("List<Product> 当前数据量为：" + Products.Count);
            Console.WriteLine("List<Product> 执行时间为：" + swTask.ElapsedMilliseconds);

            Thread.Sleep(1000);
            ConcurrenProducts = new ConcurrentStack<Product>();
            Stopwatch swTask1 = new Stopwatch();
            swTask1.Start();

            /*创建任务 tk1  tk1 执行 数据集合添加操作*/
            Task tk1 = Task.Factory.StartNew(AddConcurrenProducts);
            /*创建任务 tk2  tk2 执行 数据集合添加操作*/
            Task tk2 = Task.Factory.StartNew(AddConcurrenProducts);
            /*创建任务 tk3  tk3 执行 数据集合添加操作*/
            Task tk3 = Task.Factory.StartNew(AddConcurrenProducts);

            Task.WaitAll(tk1, tk2, tk3);
            swTask1.Stop();
            Console.WriteLine("ConcurrentStack<Product> 当前数据量为：" + ConcurrenProducts.Count);
            Console.WriteLine("ConcurrentStack<Product> 执行时间为：" + swTask1.ElapsedMilliseconds);
            Console.ReadLine();
        }

        /*执行集合数据添加操作*/
        static void AddProducts()
        {
            Parallel.For(0, 30000, (i) =>
            {
                Product product = new Product();
                product.Name = "name" + i;
                product.Category = "Category" + i;
                product.SellPrice = i;
                lock (O)
                {
                    Products.Push(product);
                }
            });

        }

        /*执行集合数据添加操作*/
        static void AddConcurrenProducts()
        {
            Parallel.For(0, 30000, (i) =>
            {
                Product product = new Product();
                product.Name = "name" + i;
                product.Category = "Category" + i;
                product.SellPrice = i;
                ConcurrenProducts.Push(product);
            });

        }
    }
}

/*
 * 
 * ConcurrentStack  是完全无锁的，能够支持并发的添加元素，后进先出。
 * 
 * List<Product> 当前数据量为：90000
List<Product> 执行时间为：89

ConcurrentStack<Product> 当前数据量为：90000
ConcurrentStack<Product> 执行时间为：98
 */
