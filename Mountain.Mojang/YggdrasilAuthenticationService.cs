using System.Security.Cryptography;

namespace Mountain.Mojang
{
    public class YggdrasilAuthenticationService
    {
        public const string AuthServer = "https://authserver.mojang.com";

        public YggdrasilAuthenticationService()
        {
            RSA rsa = RSA.Create(1024);
            var rsaParams = rsa.ExportParameters(false);
            
        }
    }
}
