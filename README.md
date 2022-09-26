# Centrify.Samples.DotNet

# PUBLIC ARCHIVE

> ***NOTE***
> This repo is archived.
> This is still available under the licensing terms, but is not being actively developed or updated any further. Please see [DelineaXPM](https://github.com/DelineaXPM) for active projects.

Notes: This package contains code samples for the Centrify Identity Service Platform API's written in C#.  The solution
Centrify.Samples.DotNet.sln (VS 2015) contains two project:
  1. Centrify.Samples.DotNet.ApiLib - Includes a general REST client for communicating with the CIS Platform, as well as
  a class, ApiClient, which uses the REST client to make calls to specific platform API's.
  2. Centrify.Samples.DotNet.Client - A sample console application which utilizes the ApiLib project to authenticate a user
  using MFA, then retrieve their assigned application list.  Can be modified to invoke any of the ApiLib functionality as well.
 

Sample Centrify.Samples.DotNet.ApiLib Functionality Includes:

    1. Utilizing interactive MFA to authenticate a user and retrieve a session for interacting with the platform: InteractiveLogin.cs
    2. Issuing queries to the report system: ApiLib.ExecuteQuery()
    3. Updating credentials on a UsernamePassword application: ApiLib.UpdateApplicationDE()
    4. Getting assigned apps (User Portal view): ApiLib.GetUPData()
    5. Getting assigned apps by role: ApiLib.GetRoleApps()
    6. Creating a new CUS user: ApiLib.CreateUser()
    7. Locking/Unlocking a CUS user: ApiLib.LockUser(), ApiLib.UnlockUser()
    
    For support, please contact devsupport@centrify.com
   
