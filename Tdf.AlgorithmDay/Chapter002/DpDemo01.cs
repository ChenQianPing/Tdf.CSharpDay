using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.AlgorithmDay.Chapter002
{
    public class DpDemo01
    {
        public void TestMethod1()
        {

            /*
             * 7,9
             * 10,13
             * 23
             */ 

            int capacity = 16;

            // 物品容量和价值的对应

            int[] weight = new int[5] {3, 4, 7, 8, 9};
            int[] value = new int[5] {4, 5, 10, 11, 13};
            
            int c = DynamicProgram(capacity, 5, weight, value);

            var result = "最大价值为" + c.ToString();// +"\r\n" + "物品编号为：" + "1,2,3,4,5" + "\r\n" + "物品状态为：";

            Console.WriteLine(result);
            Console.ReadLine();
        }

        /// <summary>
        /// 动态规划
        /// </summary>
        /// <param name="w">表示背包的最大容量</param>
        /// <param name="n">表示商品个数</param>
        /// <param name="weigth">表示商品重量数组</param>
        /// <param name="value">表示商品价值数组</param>
        /// <returns></returns>
        public int DynamicProgram(int w, int n, int[] weigth, int[] value)
        {
            // 剩余容量
            int w2;

            // 物品编号i;
            int i;

            // i个物品导致的背包的剩余容量为w的，最优的总价值 c(i,w)
            int[,] c = new int[n + 1, w + 1]; //&#&这里加1是因为：容量包含0   ；  n是个数，个数增加1是因为：个数包含0

            // 没有物品可放时
            for (w2 = 0; w2 <= w; w2++)
            {
                c[0, w2] = 0;
            }

            // 背包中有物品时
            for (i = 1; i <= n; i++)
            {
                c[i, 0] = 0;

                for (w2 = 1; w2 <= w; w2++)
                {
                    if (weigth[i - 1] <= w2) // 当物品在包中，状态:1
                    {
                        c[i, w2] = Max(c[i - 1, w2 - weigth[i - 1]] + value[i - 1], c[i - 1, w2]);
                    }
                    else
                    {
                        c[i, w2] = c[i - 1, w2];
                    }
                }

            }
            return c[n, w];
        }


        // 取最大值函数
        public int Max(int i, int w)
        {
            if (i > w)
            {
                return i;
            }
            else
            {
                return w;
            }

        }
    }
}


/*
 * 最大价值为23
 * 
 * 背包问题，动态规划
 * 
 * 核心思想
 * 1.当没有物品，或没有包时，总价值为0
 * 2.当物品放不进包时，最优总价值和当前物品无关系
 * 3.当物品容量比剩余容量小的时候，考虑放不放两种情况
 * 
 * 动态规划算法
动态规划常被认为是递归的反向技术，
所谓的递归算法是从顶部开始，
把问题向下全部分解为小的问题进行解决，
直到解决整个问题为止。
而动态规划则是从底部开始，解决小的问题同时把它们合并形成大问题的一个完整解决方案。
解决问题的递归算法经常是很优雅的，但是却是很低效的。
尽管可能是优雅的计算机程序，
但是C#语言编译器以及其他语言都不会把递归代码有效翻译成机器代码，并最终导致效率低下。

许多用递归解决的编程问题可以使用动态规划进行重新编写。
动态规划通常会使用缓存对象，将不同的子解决方案存储起来，
将中间运算结果记录下来，从而大大提高了效率。实际上就是使用了空间换时间的办法。
 */

