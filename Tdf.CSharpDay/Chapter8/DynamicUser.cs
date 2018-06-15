using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter8
{
    public class DynamicUser
    {
        /// <summary>
        /// 字段
        /// </summary>
        public dynamic Userid;

        /// <summary>
        /// 属性
        /// </summary>
        public dynamic UserName { get; set; }

        /// <summary>
        /// 玩游戏
        /// （dynamic可以作为参数、返回值等）
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public dynamic Play(dynamic game)
        {
            dynamic defaultGame = "Play Basketball.";

            dynamic secGame = "Play with mud.";

            if (game is int)
            {
                return defaultGame;
            }
            else
            {
                return secGame;
            }
        }

        /// <summary>
        /// 显式类型转换
        /// </summary>
        public void ConvertToDynamic(object obj)
        {
            dynamic d;
            d = (dynamic)obj;
            Console.WriteLine(d);
        }

        /// <summary>
        /// 类型判定
        /// (dynamic 可以使用is、as、typeof)
        /// </summary>
        public void TypeCheck()
        {
            var age = 20;
            Console.WriteLine($"Age is Dynamic? {true}");

            dynamic d = age as dynamic;
            Console.WriteLine("Age:{0}", d);

            Console.WriteLine($"List<dynamic>'s type is {typeof(List<dynamic>)}");
        }

    }
}
