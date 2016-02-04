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
            args["force"] = true;

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
        //  username and mail are the same.
        public void CreateUser(string username, string password)
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
        }

        // Illustrates logout
        public void Logout()
        {
            m_restClient.CallApi("/security/logout", new Dictionary<string, dynamic>());
        }
    }
}
