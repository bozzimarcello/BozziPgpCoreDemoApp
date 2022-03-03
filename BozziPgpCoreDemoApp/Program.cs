using System;
using PgpCore;
using System.IO;

namespace BozziPgpCoreDemoApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Pgp Core Demo App");

            Verifica();

        }

        public static async void Verifica()
        {
            // Load keys
            FileInfo publicKey = new FileInfo(@"C:\TEMP\Keys\public.asc");
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            // Reference input
            FileInfo inputFile = new FileInfo(@"C:\TEMP\Content\signedContent.pgp");

            // Verify
            PGP pgp = new PGP(encryptionKeys);
            bool verified = await pgp.VerifyFileAsync(inputFile);

            if (verified)
            {
                Console.WriteLine("The content is signed with the given public key");
            }
            else
            {
                Console.WriteLine("The content is NOT signed with the given public key");
            }
        }
    }
}
