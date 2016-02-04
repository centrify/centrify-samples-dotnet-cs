using System;

using Centrify.Samples.DotNet.ApiLib;

namespace Centrify.Samples.DotNet.Client
{
    class Program
    {
        static void Main(string[] args)
        {            
            // All api's require that the user first be authenticated, doing so using the sample InteractiveLogin.Authenticate
            //  supports most of the MFA mechanisms provided by CIS (Email, SMS, OTP, OATH, Phone Call, Security Question), and
            //  returns us a client which has the appropriate authentication cookies in place.
            RestClient authenticatedRestClient = InteractiveLogin.Authenticate("https://devdog.centrify.com");

            // We can now use that client to perform actions as the authenticated user, the ApiClient class has a method per
            //  REST api, which uses a RestClient for communication:
            ApiClient apiClient = new ApiClient(authenticatedRestClient);

            try
            {
                // Get my applications and list them out
                var myApplications = apiClient.GetUPData();
                Console.WriteLine("User's Application List:");
                foreach (var app in myApplications)
                {
                    Console.WriteLine("Name: {0} Key: {1} Icon: {2}", app["DisplayName"], app["AppKey"], app["Icon"]);
                }

                // Get applications assigned to the sysadmin role only
                var sysadminApps = apiClient.GetRoleApps("sysadmin");
                Console.WriteLine("App ID's assigned to sysadmin:");
                foreach (var app in sysadminApps)
                {
                    Console.WriteLine("Key: {0}", app["Row"]["ID"]);
                }

                // Create a new CUS user
                // apiClient.CreateUser("joe@suffix.com", "newP@3651awdF@!%^");

                // Update the credentials for my UP app...
                // apiClient.UpdateApplicationDE("someAppKeyFromGetUPData", "newUsername", "newPassword");


            }
            catch(Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }

            // Sign out
            apiClient.Logout();
        }
    }
}
