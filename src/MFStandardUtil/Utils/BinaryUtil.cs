using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace MFStandardUtil.Utils
{
    /// <summary>
    /// 二进制工具。
    /// </summary>
    public static class BinaryUtil
    {
        /// <summary>
        /// 序列化对象。
        /// </summary>
        /// <param name="obj">
        /// 待序列化对象。
        /// </param>
        /// <returns>
        /// 对象二进制数据。
        /// </returns>
        public static byte[] SerializeObject(object obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.GetBuffer();
            }
        }

        /// <summary>
        /// 反序列化对象。
        /// </summary>
        /// <typeparam name="TType">
        /// 对象类型。
        /// </typeparam>
        /// <param name="binary">
        /// 对象二进制数据。
        /// </param>
        /// <param name="startIndex">
        /// 二进制数据起始位置。
        /// </param>
        /// <param name="length">
        /// 二进制数据长度。
        /// </param>
        /// <returns>
        /// 已反序列化对象。
        /// </returns>
        public static TType DeserializeObject<TType>(byte[] binary, int startIndex = 0, int length = 0)
        {
            byte[] tmp;
            if (length == 0)
            {
                tmp = binary;
            }
            else
            {
                tmp = new byte[length];
                Array.Copy(binary, startIndex, tmp, 0, length);
            }
            using (var stream = new MemoryStream(tmp))
            {
                var formatter = new BinaryFormatter();
                return (TType)formatter.Deserialize(stream);
            }
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 计算对象SHA1值。
        /// </summary>
        /// <param name="obj">
        /// 待计算对象。
        /// </param>
        /// <returns>
        /// 对象SHA1字符串。
        /// </returns>
        public static string ComputeSHA1(object obj)
        {
            var binary = SerializeObject(obj);

            var sha = new SHA1CryptoServiceProvider();
            // This is one implementation of the abstract class SHA1.
            var hash = sha.ComputeHash(binary);

            var sb = new StringBuilder(hash.Length << 1);

            foreach (var b in hash)
            {
                // can be "x2" if you want lowercase
                sb.Append(b.ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
