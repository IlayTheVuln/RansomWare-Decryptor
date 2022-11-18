using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace PocDecryptor
{
    static class AesEncryption   //aes encryption utilities
    {
        private static readonly Byte[] EncryptionSymmetricKey;  //aes public key

        private static readonly Byte[] AesInitializationVector;   //aes initalization vector

        private const int DefaultAesKeySize = 256;              //aes key size


        static AesEncryption()
        {                                                //sets key and iv for th aes toolkit
            EncryptionSymmetricKey = AsymmetricProtection.GetAesDecryptedKey();
            AesInitializationVector = AsymmetricProtection.GetAesInitializationVector();
           

        }
        public static int GetKeySize()
        {
            try
            {
                return DefaultAesKeySize;
            }
            catch
            {
                Console.WriteLine("An error while getting default key size");
                return 0;
            }

        }


        public static Tuple<Byte[], Byte[]> GetEncryptionInfo() //gets a tuple with the key info
        {
            try
            {
                return new Tuple<Byte[], Byte[]>(EncryptionSymmetricKey, AesInitializationVector);

            }
            catch
            {
                Console.WriteLine("An error while getting symmetric info");
                return null;
            }

        }


        public static string ConvertByteToString(Byte[] array)
        {
            try
            {
                return Encoding.UTF8.GetString(array);

            }
            catch
            {
                Console.WriteLine("An error while convertig byte array to string");
                return null;
            }

        }


        public static Byte[] ConvertStringToByte(string str)
        {
            try
            {
                return Encoding.UTF8.GetBytes(str);

            }
            catch
            {
                Console.WriteLine("An error while converting string to Byte Array");
                return null;
            }

        }


        public static Byte[] EncryptDataStream(byte[] Data)
        {
            try
            {
                byte[] encrypted;
                using (Aes aes = Aes.Create())
                {
                    //setting parameters
                    aes.Key = EncryptionSymmetricKey;
                    aes.IV = AesInitializationVector;
                    aes.Padding = PaddingMode.Zeros;
                    //setting memory and cryptogrphic objects
                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                    using MemoryStream MemoryStreamEncrypt = new MemoryStream();

                    using CryptoStream CryptographicStreamEncrypt = new CryptoStream(MemoryStreamEncrypt, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter StreaWriterEncrypt = new StreamWriter(CryptographicStreamEncrypt))
                    {
                        //encrypting
                        StreaWriterEncrypt.Write(AesEncryption.ConvertByteToString(Data));

                    }

                    //the encrypted data in Byte Array
                    encrypted = MemoryStreamEncrypt.ToArray();
                }

                return encrypted;



            }
            catch
            {
                Console.WriteLine("An error while encrypting a data stream");
                return null;
            }


        }


        public static Byte[] GetEncryptionPublicKey()//for the "AsymmetricProtection" 
        {
            try
            {
                return EncryptionSymmetricKey;
            }
            catch
            {
                Console.WriteLine("An Error while getting symmetric key");
                return null;
            }
        }






        public static Byte[] DecryptDataStream(Byte[] Data)
        {
            try
            {
                using (Aes aes = Aes.Create())
                {   //setting arameters
                    string DecryptedData;
                    aes.Padding = PaddingMode.Zeros;
                    //setting cryptographic objects
                    ICryptoTransform Decryptor = aes.CreateDecryptor(EncryptionSymmetricKey, AesInitializationVector);
                    using MemoryStream MemoryStreamDecrypt = new MemoryStream(Data);
                    using CryptoStream CryptographicStreamDecrypt = new CryptoStream(MemoryStreamDecrypt, Decryptor, CryptoStreamMode.Read);

                    using (StreamReader DecryptionStreamReader = new StreamReader(CryptographicStreamDecrypt))
                    {
                        //Decrypting
                        DecryptedData = DecryptionStreamReader.ReadToEnd();

                    }
                    //The Decrypted Data in Byte Array
                    return ConvertStringToByte(DecryptedData);
                }
            }
            catch
            {
                Console.WriteLine("Error while decrypting DataStream");
                return null;
            }

        }

    }



}






