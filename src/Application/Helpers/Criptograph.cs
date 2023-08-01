using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace Application.Helpers
{
    public static class Criptograph
    {
        // Chave utilizada para a criptografia (Chave de 256 bits = 32 bytes)
        private static byte[] key =
        {
            0x54, 0x68, 0x69, 0x73, 0x49, 0x73, 0x41, 0x65,
            0x73, 0x4B, 0x65, 0x79, 0x46, 0x6F, 0x72, 0x41,
            0x45, 0x53, 0x2E, 0x4E, 0x45, 0x54, 0x2E, 0x61,
            0x6E, 0x64, 0x2E, 0x61, 0x77, 0x65, 0x73, 0x6F
        };

        public static string Decrypt(int escolaId)
        {
            throw new NotImplementedException();
        }

        public static string? Encrypt(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    byte[] bText = new UTF8Encoding().GetBytes(text);

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;
                        aesAlg.Mode = CipherMode.CBC;
                        aesAlg.Padding = PaddingMode.PKCS7;

                        // Inicializa o vetor de inicialização (IV)
                        aesAlg.GenerateIV();
                        byte[] iv = aesAlg.IV;

                        // Cria o espaço de memória para guardar o valor criptografado
                        MemoryStream mStream = new MemoryStream();
                        // Instancia o encriptador
                        using (ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, iv))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(mStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bText, 0, bText.Length);
                                cryptoStream.FlushFinalBlock();
                            }
                        }

                        // Concatena o IV com os dados criptografados
                        byte[] encryptedBytes = mStream.ToArray();
                        byte[] combinedBytes = new byte[iv.Length + encryptedBytes.Length];
                        Buffer.BlockCopy(iv, 0, combinedBytes, 0, iv.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, combinedBytes, iv.Length, encryptedBytes.Length);

                        // Converte para Base64 e URL encode
                        var retorno = WebUtility.UrlEncode(Convert.ToBase64String(combinedBytes));
                        return retorno;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao criptografar", ex);
            }
        }

        public static string? Decrypt(string text)
        {
            try
            {
                if (!string.IsNullOrEmpty(text))
                {
                    // URL decode e converte de Base64 para bytes
                    text = WebUtility.UrlDecode(text);
                    byte[] combinedBytes = Convert.FromBase64String(text);

                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = key;
                        aesAlg.Mode = CipherMode.CBC;
                        aesAlg.Padding = PaddingMode.PKCS7;

                        // Extrai o IV do início dos bytes combinados
                        byte[] iv = new byte[aesAlg.IV.Length];
                        byte[] encryptedBytes = new byte[combinedBytes.Length - iv.Length];
                        Buffer.BlockCopy(combinedBytes, 0, iv, 0, iv.Length);
                        Buffer.BlockCopy(combinedBytes, iv.Length, encryptedBytes, 0, encryptedBytes.Length);

                        aesAlg.IV = iv;

                        // Cria o espaço de memória para guardar o valor descriptografado
                        MemoryStream mStream = new MemoryStream();
                        // Instancia o decriptador
                        using (ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                                cryptoStream.FlushFinalBlock();
                            }
                        }

                        // Converte para string UTF-8
                        string decryptedText = Encoding.UTF8.GetString(mStream.ToArray());
                        return decryptedText;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Erro ao descriptografar", ex);
            }
        }
    }
}
