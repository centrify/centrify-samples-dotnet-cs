/**
 * Copyright 2016 Centrify Corporation
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *  http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 **/
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
            RestClient authenticatedRestClient = InteractiveLogin.Authenticate("https://cloud.centrify.com");

            // We can now use that client to perform actions as the authenticated user, the ApiClient class has a method per
            //  REST api, which uses a RestClient for communication:
            ApiClient apiClient = new ApiClient(authenticatedRestClient);

            // If we wanted to convert this authenticated session into a useable bearer token for headless activity, we can get it:
            // Console.WriteLine("Bearer token for this session: {0}", apiClient.BearerToken);

            // Alternatively, if we already have a bearer token, we can use that:
            // ApiClient apiClient = new ApiClient("https://cloud.centrify.com", ExistingBearerToken);

            try
            {
                #region Execute a query and display the results
                //// Lets query for top user logins from last 30 days
                //var queryResult = apiClient.Query(@"select NormalizedUser as User, Count(*) as Count from Event
                //                                        where EventType = 'Cloud.Core.Login'
                //                                            and WhenOccurred >= DateFunc('now', '-30')
                //                                            group by User
                //                                        order by count desc");

                //Console.WriteLine("Query resulted in {0} results, first 5:", queryResult["FullCount"]);

                //// Dump to console the first 5
                //var queryResultRows = queryResult["Results"];
                //var queryResultColumns = queryResult["Columns"];
                //for(int x = 0; x < Math.Min(5, queryResult["FullCount"]); x++)
                //{
                //    Console.Write("\t");
                //    foreach(var column in queryResultColumns)
                //    {
                //        Console.Write("{0} => {1}\t", column["Name"], queryResultRows[x]["Row"][column["Name"]]);
                //    }
                //    Console.WriteLine();
                //}
                #endregion

                #region Get user's assigned applications
                //// Get my applications and list them out
                //var myApplications = apiClient.GetUPData();
                //Console.WriteLine("User's Application List:");
                //foreach (var app in myApplications)
                //{
                //    Console.WriteLine("Name: {0} Key: {1} Icon: {2}", app["DisplayName"], app["AppKey"], app["Icon"]);
                //}
                #endregion

                #region Get applications list assigned to a role
                //// Get applications assigned to the sysadmin role only
                //var sysadminApps = apiClient.GetRoleApps("sysadmin");
                //Console.WriteLine("App ID's assigned to sysadmin:");
                //foreach (var app in sysadminApps)
                //{
                //    Console.WriteLine("Key: {0}", app["Row"]["ID"]);
                //}
                #endregion

                #region Create a new CUS user
                //// Create a new CUS user
                // string userUuid = apiClient.CreateUser("apitest@contoso", "newP@3651awdF@!%^");
                #endregion

                #region Lock/Unlock a CUS user
                //// Lock a CUS user's account, UUID can come from CreateUser, or a Query on DsUser table
                //apiClient.LockUser(userUuid);

                //apiClient.UnlockUser(userUuid);
                #endregion

                #region Update a username/password applications stashed credentials
                // Update the credentials for my UP app...
                // apiClient.UpdateApplicationDE("someAppKeyFromGetUPData", "newUsername", "newPassword");
                #endregion                
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }

            // Sign out
            apiClient.Logout();
        }
    }
}
