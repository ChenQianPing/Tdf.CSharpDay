using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter8
{
    public class DynamicDemo02
    {

        public static void TestMethod1()
        {
            var user = new DynamicUser
            {
                Userid = 123,
                UserName = "Lucy"
            };

            user.ConvertToDynamic(user.Userid);
            user.ConvertToDynamic(user.UserName);
            user.TypeCheck();

            Console.WriteLine("Input any key to exit.");
            Console.ReadKey();
        }
    }


}


/*
 * 123
Lucy
Age is Dynamic? True
Age:20
List<dynamic>'s type is System.Collections.Generic.List`1[System.Object]
Input any key to exit.

应用
1.自动反射
2.COM组件互操作
3.混合编程，例如IronRuby和IronPython
4.处理Html DOM对象
5.还有一种应用，数据传输中格式转换，如：对象转json等，很方便
 */
