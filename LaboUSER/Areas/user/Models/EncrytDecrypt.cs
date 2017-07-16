using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LaboUSER.Areas.user.Models;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;

namespace LaboUSER.Areas.user.Models
{
    public class EncrytDecrypt
    {
        public static string passwordEncrypt(string inText, bool flag)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            byte[] clearBytes = Encoding.Unicode.GetBytes(inText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    inText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return inText;
        }
        //Decrypting a string
        public static string passwordDecrypt(string cryptTxt, bool flag)
        {
            string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            cryptTxt = cryptTxt.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cryptTxt);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cryptTxt = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cryptTxt;
        }
    }
}