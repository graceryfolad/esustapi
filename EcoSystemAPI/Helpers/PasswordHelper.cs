
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Helpers
{
    public class Activation
    {
        /// <summary>
        /// Public property to access EncryptionKey
        /// </summary>
        public byte[] EncryptionKey { get; set; }

        /// <summary>
        /// Public property to access EncryptionIV
        /// </summary>
        public byte[] EncryptionIV { get; set; }
    }

    public class PasswordHelper
    {
        #region Consts

        /// <summary>
        /// Change the Inputkey GUID when you use this code in your own program.
        /// Keep this inputkey very safe and prevent someone from decoding it some way!!
        /// </summary>
        internal const string inputKey = "560A18CD-7346-4CF0-A2E8-671F9B0B9EA9";

        #endregion Consts

        /// <summary>
        /// Method to create password hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Create(string password, string value)
        {
            var valueBytes = KeyDerivation.Pbkdf2(
                                password: password,
                                salt: Encoding.UTF8.GetBytes(value),
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 10000,
                                numBytesRequested: 256 / 8);

            return Convert.ToBase64String(valueBytes);
        }

        /// <summary>
        /// Method to validate password
        /// </summary>
        /// <param name="value"></param>
        /// <param name="salt"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        public static bool Validate(string value, string salt, string hash)
            => Create(value, salt) == hash;

        /// <summary>
        /// Method to create password salt
        /// </summary>
        /// <returns></returns>
        public static string CreateSalt()
        {
            byte[] randomBytes = new byte[128 / 8];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        /// <summary>
        /// Encrypt the given text and give the byte array back as a BASE64 string using AES
        /// </summary>
        /// <param name="plainText" />The plain text to encrypt
        /// <param name="Key" />The pasword salt
        /// <param name="IV" />The pasword salt
        /// <returns>The encrypted text</returns>
        public static string Encrypt(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using MemoryStream msEncrypt = new MemoryStream();
                using CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                encrypted = msEncrypt.ToArray();
            }

            return Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }

        /// <summary>
        /// Decrypts the given text
        /// </summary>
        /// <param name="cipherText" />The encrypted BASE64 text
        /// <param name="Key" />The pasword salt
        /// <param name="IV" />The pasword salt
        /// <returns>The decrypted text</returns>
        public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Aes object
            // with the specified key and IV.
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using MemoryStream msDecrypt = new MemoryStream(cipherText);
                using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using StreamReader srDecrypt = new StreamReader(csDecrypt);
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }

            // Return the decrypted bytes from the memory stream.
            return plaintext;
        }

        /// <summary>
        /// Checks if a string is base64 encoded
        /// </summary>
        /// <param name="base64String" />The base64 encoded string
        /// <returns>Base64 encoded string</returns>
        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);
        }

        /// <summary>
        /// Get encryption parameters
        /// </summary>
        /// <returns>Encryption parameters</returns>
        public static Activation GetActivationDetails()
        {
            // Create a new instance of the Aes
            // class.  This generates a new key and initialization
            // vector (IV).
            Aes myAes = Aes.Create();

            // Get encryption key
            Activation activationDetailEntity = new Activation()
            {
                EncryptionKey = myAes.Key,
                EncryptionIV = myAes.IV
            };
            return activationDetailEntity;
        }

        private static readonly Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomNumbers(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GenerateSecretKey()
        {
            var crypto = new TripleDESCryptoServiceProvider
            {
                KeySize = 192
            };
            crypto.GenerateKey();
            return string.Join("", crypto.Key.Select(x => x.ToString("X2")));
        }

        public static string HashSecretkey(params string[] values)
        {
            var s = values.Aggregate(string.Empty, (current, t) => current + t);
            var hash = new System.Text.StringBuilder();
            var crypto = SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(s), 0, Encoding.UTF8.GetByteCount(s));
            foreach (byte b in crypto)
            {
                hash.Append(b.ToString("x2"));
            }
            //crypto.ForEach(x => hash.Append(x.ToString("x2")));
            return hash.ToString();
        }

        public static string EncryptString(string text, string keyString)
        {
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            using var msEncrypt = new MemoryStream(key);
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(text);
            }

            var iv = aesAlg.IV;

            var decryptedContent = msEncrypt.ToArray();

            var result = new byte[iv.Length + decryptedContent.Length];

            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

            return Convert.ToBase64String(result);
        }

        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            string result;
            using (var msDecrypt = new MemoryStream(cipher))
            {
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                result = srDecrypt.ReadToEnd();
            }

            return result;
        }

        public static string DecryptString2(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[fullCipher.Length - iv.Length]; //changes here

            //Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            //// Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, cipher.Length);
            //Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length); // changes here
            var key = Encoding.UTF8.GetBytes(keyString);

            using var aesAlg = Aes.Create();
            using var decryptor = aesAlg.CreateDecryptor(key, iv);
            string result;


            using MemoryStream input = new(cipher);

           // var msDecrypt = new MemoryStream(cipher);
            
                using var csDecrypt = new CryptoStream(input, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                result = srDecrypt.ReadToEnd();
            

            return result;
        }

        

        public static string ValidateToken(string token)
        {
            if (token == null)
                return null;

            string jwtkey = ConfigHelper.getAppSetting();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtkey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                string userId = jwtToken.Claims.First(x => x.Type == "ClientID").Value;

                // return user id from JWT token if validation successful
                return userId;
            }
            catch
            {
                // return null if validation fails
                return null;
            }
        }

        public static string Mask(string value)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;
            int startIndex = 4;
            var result = value;
            string mask = "********";
            var starLength = mask.Length;

            if (value.Length < startIndex) return result;

            result = value.Insert(startIndex, mask);

            result = result.Length >= (startIndex + (starLength * 2)) ? result.Remove(startIndex + starLength, starLength) : result.Remove(startIndex + starLength, result.Length - (startIndex + starLength));

            return result;
        }

       public  static string EncryptText(string textToEncrypt, string secretkey)
        {
            try
            {
                //string textToEncrypt = "WaterWorld";
                string ToReturn = "";
                string publickey = "12345678";
               // string secretkey = "87654321";

                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte),
                                          CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }

        public static string Decrypt(string textToDecrypt)
        {
            //try
            //{
            //   // string textToDecrypt = "6+PXxVWlBqcUnIdqsMyUHA==";
            //    string ToReturn = "";
            //    string publickey = "12345678";
            //    string secretkey = "87654321";
            //    byte[] privatekeyByte = { };
            //    privatekeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
            //    byte[] publickeybyte = { };
            //    publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
            //    MemoryStream ms = null;
            //    CryptoStream cs = null;
            //    byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
            //    inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
            //    using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            //    {
            //        ms = new MemoryStream();
            //        cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte),
            //                              CryptoStreamMode.Write);
            //        cs.Write(inputbyteArray, 0, inputbyteArray.Length);
            //       cs.FlushFinalBlock();
            //        Encoding encoding = Encoding.UTF8;
            //        ToReturn = encoding.GetString(ms.ToArray());
            //    }
            //    return ToReturn;
            //}
            //catch (Exception ae)
            //{
            //    throw new Exception(ae.Message, ae.InnerException);
            //}
            return string.Empty;
        }
    }

    public static class Securetext
    {
       // private readonly static string key = "zdf3kdf820nvk94dJsd7nsl29kslqTx8"; //"ashproghelpdotnetmania2022key123";
        private readonly static string SecretKey = "zdf3kdf820nvk94dJsd7nsl29kslqTx8"; //Length must be 32 (256 bit)
        private readonly static string InitializationVector = "h82hmD9tmwNn2Xh4";      //Length must be 16 (128 bit)
        public static byte[] Encrypt(string plaintext, byte[] key)
        {

            using (var desObj = Rijndael.Create())
            {
                desObj.Key = key;
                desObj.Mode = CipherMode.CFB;
                desObj.Padding = PaddingMode.PKCS7;
                using (var ms = new MemoryStream())
                {
                    //Append the random IV that was generated to the front of the stream.
                    ms.Write(desObj.IV, 0, desObj.IV.Length);

                    //Write the bytes to be encrypted.
                    using (CryptoStream cs = new CryptoStream(ms, desObj.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        var plainTextBytes = Encoding.UTF8.GetBytes(plaintext);
                        cs.Write(plainTextBytes, 0, plainTextBytes.Length);
                    }
                    return ms.ToArray();
                }
            }
        }

        public static string Decrypt(byte[] cyphertext, byte[] key)
        {

            using (MemoryStream ms = new MemoryStream(cyphertext))
            using (var desObj = Rijndael.Create())
            {
                desObj.Key = key;
                desObj.Mode = CipherMode.CFB;
                desObj.Padding = PaddingMode.PKCS7;

                //Read the IV from the front of the stream and assign it to our object.
                var iv = new byte[16];
                var offset = 0;
                while (offset < iv.Length)
                {
                    offset += ms.Read(iv, offset, iv.Length - offset);
                }
                desObj.IV = iv;
                string result = "";
                //Read the bytes to be decrypted
                using (var cs = new CryptoStream(ms, desObj.CreateDecryptor(), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();
                    }

                }


                return result;
            }
        }

        public static string Encrypt2(string plaintext)
        {
            byte[] iv= Encoding.ASCII.GetBytes(InitializationVector);
            byte[] array;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(SecretKey);
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key,aesAlg.IV);
                using(MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter((Stream)csEncrypt))
                        {
                            swEncrypt.Write(plaintext);
                        }
                        array = ms.ToArray();
                    }
                   
                }
            };
          //  using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);
            
            return Convert.ToBase64String(array);


        }

        public static string Decrypt2(string plaintext)
        {
            byte[] iv = Encoding.ASCII.GetBytes(InitializationVector); ;
            byte[] array = Convert.FromBase64String(plaintext);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(SecretKey);
                aesAlg.IV = iv;
                ICryptoTransform encryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream ms = new MemoryStream(array))
                {
                    using (CryptoStream csEncrypt = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader((Stream)csEncrypt))
                        {
                           return reader.ReadToEnd();
                        }
                       // array = ms.ToArray();
                    }

                }
            };
            //  using var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV);

            //return Convert.ToBase64String(array);


        }
    }


    
}
