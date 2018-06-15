using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter11
{
    public class StrPermutationHelper
    {
        public List<string> Strs = new List<string>();

        public static void TestMethod1()
        {
            var lstResult = GetPattern("ABC");

            foreach (var item in lstResult)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($@"记录总数：{lstResult.Count}");
            Console.ReadLine();
        }

        public void Permutation()
        {
            Strs.Clear();

            var str = "ABCD";
            char[] lvChar = str.ToCharArray();
            List<char> list = new List<char>();

            for (int i = 1, len = lvChar.Length; i <= len; i++)
            {
                // Console.WriteLine($@"当前Length：{i}");
                Combine(lvChar, 0, i, list);
            }

            foreach (var item in Strs)
            {
                Console.WriteLine(item);
            }

            Console.WriteLine($@"记录数：{Strs.Count}");
            Console.ReadLine();
        }

        /// <summary>
        /// 输入一个字符串，输出该字符串中字符的所有组合（递归）
        /// </summary>
        /// <param name="vChar"></param>
        /// <param name="vStart"></param>
        /// <param name="vResultLength"></param>
        /// <param name="list"></param>
        private void Combine(char[] vChar, int vStart, int vResultLength, List<char> list)
        {
            var str = "";

            if (vResultLength == 0)
            {
                var lvTempCombine = string.Empty;
                for (var j = 0; j < list.Count; j++)
                    lvTempCombine += list[j].ToString();

                str += lvTempCombine;
                // Console.WriteLine(str);
                Strs.Add(str);
                return;
            }

            if (vStart == vChar.Count())
                return;

            list.Add(vChar[vStart]);
            Combine(vChar, vStart + 1, vResultLength - 1, list);
            list.Remove((char)vChar[vStart]);
            Combine(vChar, vStart + 1, vResultLength, list);
        }

        #region GetPattern
        /// <summary>
        /// 返回一个字符串的字符所有组合
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> GetPattern(string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;
            else if (str.Length == 1)
                return new List<string> {str};

            var result = new List<string>();
            var current = str[0];
            var sbChildren = GetPattern(str.Substring(1));
            foreach (var child in sbChildren)
            {
                for (var i = 0; i <= child.Length; i++)
                    result.Add(child.Insert(i, current.ToString()));
            }
            return result;
        }
        #endregion

    }
}
