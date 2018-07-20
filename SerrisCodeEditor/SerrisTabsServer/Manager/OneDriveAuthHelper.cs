using Microsoft.OneDrive.Sdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Manager
{
    public static class OneDriveAuthHelper
    {
        private static string[] scopes = new string[] { "wl.signin", "wl.skydrive", "wl.skydrive_update" };

        /*
         *      Verify if the user has been logged on OneDrive
         */
        public static async Task<bool> VerifyOneDriveLogin()
        {
            var msaAuthenticationProvider = new OnlineIdAuthenticationProvider(scopes);
            await msaAuthenticationProvider.RestoreMostRecentFromCacheAsync();

            if (msaAuthenticationProvider.CurrentAccountSession != null)
                return true;
            else
                return false;

        }

        /*
         *      Authentification to OneDrive
         */
        public static async Task<bool> OneDriveAuthentification()
        {
            try
            {
                var msaAuthenticationProvider = new OnlineIdAuthenticationProvider(scopes);

                if (!msaAuthenticationProvider.IsAuthenticated)
                {
                    await msaAuthenticationProvider.AuthenticateUserAsync();
                    await msaAuthenticationProvider.RestoreMostRecentFromCacheOrAuthenticateUserAsync();
                    TabsDataCache.OneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", msaAuthenticationProvider);
                    TabsDataCache.AuthProvider = msaAuthenticationProvider;
                    return true;
                }
            }
            catch { return false; }

            return false;
        }

        /*
         *      Sign out
         */
        public static async void OneDriveSignOut()
        {
            try
            {
                var msaAuthenticationProvider = new OnlineIdAuthenticationProvider(scopes);
                await msaAuthenticationProvider.SignOutAsync();
            }
            catch { }
        }
    }
}
