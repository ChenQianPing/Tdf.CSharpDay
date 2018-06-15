using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter10
{
    public class TomAndJerryDemo
    {
        public static void TestMethod1()
        {
            var c = new Cat("Tom");
            var m1 = new Mouse(c, "Jerry1");
            var m2 = new Mouse(c, "Jerry2");
            c.Call();

            Console.ReadKey();
        }
    }

    public class Cat
    {
        private readonly string _name;
        public Cat(string name)
        {
            _name = name;
        }

        public delegate void CatCallEventHandler();  // 猫叫的委托  
        public event CatCallEventHandler Catevent;   // 猫叫事件  
        public void Call()
        {
            Console.WriteLine($"猫{_name}叫：喵，喵，喵");
            Catevent?.Invoke();
        }
    }

    public class Mouse
    {
        private readonly string _name;
        public Mouse(Cat c, string name)
        {
            this._name = name;
            c.Catevent += new Cat.CatCallEventHandler(this.Run);  // 注册事件    
        }

        public void Run()
        {
            Console.WriteLine($"老鼠{_name}开始逃跑");
        }
    }
}
