using System;
using PgpCore;
using System.IO;
using System.Threading.Tasks;

namespace BozziPgpCoreDemoApp
{
    class Program
    {
        static string basePath = @"E:\_Dev\BozziPgpCoreDemoApp\DemoFiles\";

        static void Main(string[] args)
        {

            Console.WriteLine("Pgp Core Demo App");

            GeneraChiavi();

            Task<string> messaggio;

            messaggio = Firma();

            Console.WriteLine(messaggio.Result);

            messaggio = Verifica();

            Console.WriteLine(messaggio.Result);

        }

        public static void GeneraChiavi()
        {
            using (PGP pgp = new PGP())
            {
                // Generate keys
                pgp.GenerateKey(basePath + "public.asc", basePath + "private.asc", "email@email.com", "password");
            }
            Console.WriteLine("Keys successfully generated");
        }

        public static async Task<string> Firma()
        {
            // Load keys
            FileInfo privateKey = new FileInfo(basePath + "private.asc");
            EncryptionKeys encryptionKeys = new EncryptionKeys(privateKey, "password");

            // Reference input/output files
            FileInfo inputFile = new FileInfo(basePath + "just-a-file.txt");
            FileInfo signedFile = new FileInfo(basePath + "just-a-signed-file.pgp");

            // Sign
            PGP pgp = new PGP(encryptionKeys);
            await pgp.SignFileAsync(inputFile, signedFile);

            return "File successfully signed";

        }

        public static async Task<string> Verifica()
        {
            // Load keys
            FileInfo publicKey = new FileInfo(basePath + "public.asc");
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            // Reference input
            FileInfo inputFile = new FileInfo(basePath + "just-a-signed-file.pgp");

            // Verify
            PGP pgp = new PGP(encryptionKeys);
            bool verified = await pgp.VerifyFileAsync(inputFile);

            if (verified)
            {
                return "The content is signed with the given public key";
            }
            else
            {
                return "WARNING The content is NOT signed with the given public key";
            }
        }
    }
}
