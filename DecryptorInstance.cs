using System;
using System.Collections.Generic;
using System.Text;

namespace PocDecryptor
{
    class DecryptorInstance
    {
        public void StartDecryptorInstance()
        {
            try
            {
                SystemDecryptor.DecryptFileSystem();

            }
            catch
            {
                Console.WriteLine("An Error while decrypting file system! Run the decryptor as admin!");
            }
            
        }
    }
}
