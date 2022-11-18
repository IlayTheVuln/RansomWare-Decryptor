using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace PocDecryptor
{
   
    static class SystemDecryptor
    {
        private static readonly List<string> AllFileSystem;
        private const string DecryptionDirectory = @"C:\Users\";

        static SystemDecryptor()
        {
            var filePaths = Directory.EnumerateFiles(DecryptionDirectory, "*.PocEncrypted", new EnumerationOptions
            {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            });
            AllFileSystem = new List<string>(filePaths);
        }


        public static void DecryptFileSystem()
        {
            try
            {
                foreach (string FileNmae in AllFileSystem)
                {
                    int x = 0;
                    try
                    {
                        Byte[] RawFileContent = FileHandlingUtilities.GetsRawFileContent(FileNmae);
                        Byte[] Decrypted = AesEncryption.DecryptDataStream(RawFileContent);
                        FileHandlingUtilities.CreateOriginalFileForDecryption(FileNmae, Decrypted);
                        FileHandlingUtilities.DeleteSourceFile(FileNmae);

                    }
                    catch
                    {
                        Console.WriteLine("An iteration failed!");
                        x++;
                        Console.WriteLine($"{x} missing iterations");
                    }
                   

                }

            }
            catch
            {
                Console.WriteLine("File DecryptionProccess Failed!");
            }
            
        }
    }
}
