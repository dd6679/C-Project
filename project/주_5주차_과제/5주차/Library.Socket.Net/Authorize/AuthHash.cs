using System;
using System.Security.Cryptography;
using System.Text;

namespace Library.SocketCommunication
{

    public class AuthHash
    {
        public byte[] MakeHash(AuthInfo auth, string password, ServerInfo serverInfo, byte[] nonce)
        {
            var hashString = auth.User + ":" + serverInfo.Server + "/" + serverInfo.Version + ":";
            hashString += ToHexString(nonce);
            hashString += ":";
            hashString += ToHexString(MakeAuthHash(auth, password));

            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(hashString));
        }

        public byte[] MakeAuthHash(AuthInfo auth, string password)
        {
            var userPassString = auth.User + ":" + password;
            return SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(userPassString));
        }

        public static string ToHexString(byte[] hash)
        {
            string hex = BitConverter.ToString(hash);
            return hex.ToLower().Replace("-", "");
        }
    }

}
