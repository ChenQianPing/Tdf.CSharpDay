using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter14
{
    public class AsyncDemo
    {
        public static void TestMethod1()
        {
            // 同步方式
            Console.WriteLine("同步方式测试开始！");
            SyncMethod(0);
            Console.WriteLine("同步方式结束！");
            Console.ReadKey();
        }

        public static void TestMethod2()
        {
            // 异步方式
            Console.WriteLine("\n异步方式测试开始！");
            AsyncMethod(0);
            Console.WriteLine("异步方式结束！");
            Console.ReadKey();
        }

        #region 同步操作
        /// <summary>
        /// 同步操作
        /// </summary>
        /// <param name="input"></param>
        private static void SyncMethod(int input)
        {
            Console.WriteLine("进入同步操作！");
            var result = SyancWork(input);
            Console.WriteLine($"最终结果{result}");
            Console.WriteLine("退出同步操作！");
        }
        #endregion

        #region 模拟耗时操作（同步方法）
        /// <summary>
        /// 模拟耗时操作（同步方法）
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static int SyancWork(int val)
        {
            for (var i = 0; i < 5; ++i)
            {
                Console.WriteLine($"耗时操作{i}");
                Thread.Sleep(100);
                val++;
            }
            return val;
        }
        #endregion

        #region 异步操作
        /// <summary>
        /// 异步操作
        /// </summary>
        /// <param name="input"></param>
        private static async void AsyncMethod(int input)
        {
            Console.WriteLine("进入异步操作！");
            var result = await AsyncWork(input);
            Console.WriteLine($"最终结果{result}");
            Console.WriteLine("退出异步操作！");
        }
        #endregion

        #region 模拟耗时操作（异步方法）
        /// <summary>
        /// 模拟耗时操作（异步方法）
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private static async Task<int> AsyncWork(int val)
        {
            for (var i = 0; i < 5; ++i)
            {
                Console.WriteLine($"耗时操作{i}");
                await Task.Delay(100);
                val++;
            }
            return val;
        }
        #endregion

    }
}


/*
 * 同步方式测试开始！
进入同步操作！
耗时操作0
耗时操作1
耗时操作2
耗时操作3
耗时操作4
最终结果5
退出同步操作！
同步方式结束！

异步方式测试开始！
进入异步操作！
耗时操作0
异步方式结束！
耗时操作1
耗时操作2
耗时操作3
耗时操作4
最终结果5
退出异步操作！
 */
