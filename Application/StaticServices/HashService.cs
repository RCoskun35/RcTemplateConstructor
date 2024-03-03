using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.StaticServices
{
    public class HashService
    {
        private readonly byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        private const int KeySize = 256;
        private string _key = "BuBirOrnekAnahtar123";
        public HashService(object key)
        {
            try
            {
                var a = ((System.Reflection.Assembly)key);

                if (a.ManifestModule.Name != "ef.dll")
                {
                    string assmbly = GetAssemblyKeyName(a);
                    if (assmbly != "rctemplate")
                        throw new Exception("");
                }
            }
            catch (Exception)
            {
                throw new Exception("illegalKullanimIpMacVeDigerBilgilerinizGuvenlikAmaclıYoneticiFirmayaGonderilmistir");
            }
        }

        public string Encrypt(string toEncrypt, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = this._key;
            }

            byte[] encryptedBytes;
            byte[] iv;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = 128;
                aes.Key = AdjustKeySize(key, KeySize);
                aes.GenerateIV();
                iv = aes.IV;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] plainBytes = Encoding.UTF8.GetBytes(toEncrypt);
                        cs.Write(plainBytes, 0, plainBytes.Length);
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
            Buffer.BlockCopy(iv, 0, combinedBytes, 0, iv.Length);
            Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

            return Convert.ToBase64String(combinedBytes);
        }
        public byte[] AdjustKeySize(string key, int keySize)
        {
            byte[] keyBytes = new byte[keySize / 8];

            int minSize = Math.Min(key.Length, keyBytes.Length);
            Array.Copy(Encoding.UTF8.GetBytes(key), keyBytes, minSize);

            return keyBytes;
        }
        public string Decrypt(string cipherString, string key = null)
        {
            if (string.IsNullOrEmpty(key))
            {
                key = this._key;
            }

            byte[] combinedBytes = Convert.FromBase64String(cipherString);
            byte[] iv = new byte[16];
            byte[] encryptedBytes = new byte[combinedBytes.Length - 16];

            Buffer.BlockCopy(combinedBytes, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

            byte[] decryptedBytes;

            using (Aes aes = Aes.Create())
            {
                aes.KeySize = KeySize;
                aes.BlockSize = 128;
                aes.Key = AdjustKeySize(key, KeySize);
                aes.IV = iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (MemoryStream output = new MemoryStream())
                        {
                            int bytesRead;
                            byte[] buffer = new byte[1024];

                            while ((bytesRead = cs.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                output.Write(buffer, 0, bytesRead);
                            }

                            decryptedBytes = output.ToArray();
                        }
                    }
                }
            }

            return Encoding.UTF8.GetString(decryptedBytes);
        }

        private string GetAssemblyKeyName(System.Reflection.Assembly asmbly)
        {
            System.Reflection.Assembly asm = asmbly;
            object[] obj = asm.GetCustomAttributes(false);
            foreach (object o in obj)
            {
                if (o.GetType() == typeof(System.Reflection.AssemblyTrademarkAttribute))
                {
                    System.Reflection.AssemblyTrademarkAttribute aca = (System.Reflection.AssemblyTrademarkAttribute)o;
                    return aca.Trademark;
                }
            }

            return string.Empty;
        }
    }
}
