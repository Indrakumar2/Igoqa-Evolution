using System;
using System.Security.Cryptography;
using System.Text;

namespace Evolution.Cryptography
{
    public class KeyGenerator
    {  
        public string GenratePrivateKey(int iKeySize = 256)
        {
            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.KeySize = iKeySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.GenerateIV();
            string ivStr = Convert.ToBase64String(aesEncryption.IV);
            aesEncryption.GenerateKey();
            string keyStr = Convert.ToBase64String(aesEncryption.Key);
            string completeKey = ivStr + "," + keyStr;

            return Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(completeKey));
        }
    }
}
