using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.AlgorithmDay.Chapter003
{
    public class ExhaustDemo01
    {
        public static void TestMethod1()
        {
            int total = 0, i, j, k;
            for (i = 1; i < 5; i++)
                for (j = 1; j < 5; j++)
                    for (k = 1; k < 5; k++)
                    {
                        if (i != j && i != k && j != k)
                        {
                            total++;
                            Console.Write(i);
                            Console.Write(j);
                            Console.Write(k);
                            Console.Write("\n");
                        }
                    }
            Console.WriteLine("无重复的数总共有：" + total);
            Console.ReadLine();

        }
    }
}


/*
 * 【题目一】：有1、2、3、4个数字，能组成多少个互不相同的而且无重复的数字的三位数？都是多少？
   程序分析：可填在百位、十位、个位的数字都是1、2、3、4，组成所有的排列后在去掉部不满足条件的排列。
 */
