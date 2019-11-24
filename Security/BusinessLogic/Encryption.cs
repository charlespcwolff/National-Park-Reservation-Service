using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Security.BusinessLogic
{
    /// <summary>
    /// Handles encrypting and decrypting data (Only works with .Net Framework)
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// Decodes the passed in RSA encrypted text given the private key path
        /// </summary>
        /// <param name="cipherText">RSA encrypted text</param>
        /// <param name="privateKeyPath">The path to the private key xml file</param>
        /// <returns>The decrypted text</returns>
        public static string DecryptCipherText(string cipherText, string privateKeyPath)
        {
            RSACryptoServiceProvider cipher = new RSACryptoServiceProvider();
            cipher.FromXmlString(System.IO.File.ReadAllText(privateKeyPath));
            byte[] original = cipher.Decrypt(Convert.FromBase64String(cipherText), false);
            return Encoding.UTF8.GetString(original);
        }

        /// <summary>
        /// RSA encrypts the passed in text given the public key path
        /// </summary>
        /// <param name="plaintext">The text to be encrypted</param>
        /// <param name="publicKeyPath">The path to the public key xml file</param>
        /// <returns>The RSA encrypted text</returns>
        public static string GetCipherText(string plaintext, string publicKeyPath)
        {
            RSACryptoServiceProvider cipher = new RSACryptoServiceProvider();
            cipher.FromXmlString(System.IO.File.ReadAllText(publicKeyPath));
            byte[] data = Encoding.UTF8.GetBytes(plaintext);
            byte[] cipherText = cipher.Encrypt(data, false);
            return Convert.ToBase64String(cipherText);
        }

        /// <summary>
        /// Generates a set of public/private key xml files for use in RSA encryption
        /// </summary>
        /// <param name="publicKeyFileName">The full filepath to the public key file to be created</param>
        /// <param name="privateKeyFileName">The full filepath to the private key file to be created</param>
        public static void GenerateKeys(string publicKeyFileName, string privateKeyFileName)
        {
            // Variables
            CspParameters cspParams = null;
            RSACryptoServiceProvider rsaProvider = null;
            StreamWriter publicKeyFile = null;
            StreamWriter privateKeyFile = null;

            string publicKey = "";
            string privateKey = "";

            try
            {
                // Create a new key pair on target CSP
                cspParams = new CspParameters();
                cspParams.ProviderType = 1; // PROV_RSA_FULL
                //cspParams.ProviderName; // CSP name

                cspParams.Flags = CspProviderFlags.UseArchivableKey;
                cspParams.KeyNumber = (int)KeyNumber.Exchange;
                rsaProvider = new RSACryptoServiceProvider(cspParams);

                // Export public key
                publicKey = rsaProvider.ToXmlString(false);

                // Write public key to file 
                publicKeyFile = System.IO.File.CreateText(publicKeyFileName);
                publicKeyFile.Write(publicKey);

                // Export private/public key pair
                privateKey = rsaProvider.ToXmlString(true);

                // Write private/public key pair to file
                privateKeyFile = System.IO.File.CreateText(privateKeyFileName);
                privateKeyFile.Write(privateKey);
            }
            finally
            {
                if (publicKeyFile != null)
                {
                    publicKeyFile.Close();
                }

                if (privateKeyFile != null)
                {
                    privateKeyFile.Close();
                }
            }
        }
    }
}
