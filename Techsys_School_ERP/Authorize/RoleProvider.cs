using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Techsys_School_ERP.DBAccess;
using Techsys_School_ERP.Model;

namespace Techsys_School_ERP.Authorize
{
	public class MyRoleProvider : RoleProvider
	{
		public override string ApplicationName
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public override void AddUsersToRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override void CreateRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
		{
			throw new NotImplementedException();
		}

		public override string[] FindUsersInRole(string roleName, string usernameToMatch)
		{
			throw new NotImplementedException();
		}

		public override string[] GetAllRoles()
		{
			throw new NotImplementedException();
		}

		public override string[] GetRolesForUser(string username)
		{
			string[] sRoleNameArr = new string[1];

			using (var dbcontext = new SchoolERPDBContext())
			{
				var sRoleName = (from usr in dbcontext.Users
									 join rle in dbcontext.User_Roles on usr.Role_Id equals rle.Role_Id
									 where usr.User_Id == username 
									 select rle.Name).ToList();

				if (sRoleName.Count > 0)
				{
					sRoleNameArr[0] = sRoleName[0].ToString();
				}
				else
				{
					sRoleNameArr[0] = string.Empty;
				}
			}
			return sRoleNameArr;
		}

		public override string[] GetUsersInRole(string roleName)
		{
			throw new NotImplementedException();
		}

		public override bool IsUserInRole(string username, string roleName)
		{
			throw new NotImplementedException();
		}

		public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
		{
			throw new NotImplementedException();
		}

		public override bool RoleExists(string roleName)
		{
			throw new NotImplementedException();
		}
	}
}