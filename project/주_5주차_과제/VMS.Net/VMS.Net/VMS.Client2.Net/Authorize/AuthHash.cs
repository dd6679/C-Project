using System;
using System.Security.Cryptography;
using System.Text;

namespace VMS.Client2.Net
{
    public abstract class AuthHash
    {
        public abstract byte[] MakeHash(AuthInfo auth, string password, ServerInfo serverInfo, byte[] nonce);
        public static string ToHexString(byte[] hash)
        {
            string hex = BitConverter.ToString(hash);
            return hex.ToLower().Replace("-", "");
        }

        public static AuthHash Create(string version)
        {
            //version == "1.1.0.1"
            return new Sha256Hash();
        }

        public virtual string SerializeJson()
        {
            return string.Empty;
        }
    }

    public class Sha256Hash : AuthHash
    {
        SHA256 sha256Hash = SHA256.Create();
        public override byte[] MakeHash(AuthInfo auth, string password, ServerInfo serverInfo, byte[] nonce)
        {
            var hashString = auth.User + ":" + serverInfo.Server + "/" + serverInfo.Version + ":";
            hashString += ToHexString(nonce);
            hashString += ":";
            hashString += ToHexString(MakeAuthHash(auth, password));

            return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(hashString));
        }

        public byte[] MakeAuthHash(AuthInfo auth, string password)
        {
            var userPassString = auth.User + ":" + password;
            return sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(userPassString));
        }
    }
}
