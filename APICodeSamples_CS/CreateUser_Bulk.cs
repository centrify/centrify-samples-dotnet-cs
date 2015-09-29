//Expected CSV format
//Username,Password
//testuser@domain.com,newpass1

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web.Script.Serialization; //Must include System.Web.Extensions as a referance.
using APICodeSamples.Classes; //Centrify_API_Interface class location.

namespace APICodeSamples
{
    class CreateUser_Bulk
    {
        public static string CentLoginURL = "https://cloud.centrify.com/security/login";
        public static string CentCreateUserURL = "https://pod.centrify.com/cdirectoryservice/createuser";

        public static string UserName = "user@domain.com";
        public static string Password = "userPass";

        public void CreateUser_Bulk()
        {
            List<string[]> newUsers = ParseCSV(@"c:\newUsers.csv");

            string loginJSON = "{user:'" + UserName + "', password:'" + Password + "'}";
            Centrify_API_Interface centLogin = new Centrify_API_Interface().MakeRestCall(CentLoginURL, loginJSON);

            foreach (var user in newUsers)
            {
                string NewUserName = (string)user.GetValue(0);
                string NewPassword = (string)user.GetValue(1);

                string strCreateUserJSON = "{Name:'" + UserName + "', Mail:'" + UserName + "', Password:'" + Password + "'}";
                Centrify_API_Interface centCreateUser = new Centrify_API_Interface().MakeRestCall(CentCreateUserURL, strCreateUserJSON, centLogin.returnedCookie);
                var jssAdvanceAuthPoll = new JavaScriptSerializer();
                Dictionary<string, dynamic> centCreateUser_Dict = jssAdvanceAuthPoll.Deserialize<Dictionary<string, dynamic>>(centCreateUser.returnedResponse);

                if (centCreateUser_Dict["Message"].ToString() != null)
                {
                    Console.WriteLine("Error: " + centCreateUser_Dict["Message"].ToString());
                }
                else
                {
                    Console.WriteLine("Create User Successful.");
                }
            }
        }

        public List<string[]> ParseCSV(string path)
        {
            List<string[]> parsedData = new List<string[]>();

            try
            {
                using (StreamReader readFile = new StreamReader(path))
                {
                    string line;
                    string[] row;

                    while ((line = readFile.ReadLine()) != null)
                    {
                        row = line.Split(',');
                        parsedData.Add(row);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return parsedData;
        }
    }
}
