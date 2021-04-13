using Client.Entity.FuYouZhiFu;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using WxPayAPI;

namespace Client.JiaJie
{
   public static class MakeSign
    {
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">原文</param>
        /// <returns></returns>
        public static string GetSign(string str)
        {
            var md5s = System.Security.Cryptography.MD5.Create();
            var md5 = System.Security.Cryptography.MD5.Create();
                var bs = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
                var sb = new StringBuilder();
                foreach (byte b in bs)
                {
                    sb.Append(b.ToString("x2"));
                }
                //所有字符转为大写
                return sb.ToString().ToLower();
        }

       

        public static string SignData(string fileName, string password, string strdata, string encoding = "ISO-8859-1")
         {
            X509Certificate2 objx5092;
            if (string.IsNullOrWhiteSpace(password))
            {
                objx5092 = new X509Certificate2(fileName);
            }
            else
            {
                objx5092 = new X509Certificate2(fileName, password);
            }
            RSACryptoServiceProvider rsa = objx5092.PrivateKey as RSACryptoServiceProvider;
             byte[] data = Encoding.GetEncoding(encoding).GetBytes(strdata);
             byte[] hashvalue = rsa.SignData(data, "MD5");//为证书采用MD5withRSA 签名
            return bytesToHexStr(hashvalue);///将签名结果转化为16进制字符串
       }

        private static string bytesToHexStr(byte[] bcd)
         {
            StringBuilder s = new StringBuilder(bcd.Length * 2);
            for (int i = 0; i<bcd.Length; i++)
             {
                 s.Append(bcdLookup[(bcd[i] >> 4) & 0x0f]);
                 s.Append(bcdLookup[bcd[i] & 0x0f]);
             }
             return s.ToString();
         }

        private static char[] bcdLookup = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };

        public static Encoding encoding = Encoding.GetEncoding("GBK");
        public static string SignerSymbol = "MD5withRSA";

    

        public static void md5withRsa(Encoding e, string s)
        {
            encoding = e; SignerSymbol = s;
        }
        private static AsymmetricKeyParameter CreateKEY(bool isPrivate, string key)
        { byte[] keyInfoByte = Convert.FromBase64String(key);
            if (isPrivate)
                return PrivateKeyFactory.CreateKey(keyInfoByte);
            else return PublicKeyFactory.CreateKey(keyInfoByte); } 
       
        public static string Sign(string content, string privatekey)
        { ISigner sig = SignerUtilities.GetSigner(SignerSymbol);
            sig.Init(true, CreateKEY(true, privatekey));
            var bytes = encoding.GetBytes(content);
            sig.BlockUpdate(bytes, 0, bytes.Length);
            byte[] signature = sig.GenerateSignature(); /* Base 64 encode the sig so its 8-bit clean */
            var signedString = Convert.ToBase64String(signature);
            return signedString; }
        /// <summary> /// 验证签名 /// </summary> /// <param name="content">待签名的字符串</param> /// <param name="signData">加密后的文本</param> /// <param name="publickey">公钥文本</param> /// <returns>是否一致</returns> 

        public static bool Verify(string content, string signData, string publickey)
        { ISigner signer = SignerUtilities.GetSigner(SignerSymbol);
            signer.Init(false, CreateKEY(false, publickey));
            var expectedSig = Convert.FromBase64String(signData); /* Get the bytes to be signed from the string */
            var msgBytes = encoding.GetBytes(content); /* Calculate the signature and see if it matches */
            signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
            return signer.VerifySignature(expectedSig);
        }
   

    }
}
