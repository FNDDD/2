using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.JiaJieMi
{
    class MD5_L
    {
        private  static String[] hexDigits = { "0", "1", "2", "3", "4", "5",
            "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

        /**
         * 转换字节数组为16进制字串
         * 
         * @param b
         *            字节数组
         * @return 16进制字串
         */
        public static String byteArrayToHexString(byte[] b)
        {
            StringBuilder resultSb = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                resultSb.Append(byteToHexString(b[i]));
            }
            return resultSb.ToString();
        }

        /**
         * J 转换byte到16进制
         * 
         * @param b
         * @return
         */
        private static String byteToHexString(byte b)
        {
            int n = b;
            if (n < 0)
            {
                n = 256 + n;
            }
            int d1 = n / 16;
            int d2 = n % 16;
            return hexDigits[d1] + hexDigits[d2];
        }

        // MessageDigest 为 JDK 提供的加密类
        public static String MD5Encode(String origin)
        {
            String resultString = null;
            try
            {
                //resultString = origin;
                //MessageDigest md = MessageDigest.getInstance("MD5");
                //resultString = byteArrayToHexString(md.digest(resultString
                //        .getBytes("UTF-8")));
            }
            catch (Exception ex)
            {
            }




            //var md5s = System.Security.Cryptography.MD5.Create();
            //var md5 = System.Security.Cryptography.MD5.Create();
            //var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            //var sb = new StringBuilder();
            //foreach (byte b in bs)
            //{
            //    sb.Append(b.ToString("x2"));
            //}
            ////所有字符转为大写
            //return sb.ToString().ToUpper();
            return resultString;
        }
    }
}
