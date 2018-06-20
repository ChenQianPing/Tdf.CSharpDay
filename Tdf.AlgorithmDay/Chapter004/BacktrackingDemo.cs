using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.AlgorithmDay.Chapter004
{
    public class BacktrackingDemo
    {
        /// <summary>
        /// 用回溯法解决子集和问题
        /// 因为我只需要求出一解，所以我选择了回溯法。
        /// </summary>
        public static void TestMethod1()
        {
            int[] test = {3, 7, 18, 33, 22, 6, 10, 21, 2, 15}; // 测试用的数组
            bool[] flag = {false, false, false, false, false, false, false, false, false, false}; // 标志数组,与测试数组对应,true代表该数在组合中,false则不在
            Bubble(test); // 冒泡,其实不排序也可以
            // int target = Convert.ToInt32(Console.Read()); // 读入的是字符串,要经过Convert.ToInt32()的处理,因为输入的是字符串,其值与字面值不一致

            int target = ReadInt();
            
            Console.WriteLine(target);

            bool result = BackTrace(test, flag, target);

            if (result == true)
            {
                for (var i = 0; i <= test.Length - 1; i++)
                {
                    if (flag[i] == true)
                        Console.Write($"{test[i]}, ");
                }
            }
            else
            {
                Console.Write("no sub sets found!");
            }
            Console.WriteLine();
            
        }

        public static int ReadInt()
        {
            do
            {
                try
                {
                    var number = Convert.ToInt32(Console.ReadLine());
                    return number;
                }
                catch
                {
                    Console.WriteLine("输入有误，重新输入！");
                }
            }
            while (true);
        }

        /// <summary>
        /// 回溯法
        /// </summary>
        /// <param name="a">待搜索的数组</param>
        /// <param name="flag"></param>
        /// <param name="target">给定的和</param>
        /// <returns></returns>
        static bool BackTrace(int[] a, bool[] flag, int target)
        {
            int sum = 0;   // 初始化和
            int index = 0; // 初始化索引
            while (index >= 0) // 从第一个开始找,循环找,与for相比循环更多次
            {
                if (flag[index] == false) // 如果不在组合中,则尝试把它加入
                {
                    sum += a[index];
                    flag[index] = true;

                    if (sum == target)    // 如果加入后组合数与目标数一致,则说明找到组合,如果组合数大于目标数,则将刚才的元素从组合中剔除,如果小于,则什么也不做.同时继续检验下一元素.
                        return true;
                    else if (sum > target)
                    {
                        sum -= a[index];
                        flag[index] = false;
                    }
                    index++; // 继续检验下一元素
                }

                // 如果索引到了最后,还没有找到合适的组合,那么将回溯.一般来说会出现*100011或*1000的情况,即此时flag中的元素应该在某个1之后有若干个0或01组合【先0后1】, [I个数] 1 [J个0][K个1]这样的情况,回溯到1的位置,将其变为0,然后继续往下循环检验.从后面回溯的时候,将1变为0,遇0不变.如果回溯到首位,则说明没有合适的组合存在.

                if (index >= a.Length)
                {
                    while (flag[index - 1] == true) // 如果在组合中，则退出，并往前回溯
                    {
                        flag[index - 1] = false;
                        index--;
                        sum -= a[index];
                        if (index < 1) // 此时index最大为0，但下次循环将出界，已经回溯到最开始了
                            return false;
                    }

                    while (flag[index - 1] == false) // 如果不在组合中，往前回溯
                    {
                        index--;
                        if (index < 1)
                            return false;//此时index最大为0，但下次循环将出界，已经回溯到最开始了
                    }
                    flag[index - 1] = false;
                    sum -= a[index - 1];
                }

            }
            return false;
        }

        static void Bubble(int[] a)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));
            for (var i = 0; i <= a.Length - 2; i++)
                for (var j = i + 1; j <= a.Length - 1; j++)
                {
                    if (a[i] > a[j])
                    {
                        var temp = a[i];
                        a[i] = a[j];
                        a[j] = temp;
                    }
                }

            foreach (var k in a)
                Console.Write($"{k}, ");
            Console.WriteLine();
        }

    }
}
