using Microsoft.Identity.Client;
using System.Threading.Tasks;

namespace MSGraphAuthClientCredSample
{
    public static class TokenStorageProvider
    {
        private static string TokenCacheFile = $"./tokencache.bin";
        public static void Initialize(ITokenCache tokenCache)
        {
            tokenCache.SetBeforeAccessAsync(BeforeAccessAsync);
            tokenCache.SetAfterAccessAsync(AfterAccessAsync);
        }

        private static async Task BeforeAccessAsync(TokenCacheNotificationArgs arg)
        {
            if (System.IO.File.Exists(TokenCacheFile))
            {
                byte[] tokenByte = await System.IO.File.ReadAllBytesAsync(TokenCacheFile);
                // load token byte from storage to token cache.
                arg.TokenCache.DeserializeMsalV3(tokenByte);
            }
        }

        private static async Task AfterAccessAsync(TokenCacheNotificationArgs arg)
        {
            if (arg.HasStateChanged)
            {
                var tokenByte = arg.TokenCache.SerializeMsalV3();
                // Write token cahce from cache to storage.
                await System.IO.File.WriteAllBytesAsync(TokenCacheFile, tokenByte);
            }
        }
    }
}
