using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Suinet.Wallet
{   
    public class SuiAddressConverter
    {
        private static readonly Dictionary<string, byte> SignatureSchemeToFlag = new Dictionary<string, byte>
    {
        { "ED25519", 0x00 },
        { "Secp256k1", 0x01 },
        { "Secp256r1", 0x02 },
        { "MultiSig", 0x03 }
    };

        private const int PUBLIC_KEY_SIZE = 32;
        private const int SUI_ADDRESS_LENGTH = 32;

        public static string ToSuiAddress(byte[] publicKey, string signatureScheme)
        {
            byte flag = GetSignatureSchemeFlag(signatureScheme);

            byte[] tmp = new byte[PUBLIC_KEY_SIZE + 1];
            tmp[0] = flag;
            Array.Copy(publicKey, 0, tmp, 1, publicKey.Length);

            byte[] hash = Blake2b(tmp, 256);
            string hexHash = BytesToHex(hash);
            string normalizedAddress = NormalizeSuiAddress(hexHash);

            return normalizedAddress;
        }

        private static byte GetSignatureSchemeFlag(string signatureScheme)
        {
            if (SignatureSchemeToFlag.ContainsKey(signatureScheme))
            {
                return SignatureSchemeToFlag[signatureScheme];
            }
            throw new ArgumentException("Invalid signature scheme");
        }

        private static byte[] Blake2b(byte[] data, int outputSize)
        {
            using (HMACBlake2B blake2b = new HMACBlake2B(outputSize))
            {
                return blake2b.ComputeHash(data);
            }
        }

        private static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
            {
                sb.Append(b.ToString("x2"));
            }
            return sb.ToString();
        }

        private static string NormalizeSuiAddress(string value)
        {
            string address = value.ToLower();
            if (address.StartsWith("0x"))
            {
                address = address.Substring(2);
            }
            return $"0x{address.PadLeft(SUI_ADDRESS_LENGTH * 2, '0')}";
        }
    }

}
