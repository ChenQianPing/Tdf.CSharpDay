using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tdf.AlgorithmDay.Chapter001
{
    /// <summary>
    /// 物品类用于存储物品的属性
    /// </summary>
    public class Goods
    {

        /// <summary>
        /// 编号
        /// </summary>
        public int Flag;

        /// <summary>
        /// 重量
        /// </summary>
        public int Weight;

        /// <summary>
        /// 价值
        /// </summary>
        public int Value;

        /// <summary>
        /// 单位重量的价值 = 价值/重量
        /// </summary>
        public double Vw;

        /// <summary>
        /// goods的构造函数
        /// </summary>
        /// <param name="flag">编号</param>
        /// <param name="weight">重量</param>
        /// <param name="value">价值</param>
        /// <param name="vw">单位重量价值</param>
        public Goods(int flag, int weight, int value, double vw)
        {
            this.Flag = flag;
            this.Weight = weight;
            this.Value = value;
            this.Vw = vw;
        }
    }
}
