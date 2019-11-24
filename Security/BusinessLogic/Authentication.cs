using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Security.BusinessLogic
{
    /// <summary>
    /// Handles salting and hashing for a password
    /// </summary>
    public class Authentication
    {
        /// <summary>
        /// Determines how long it takes to validate a password
        /// </summary>
        private const int WorkFactor = 200;

        /// <summary>
        /// Determines the size of the salt used to hash the password
        /// </summary>
        private const int SaltSize = 16;

        /// <summary>
        /// Holds the salt value used in hashing the password
        /// </summary>
        public string Salt { get; private set; }

        /// <summary>
        /// Holds the password hash value
        /// </summary>
        public string Hash { get; private set; }

        /// <summary>
        /// Use when Registering a new user
        /// </summary>
        /// <param name="password">The password entered by the user</param>
        public Authentication(string password)
        {
            Salt = GenerateSalt(password, SaltSize, WorkFactor);
            Hash = GenerateHash(password, Salt, WorkFactor);
        }

        /// <summary>
        /// Use this when verifying an existing user
        /// </summary>
        /// <param name="password">The password entered by the user</param>
        /// <param name="salt">The salt used to create the original hash</param>
        public Authentication(string password, string salt)
        {
            Salt = salt;
            Hash = GenerateHash(password, salt, WorkFactor);
        }

        /// <summary>
        /// Verifies if the passed in hash matches the stored hash property
        /// </summary>
        /// <param name="hash">The hash to be verified</param>
        /// <returns>True if the passed in hash and the stored hash are the same</returns>
        public bool Verify(string hash)
        {
            return Hash == hash;
        }

        /// <summary>
        /// Generates a random salt value
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <returns>Salt string</returns>
        private static string GenerateSalt(string password, int saltSize, int workFactor)
        {
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, saltSize, workFactor);
            return Convert.ToBase64String(rfc.Salt);
        }

        /// <summary>
        /// Generates the hash for the
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Hash for the passed in password, salt, and work factor</returns>
        private static string GenerateHash(string password, string salt, int workFactor)
        {
            Rfc2898DeriveBytes rfc = HashPasswordWithPBKDF2(password, salt, workFactor);
            return Convert.ToBase64String(rfc.GetBytes(20));
        }

        /// <summary>
        /// Generates the RFC object given the password, salt, and work factor
        /// </summary>
        /// <param name="password">The password to be hashed</param>
        /// <param name="salt">The salt to use in the hashing process</param>
        /// <param name="workFactor">The work factor needed for the hashing process</param>
        /// <returns>RFC object</returns>
        private static Rfc2898DeriveBytes HashPasswordWithPBKDF2(string password, string salt, int workFactor)
        {
            // Creates the crypto service provider and provides the salt - usually used to check for a password match
            Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), workFactor);

            return rfc2898DeriveBytes;
        }
    }
}
