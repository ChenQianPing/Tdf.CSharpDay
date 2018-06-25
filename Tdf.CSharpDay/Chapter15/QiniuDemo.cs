using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;

namespace Tdf.CSharpDay.Chapter15
{
    public class QiniuDemo
    {
        public static string AccessKey = "xvRVthH0MQwsW82ndDYEybYUC8IbSWQvGNgmX2y0";
        public static string SecretKey = "jN7BAny3rJssgTbqKzA6l0ZXWs1zxcrHfk_1u7VP";

        /// <summary>
        /// 存储空间名
        /// </summary>
        public static string Bucket = "pingkeke";
        public static string UrlPrefix { get; set; }

        #region 简单上传-上传小文件
        public void UploadFile()
        {
            // 构建配置类
            var config = new Config
            {
                Zone = Zone.ZONE_CN_East
            };

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            var mac = new Mac(AccessKey, SecretKey);
            var bucket = Bucket;
            var saveKey = "1-1.jpg";
            var localFile = @"E:\src\Tdf.S01\Tdf.Resources\Pic\Scan\1-1.jpg";

            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            var putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            putPolicy.Scope = bucket + ":" + saveKey;
            // putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            // putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            var jstr = putPolicy.ToJsonString();
            Console.WriteLine($@"jstr:{jstr}");

            var token = Auth.CreateUploadToken(mac, jstr);
            Console.WriteLine($@"token:{token}");

            var um = new UploadManager(config);
            var result = um.UploadFile(localFile, saveKey, token, null);
            Console.WriteLine($@"result:{result}");

            Console.ReadLine();
        }
        #endregion

        #region 简单上传-上传字节数据
        /// <summary>
        /// 简单上传-上传字节数据
        /// </summary>
        public void UploadData()
        {
            // 构建配置类
            var config = new Config
            {
                Zone = Zone.ZONE_CN_East
            };

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            var mac = new Mac(AccessKey, SecretKey);
            var bucket = Bucket;
            var saveKey = "1-2.jpg";
            byte[] data = System.IO.File.ReadAllBytes(@"E:\src\Tdf.S01\Tdf.Resources\Pic\Scan\1-2.jpg");
            // byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello World!");
            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            var putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            var jstr = putPolicy.ToJsonString();
            var token = Auth.CreateUploadToken(mac, jstr);
            FormUploader fu = new FormUploader(config);
            HttpResult result = fu.UploadData(data, saveKey, token, null);
            Console.WriteLine(result);

            Console.ReadLine();
        }
        #endregion

        #region 数据流上传
        /// <summary>
        /// 上传（来自网络回复的）数据流
        /// </summary>
        public void UploadStream()
        {
            // 构建配置类
            var config = new Config
            {
                Zone = Zone.ZONE_CN_East
            };

            // 生成(上传)凭证时需要使用此Mac
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(AccessKey, SecretKey);
            var bucket = Bucket;
            var saveKey = "1.jpg";

            // 上传策略，参见 
            // https://developer.qiniu.com/kodo/manual/put-policy
            var putPolicy = new PutPolicy();
            // 如果需要设置为"覆盖"上传(如果云端已有同名文件则覆盖)，请使用 SCOPE = "BUCKET:KEY"
            // putPolicy.Scope = bucket + ":" + saveKey;
            putPolicy.Scope = bucket;
            // 上传策略有效期(对应于生成的凭证的有效期)          
            putPolicy.SetExpires(3600);
            // 上传到云端多少天后自动删除该文件，如果不设置（即保持默认默认）则不删除
            putPolicy.DeleteAfterDays = 1;

            // 生成上传凭证，参见
            // https://developer.qiniu.com/kodo/manual/upload-token            
            var jstr = putPolicy.ToJsonString();
            var token = Auth.CreateUploadToken(mac, jstr);
            try
            {
                var url = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
                var wReq = System.Net.WebRequest.Create(url) as System.Net.HttpWebRequest;
                var resp = wReq.GetResponse() as System.Net.HttpWebResponse;
                using (var stream = resp.GetResponseStream())
                {
                    // 请不要使用UploadManager的UploadStream方法，因为此流不支持查找(无法获取Stream.Length)
                    // 请使用FormUploader或者ResumableUploader的UploadStream方法
                    FormUploader fu = new FormUploader(config);
                    var result = fu.UploadStream(stream, "xuejing-001.jpg", token, null);
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadLine();
        }
        #endregion

        #region 下载私有空间中的文件
        /// <summary>
        /// 下载私有空间中的文件
        /// </summary>
        public void DownloadPrivateFile()
        {
            // 这个示例单独使用了一个Settings类，其中包含AccessKey和SecretKey
            // 实际应用中，请自行设置您的AccessKey和SecretKey
            Mac mac = new Mac(AccessKey, SecretKey);
            var rawUrl = "http://pav6nuon9.bkt.clouddn.com";
            var saveFile = "D:\\QFL\\1-1.jpg";
            // 设置下载链接有效期3600秒
            var expireInSeconds = 3600;
            var accUrl = DownloadManager.CreatePrivateUrl(mac, rawUrl, "1-1.jpg", expireInSeconds);
            // 接下来可以使用accUrl来下载文件
            HttpResult result = DownloadManager.Download(accUrl, saveFile);
            Console.WriteLine(result);

            Console.ReadLine();
        }
        #endregion

        #region 下载可公开访问的文件
        /// <summary>
        /// 下载可公开访问的文件
        /// </summary>
        public void DownloadFile()
        {
            // 文件URL
            var rawUrl = "http://img.ivsky.com/img/tupian/pre/201610/09/beifang_shanlin_xuejing-001.jpg";
            // 要保存的文件名，如果已存在则默认覆盖
            var saveFile = "D:\\QFL\\saved-snow.jpg";
            // 可公开访问的url，直接下载
            HttpResult result = DownloadManager.Download(rawUrl, saveFile);
            Console.WriteLine(result);

            Console.ReadLine();
        }
        #endregion

    }
}

/*
 * jstr:{"scope":"pingkeke","deadline":1529913974}
 * 
 * token:xvRVthH0MQwsW82ndDYEybYUC8IbSWQvGNgmX2y0:7h95g9Wgy6IG4D8Q1nLqysjv6H4=:eyJz
Y29wZSI6InBpbmdrZWtlIiwiZGVhZGxpbmUiOjE1Mjk5MTM5NzR9


UploadStream:
code:0
ref-code:0
ref-text:
[2018-06-25 15:38:49.7669] [FormUpload] Error: 此流不支持查找操作。


https://developer.qiniu.com/dora/manual/3683/img-directions-for-use
原图	
http://pav6nuon9.bkt.clouddn.com/1-1.jpg
瘦身后的图片
http://pav6nuon9.bkt.clouddn.com/1-1.jpg?imageslim

等比缩小 25%：
http://pav6nuon9.bkt.clouddn.com/1-1.jpg?imageMogr2/thumbnail/!25p

顺时针旋转 45 度：
http://pav6nuon9.bkt.clouddn.com/1-1.jpg?imageMogr2/rotate/45

高斯模糊 半径为 3，Sigma 值为 5：
http://pav6nuon9.bkt.clouddn.com/1-1.jpg?imageMogr2/blur/3x5

把一张图片变成圆角
http://pav6nuon9.bkt.clouddn.com/1-1.jpg?roundPic/radius/50


 */
