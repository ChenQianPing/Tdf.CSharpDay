using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.AlgorithmDay.Chapter002
{
    public class DpDemo02
    {
        private int _numCalls = 0;
        private int _n = 23;

        public void TestMethod1()
        {
            var res = Fib(_n);

            Console.WriteLine($"Fib_{_n}={res},调用次数为 {_numCalls}");
            // Console.ReadKey();
        }

        public void TestMethod2()
        {
            var res = FibL(_n);

            Console.WriteLine($"Fib_{_n}={res},调用次数为 {_numCalls}");
            Console.ReadKey();
        }

        /// <summary>
        /// 递归算法求斐波那契数列
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int Fib(int n)
        {
            _numCalls++;
            // Console.WriteLine($"Fib调用 {n}");
            if (n <= 1)
            {
                return 1;
            }
            else
            {
                return Fib(n - 1) + Fib(n - 2);
            }
        }


        private int FastFib(int n, Dictionary<int, int> memo)
        {
            if (memo == null) throw new ArgumentNullException(nameof(memo));

            _numCalls++;
            // Console.WriteLine($"Fib调用 {n}");
            if (!memo.ContainsKey(n))
            {
                memo.Add(n, FastFib(n - 1, memo) + FastFib(n - 2, memo));
            }
            return memo[n];

        }

        /// <summary>
        /// 动态规划求斐波那契数列
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        private int FibL(int n)
        {
            var memo = new Dictionary<int, int> {{0, 1}, {1, 1}};
            return FastFib(n, memo);
        }


    }
}


/*
 * 斐波那契数列指的是这样一个数列 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233，377，610，987，1597，2584，4181，6765，10946，17711，28657，46368........


这个数列从第3项开始，每一项都等于前两项之和。

    许多用递归解决的编程问题可以使用动态规划进行重新编写。
动态规划通常会使用缓存对象，将不同的子解决方案存储起来，
将中间运算结果记录下来，从而大大提高了效率。实际上就是使用了空间换时间的办法。

 */
