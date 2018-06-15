using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tdf.CSharpDay.Chapter9
{
    public class FileReader
    {
        /// <summary>
        /// 缓存池
        /// </summary>
        private byte[] Buffer { get; set; }

        /// <summary>
        /// 缓存区大小
        /// </summary>
        public int BufferSize { get; set; }

        public FileReader(int bufferSize)
        {
            this.BufferSize = bufferSize;
            this.Buffer = new byte[BufferSize];
        }

        /// <summary>
        /// 同步读取文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public void SynsReadFile(string path)
        {
            Console.WriteLine("同步读取文件 begin");
            using (var fs = new FileStream(path, FileMode.Open))
            {
                fs.Read(Buffer, 0, BufferSize);
                var output = System.Text.Encoding.UTF8.GetString(Buffer);
                Console.WriteLine("读取的文件信息：{0}", output);
            }

            Console.WriteLine("同步读取文件 end");
        }

        /// <summary>
        /// 异步读取文件
        /// </summary>
        /// <param name="path"></param>
        public void AsynReadFile(string path)
        {
            Console.WriteLine("异步读取文件 begin");

            if (File.Exists(path))
            {
                var fs = new FileStream(path, FileMode.Open);
                fs.BeginRead(Buffer, 0, BufferSize, AsyncReadCallback, fs);
            }
            else
            {
                Console.WriteLine("该文件不存在");
            }

        }

        /// <summary>
        /// AsyncReadCallback
        /// </summary>
        /// <param name="ar"></param>
        void AsyncReadCallback(IAsyncResult ar)
        {
            var stream = ar.AsyncState as FileStream;
            if (stream != null)
            {
                Thread.Sleep(1000);
                // 读取结束
                stream.EndRead(ar);
                stream.Close();

                var output = System.Text.Encoding.UTF8.GetString(this.Buffer);
                Console.WriteLine("读取的文件信息：{0}", output);
            }
        }

    }
}
