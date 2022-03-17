using System;
using PgpCore;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BozziPgpCoreDemoApp
{
    class Program
    {
        static string basePath = @"E:\_Dev\BozziPgpCoreDemoApp\DemoFiles\";

        static void Main(string[] args)
        {

            Console.WriteLine("Pgp Core Demo App");
            try
            {
                //GeneraChiavi();

                //Task<string> messaggio;

                //messaggio = Firma();

                //Console.WriteLine(messaggio.Result);

                //messaggio = VerificaInChiaro("openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256.asc",
                //                        "openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256");
                
                //Console.WriteLine(messaggio.Result);

                string messaggio = VerificaChamataEsterna( "openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256.asc",
                                        "openSUSE-Leap-15.3-3-NET-x86_64-Build38.1-Media.iso.sha256");

                //string messaggio = VerificaChamataEsterna("electrum-4.1.5.exe.asc",
                //                        "electrum-4.1.5.exe");
                
                Console.WriteLine(messaggio);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static string VerificaChamataEsterna( string publicKeyFilename, string signedFilename )
        {
            string output;
            using (Process pProcess = new Process())
            {
                pProcess.StartInfo.FileName = @"C:\Program Files (x86)\GnuPG\bin\gpg.exe";
                pProcess.StartInfo.Arguments = "--verify " +
                    basePath + publicKeyFilename + " " +
                    basePath + signedFilename; //argument
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                //pProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                //pProcess.StartInfo.CreateNoWindow = true; //not diplay a windows
                pProcess.Start();
                output = pProcess.StandardOutput.ReadToEnd(); //The output result
                pProcess.WaitForExit();
            }
            return output;
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

        public static async Task<string> Verifica(string publicKeyFilename, string signedFilename)
        {
            // Load keys
            FileInfo publicKey = new FileInfo(basePath + publicKeyFilename);
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            // Reference input
            FileInfo inputFile = new FileInfo(basePath + signedFilename);

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

        public static async Task<string> VerificaStream(string publicKeyFilename, string signatureFilename, string downloadedFilename)
        {
            // Load keys
            EncryptionKeys encryptionKeys;
            using (Stream publicKeyStream = new FileStream(basePath + publicKeyFilename, FileMode.Open))
                encryptionKeys = new EncryptionKeys(publicKeyStream);

            PGP pgp = new PGP(encryptionKeys);

            // Reference input file
            bool verified;
            using (FileStream inputFileStream = new FileStream(basePath + signatureFilename, FileMode.Open))
                // Verify
                verified = await pgp.VerifyStreamAsync(inputFileStream);

            if (verified)
            {
                return "The content is signed with the given public key";
            }
            else
            {
                return "WARNING The content is NOT signed with the given public key";
            }
        }

        public static async Task<string> VerificaInChiaro(string publicKeyFilename, string signedFilename)
        {
            // Load keys
            FileInfo publicKey = new FileInfo(basePath + publicKeyFilename);
            EncryptionKeys encryptionKeys = new EncryptionKeys(publicKey);

            // Reference input
            FileInfo inputFile = new FileInfo(basePath + signedFilename);

            // Verify
            PGP pgp = new PGP(encryptionKeys);
            bool verified = await pgp.VerifyClearFileAsync(inputFile);

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
