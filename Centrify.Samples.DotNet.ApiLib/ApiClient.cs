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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Centrify.Samples.DotNet.ApiLib
{
    public class ApiClient
    {
        private RestClient m_restClient = null;

        public ApiClient(RestClient authenticatedClient)
        {
            m_restClient = authenticatedClient;
        }

        // Illustrates locking a CUS user via /cdirectoryservice/setuserstate
        public void LockUser(string userUuid)
        {
            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args["ID"] = userUuid;            
            args["state"] = "Locked";

            var result = m_restClient.CallApi("/cdirectoryservice/setuserstate", args);
            if (result["success"] != true)
            {
                Console.WriteLine("LockUser {0} failed: {1}", userUuid, result["Message"]);
                throw new ApplicationException(result["Message"]);
            }
        }

        // Illustrates unlocking a CUS user via /cdirectoryservice/setuserstate
        public void UnlockUser(string userUuid)
        {
            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args["ID"] = userUuid;
            args["state"] = "None";

            var result = m_restClient.CallApi("/cdirectoryservice/setuserstate", args);
            if (result["success"] != true)
            {
                Console.WriteLine("UnlockUser {0} failed: {1}", userUuid, result["Message"]);
                throw new ApplicationException(result["Message"]);
            }
        }

        // Illustrates usage of /redrock/query to run queries
        public dynamic Query(string sql)
        {
            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args["Script"] = sql;

            Dictionary<string, dynamic> queryArgs = new Dictionary<string, dynamic>();
            args["Args"] = queryArgs;

            queryArgs["PageNumber"] = 1;
            queryArgs["PageSize"] = 10000;
            queryArgs["Limit"] = 10000;
            queryArgs["Caching"] = -1;

            var result = m_restClient.CallApi("/redrock/query", args);
            if (result["success"] != true)
            {
                Console.WriteLine("Running query failed: {0}", result["Message"]);
                throw new ApplicationException(result["Message"]);
            }

            return result["Result"];
        }


        // Illustrates usage of /saasManage/updateapplicationde to stash a new username/password for a UP application
        public void UpdateApplicationDE(string appKey, string username, string password)
        {
            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args["UserNameStrategy"] = "Fixed";
            args["Password"] = password;
            args["UserNameArg"] = username;

            var result = m_restClient.CallApi("/saasmanage/updateapplicationde?_RowKey=" + appKey, args);
            if (result["success"] != true)
            {
                Console.WriteLine("Updating app credentials failed: {0}", result["Message"]);
                throw new ApplicationException(result["Message"]);
            }
        }

        // Illustrates usage of /uprest/GetUPData to get a list of all applications
        //  assigned to the current user
        public dynamic GetUPData()
        {
            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            args["force"] = false;

            var result = m_restClient.CallApi("/uprest/getupdata", args);
            if (result["success"] != true)
            {
                Console.WriteLine("Getting apps list failed: {0}", result["Message"]);
                throw new ApplicationException(result["Message"]);
            }

            return result["Result"]["Apps"];
        }

        // Illustrates usage of /saasmanage/getroleapps to get apps assigned to a role
        public dynamic GetRoleApps(string role)
        {
            Dictionary<string, dynamic> args = new Dictionary<string, dynamic>();
            var result = m_restClient.CallApi("/saasmanage/getroleapps?role=" + role, args);

            if (result["success"] != true)
            {
                Console.WriteLine("Getting apps list for role {0} failed: {1}", role, result["Message"]);
                throw new ApplicationException(result["Message"]);
            }

            return result["Result"]["Results"];
        }

        // Illustrates usage of /cdirectoryservice/createuser to create a new CUS user, presumes
        //  username and mail are the same.  Return value is user's UUID
        public string CreateUser(string username, string password)
        {
            Dictionary<string, dynamic> createUserArgs = new Dictionary<string, dynamic>();
            createUserArgs["Name"] = username;
            createUserArgs["Mail"] = username;
            createUserArgs["Password"] = password;

            var result = m_restClient.CallApi("/cdirectoryservice/createuser", createUserArgs);

            if (result["success"] != true)
            {
                Console.WriteLine("Creating user {0} failed: {1}", username, result["Message"]);
                throw new ApplicationException(result["Message"]);
            }

            Console.WriteLine("Creating user {0} succeeded.", username);
            return result["Result"];
        }

        // Illustrates logout
        public void Logout()
        {
            m_restClient.CallApi("/security/logout", new Dictionary<string, dynamic>());
        }
    }
}
