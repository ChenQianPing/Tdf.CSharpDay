using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.AlgorithmDay.Chapter001
{
    public class GreedyDemo
    {
        private float _value = 0;
        public void TestMethod1()
        {
            var result = "";

            /* 
             * 编号，重量，价值，单位重量价值
             * 
             * 背包的总容量100
             * 
             * 1， 1， 1， 0.8， 0，
             * 状态，0表示不在，1表示在，0-1表示部分在
             * 背包的总价值为：163
             * 
             * 30+10+20+50*0.8=100（重量）
             * 65+20+30+40*1.2=163（价值）
             * 
             * 放入编号1，编号5，编号4，编号2放到0.8，编号3不放；
             * 
             */
            Goods[] goods = new Goods[5]
            {
                new Goods(1, 30, 65, 2.1),   // 1
                new Goods(2, 50, 60, 1.2),   // 4
                new Goods(3, 40, 40, 1),     // 5
                new Goods(4, 20, 30, 1.5),   // 3
                new Goods(5, 10, 20, 2)      // 2
            };
            

            Goods[] newGoods = new Goods[5];
            float[] state = new float[5];

            // 得到goods按照单位重量价值降序排列的newGoods
            newGoods = Sort(goods);

            // 背包的总容量100
            state = GreedyKnapsack(100, goods);
            for (var i = 1; i <= state.Length; i++)
            {
                result = result + state[i - 1].ToString(CultureInfo.InvariantCulture) + "， ";
            }

            result = result + "\r\n" + "背包的总价值为：" + _value;

            Console.WriteLine(result);
            Console.ReadLine();
        }

        /// <summary>
        /// 排序算法--将单位重量的价值，降序排列,利用堆排序
        /// </summary>
        /// <param name="goods">物品集</param>
        /// <returns>单位重量价值，降序的物品集</returns>
        public Goods[] Sort(Goods[] goods)
        {
            for (var i = goods.Length / 2; i >= 1; i--)
            {
                // 筛选：使得二叉树结构中，每一个父节点，都比孩子节点小
                Sift(goods, i, goods.Length);
            }

            for (var j = goods.Length; j >= 1; j--)
            {
                // 交换两个元素，第一个位置，已是最小的元素
                Goods[] temp = new Goods[1];
                temp[0] = goods[0];       // 取出第一个位置的元素存起来
                goods[0] = goods[j - 1];  // 第一个位置，放最后一个元素
                goods[j - 1] = temp[0];   // 最后一个位置，放入第一个元素对象
                Sift(goods, 1, j - 1);    // 进行一次筛选，使得第一个位置的数最小
            }
            return goods;
        }

        /// <summary>
        /// 堆排序的筛选过程
        /// </summary>
        /// <param name="goods">数组</param>
        /// <param name="k">数组的个数</param>
        /// <param name="n">第i位置（放较小的数）</param>
        public void Sift(Goods[] goods, int k, int n)
        {
            int i = k;
            int j = 2 * i;
            Goods[] tempGoods = new Goods[1];
            tempGoods[0] = goods[k - 1];

            if (j <= n)
            {
                if (j < n && goods[j - 1].Vw > goods[j + 1 - 1].Vw) // 比较子孩子们
                {
                    j++;
                }
                if (goods[k - 1].Vw > goods[j - 1].Vw) // 比较父节点和较小的一个孩子
                {
                    goods[i - 1] = goods[j - 1]; // 在第i个位置放入j个位置放的对象
                    i = j;                       // 子节点变为父节点，递归的意思 
                    j = 2 * i;

                }
            }
            goods[i - 1] = tempGoods[0]; // 之前存起来的数，找到了她的位置

        }

        /// <summary>
        /// 贪心算法
        /// </summary>
        /// <param name="w">背包的总容量</param>
        /// <param name="goods">物品</param>
        /// <returns></returns>
        public float[] GreedyKnapsack(int w, Goods[] goods)
        {
            
            // 状态，0表示不在，1表示在，0-1表示部分在
            float[] state = new float[5];

            int i;

            // 初始化状态
            for (i = 1; i <= goods.Length; i++)
            {
                state[i - 1] = 0;
            }

            for (i = 1; i <= goods.Length; i++)
            {
                if (goods[i - 1].Weight <= w) // 当整个物品可以放下时
                {
                    state[i - 1] = 1;
                    _value = _value + goods[i - 1].Value;
                    w = w - goods[i - 1].Weight;
                }
                else
                {
                    break;   // 当整个物品放不进去，直接跳出整个循环，后面的不再判断
                }
            }

            // 只有部分物品可以放进去
            if (i <= goods.Length)
            {
                state[i - 1] = w / (float)goods[i - 1].Weight;
                _value = _value + goods[i - 1].Value * state[i - 1];
            }

            return state;
        }
    }
}

/*
 * 1， 1， 1， 0.8， 0，
背包的总价值为：163

* 30+10+20+50*0.8=100（重量）
* 65+20+30+40*1.2=163（价值）
* 
* 放入编号1，编号5，编号4，编号2放到0.8，编号3不放；

背包问题

给定n种物品和一个背包。物品i的重量是Wi，其价值为Vi，背包的容量为C。
应如何选择装入背包的物品，使得装入背包中物品的总价值最大？
（假定物品可以分割成更小部分，亦即物品可以部分装入）

例：
有5件物品，背包的容量（C）为100，物品的重量和价值分别如下所示：

编号    1   2  3   4   5
重量Wi  10  20 30  40  50
价值Vi  20  30 66  40  60

这个问题有三种看似合理的选择：
1、每次选择剩余物品中价值最大的；
2、每次选择剩余物品中重量最轻的；
3、每次选择剩余物品中单位重复价值最高的。

用贪心算法解背包问题的基本步骤：

首先计算每种物品单位重重的价值Vi/Wi，
然后，依贪心选择策略，将尽可能多的单位重量价值最高的物品装入背包。
若将这种物品全部装入背包后，
背包内的物品总重量未超过C，
则选择单位重量价值次高的物品并可能多地装入背包。
依此策略一直地进行下去，直到背包装满为止。

背包问题--贪心算法C#Demo解析
概述
      贪心算法（又称贪婪算法）是指，在对问题求解时，总是做出在当前看来是最好的选择。也就是说，不从整体最优上加以考虑，他所做出的是在某种意义上的局部最优解。 
    个人对贪心算法的理解是：贪心是有条件的，我们也常说贪心策略选择，具有一定的时效性。而通常，基于选择的性质，往往贪心算法会做一个排序。
 */
