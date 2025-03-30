using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace Doryu.JBSave
{
    public static class JBSaver
    {
        //Set path
        //C:/../AppData/LocalLow/(ConpanyName)/(ProjectName)/SaveDatas/bins/
        static private string _LocalPath = Path.Combine(Application.persistentDataPath, "SaveDatas/bins/");

        private static Encoding _ByteConverter = Encoding.UTF8;
        //Crypto Key
        private static readonly byte[] _Key = Encoding.UTF8.GetBytes("DoryuSaveAESCryptoKey2024".PadRight(32, '0'));

        /// <summary>
        /// Load the class by its name.
        /// </summary>
        public static bool LoadJson<T>(this T loadClass, string saveFileName) where T : ISavable<T>
        {
            string path = Path.Combine(_LocalPath, saveFileName + ".bin");
            if (File.Exists(path))
            {
                //Byte read and decryption
                byte[] bytes = null;
                try
                {
                    bytes = File.ReadAllBytes(path).AES_Decrypt(_Key);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Path: {path}\nLoad failed: {ex.ToString()}");
                    return false;
                }
                //Byte to json
                string json = _ByteConverter.GetString(bytes);
                //Json to class
                T res = JsonUtility.FromJson<T>(json);
                loadClass.OnLoadData(res);
                return true;
            }
            else
            {
                Debug.LogError($"Path not found.\nPath: {path}");
                return false;
            }
        }
        /// <summary>
        /// Save the class with that name.
        /// </summary>
        public static void SaveJson<T>(this T saveClass, string saveFileName) where T : ISavable<T>
        {
            if (Directory.Exists(_LocalPath) == false)
            {
                Directory.CreateDirectory(_LocalPath);
            }
            //Save path
            string path = Path.Combine(_LocalPath, saveFileName + ".bin");
            //Class to json
            string jsonData = JsonUtility.ToJson(saveClass, true);
            //Json to byte and encryption
            byte[] bytes = _ByteConverter.GetBytes(jsonData).AES_Encrypt(_Key);
            saveClass.OnSaveData(saveFileName);

            File.WriteAllBytes(path, bytes);
        }

        private static byte[] AES_Encrypt(this byte[] data, byte[] key)
        {
            using Aes aesAlg = Aes.Create();

            aesAlg.Key = key;
            //Create IV
            aesAlg.GenerateIV();

            using MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(aesAlg.IV, 0, aesAlg.IV.Length);

            //Encrypt
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
        }

        private static byte[] AES_Decrypt(this byte[] data, byte[] key)
        {
            using Aes aesAlg = Aes.Create();

            aesAlg.Key = key;
            //Get IV
            byte[] iv = new byte[16];
            Array.Copy(data, 0, iv, 0, iv.Length);
            aesAlg.IV = iv;

            using MemoryStream memoryStream = new MemoryStream(data, iv.Length, data.Length - iv.Length);

            //Decrypt
            using CryptoStream cryptoStream = new CryptoStream(memoryStream, aesAlg.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream decryptedStream = new MemoryStream();
            cryptoStream.CopyTo(decryptedStream);
            return decryptedStream.ToArray();
        }
    }
}