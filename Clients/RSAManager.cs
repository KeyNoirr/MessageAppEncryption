using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Clients
{
    class RSAManager
    {
        public static RSACryptoServiceProvider rsaCrypto = null;

        private static RSACryptoServiceProvider _rsaPrivate = null;
        private static RSACryptoServiceProvider _rsaPublic = null;


        public static void GenerateNewKeys(int bits)
        {
            CspParameters cspParams = new CspParameters(1);
            rsaCrypto = new RSACryptoServiceProvider(bits, cspParams);

            _rsaPrivate = new RSACryptoServiceProvider();
            _rsaPrivate.FromXmlString(rsaCrypto.ToXmlString(true));

            _rsaPublic = new RSACryptoServiceProvider();
            _rsaPublic.FromXmlString(rsaCrypto.ToXmlString(false));
        }


        public static string EncryptWithPublic(string cleartext)
        {
            byte[] plainbytes = Encoding.Unicode.GetBytes(cleartext);
            byte[] cipherbytes = _rsaPublic.Encrypt(plainbytes, false);
            return Convert.ToBase64String(cipherbytes);
        }

        public static string EncryptWithPrivate(string cleartext)
        {
            byte[] plainbytes = Encoding.Unicode.GetBytes(cleartext);
            byte[] cipherbytes = _rsaPrivate.Encrypt(plainbytes, false);
            return Convert.ToBase64String(cipherbytes);
        }

        /// <summary>
        /// Attempts to decrypt ciphertext with public key.
        /// As this is RSA (asymmetric algo) this method should always fail.
        /// </summary>
        /// <param name="ciphertext">The ciphertext.</param>
        /// <returns></returns>
        public static string DecryptWithPublic(string ciphertext)
        {
            string cleartext = "";

            try
            {
                byte[] cipherbytes = Convert.FromBase64String(ciphertext);
                byte[] plain = _rsaPublic.Decrypt(cipherbytes, false);
                cleartext = System.Text.Encoding.Unicode.GetString(plain);
            }
            catch
            {
                throw;
            }

            return cleartext;
        }

        public static string DecryptWithPrivate(string ciphertext)
        {
            string cleartext = "";

            try
            {
                byte[] cipherbytes = Convert.FromBase64String(ciphertext);
                byte[] plain = _rsaPrivate.Decrypt(cipherbytes, false);
                cleartext = System.Text.Encoding.Unicode.GetString(plain);
            }
            catch
            {
                throw;
            }

            return cleartext;
        }
    }
}
