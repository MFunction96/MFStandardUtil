using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace MFStandardUtil.Utils
{
    /// <summary>
    /// 文件工具。
    /// </summary>
    public static class FileUtil
    {
        /// <summary>
        /// 将对象二进制数据导出至文件。
        /// </summary>
        /// <param name="obj">
        /// 待导出对象。
        /// </param>
        /// <param name="filePath">
        /// 文件路径。
        /// </param>
        /// <param name="append">
        /// 是否追加到文件末尾。若文件不存在则创建文件。
        /// </param>
        public static async Task ExportStream(object obj, string filePath, bool append = false)
        {
            var file_info = new FileInfo(filePath);
            if (file_info.Directory != null && !file_info.Directory.Exists) file_info.Directory?.Create();
            
            if (append)
            {
                using (var fs = new FileStream(filePath, FileMode.Append))
                {
                    var bin = BinaryUtil.SerializeObject(obj);
                    await fs.WriteAsync(bin, 0, bin.Length);
                }
            }
            else
            {
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    var bin = BinaryUtil.SerializeObject(obj);
                    await fs.WriteAsync(bin, 0, bin.Length);
                }
            }
        }
        /// <summary>
        /// 从文件中导入对象二进制数据。
        /// </summary>
        /// <param name="filePath">
        /// 文件路径。
        /// </param>
        /// <returns>
        /// 对象二进制数据
        /// </returns>
        public static async Task<byte[]> ImportStream(string filePath)
        {
            using (var fs = new FileStream(filePath, FileMode.Open))
            {
                var buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, buffer.Length);
                return buffer;
            }
        }
        /// <summary>
        /// 将对象数据导出至Json文件。
        /// </summary>
        /// <param name="obj">
        /// 待导出对象。
        /// </param>
        /// <param name="filePath">
        /// 文件路径。
        /// </param>
        /// <param name="append">
        /// 是否追加至文件末尾。
        /// </param>
        /// <param name="formatString">
        /// Json文件格式化字符串。
        /// </param>
        public static void ExportJson(object obj, string filePath, bool append = false, string formatString = ",\r\n")
        {
            var file_info = new FileInfo(filePath);
            if (file_info.Directory != null && !file_info.Directory.Exists) file_info.Directory?.Create();

            if (obj is null)
            {
                File.WriteAllText(filePath, string.Empty);
                return;
            }

            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);
            if (append)
            {
                if (File.Exists(filePath)) json = formatString + json;
                File.AppendAllText(filePath, json);
            }
            else File.WriteAllText(filePath, json);
        }
        /// <summary>
        /// 从Json文件导入对象。
        /// </summary>
        /// <typeparam name="T">
        /// 对象类型。
        /// </typeparam>
        /// <param name="filePath">
        /// 文件路径。
        /// </param>
        /// <returns>
        /// 导入的对象。
        /// </returns>
        public static T ImportJson<T>(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
