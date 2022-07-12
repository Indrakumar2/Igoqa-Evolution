using System;
using System.Security.Cryptography;
using System.Text;

namespace Evolution.Cryptography
{
    public class SymmetricCryptography
    {
        public string Encrypt(string textToBeEncrypt, string privateKey, int keySize = 256)
        {
            if (string.IsNullOrEmpty(textToBeEncrypt))
                return string.Empty;

            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.KeySize = keySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            //aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(privateKey)).Split(',')[0]);
            aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(privateKey)).Split(',')[1]);
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(textToBeEncrypt);
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);            
            return Convert.ToBase64String(cipherText);
        }

        public string Decrypt(string encryptedText, string privateKey, int keySize = 256)
        {
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            RijndaelManaged aesEncryption = new RijndaelManaged();
            aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.KeySize = keySize;
            aesEncryption.BlockSize = 128;
            aesEncryption.Mode = CipherMode.CBC;
            //aesEncryption.Padding = PaddingMode.PKCS7;
            aesEncryption.IV = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(privateKey)).Split(',')[0]);
            aesEncryption.Key = Convert.FromBase64String(ASCIIEncoding.UTF8.GetString(Convert.FromBase64String(privateKey)).Split(',')[1]);
            ICryptoTransform decrypto = aesEncryption.CreateDecryptor();
            byte[] encryptedBytes = Convert.FromBase64CharArray(encryptedText.ToCharArray(), 0, encryptedText.Length);
            return ASCIIEncoding.UTF8.GetString(decrypto.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length));
        }
    }
}
