using Swertres.Web.Models.DataManager;
using Swertres.Web.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Swertres.Web.Models.Helpers
{
    public class Security
    {
        public static string GenerateSaltedHash(string secret)
        {
            return Crypto.HashPassword(secret);
        }

        public static bool VerifySaltedHash(string hashedValue, string secret)
        {
            return Crypto.VerifyHashedPassword(hashedValue, secret);
        }

        public static string GenerateRandomPassword(int lenght, int numOfAlphaNumericChars)
        {
            return Membership.GeneratePassword(lenght, numOfAlphaNumericChars);
        }

        public static string GenerateControlNumber(long num = 0)
        {
            long generated = 0;
            if (num == 0)
                generated = 1000;
            else
                generated = num + 1;

            return generated.ToString();
        }
    }

    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        private readonly string[] userAssignedRoles;
        public AuthorizeRolesAttribute(params string[] roles)
        {
            this.userAssignedRoles = roles;
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool authorize = false;
            using (SwertresEntities db = new SwertresEntities())
            {
                AdminManager am = new AdminManager();
                foreach (var roles in userAssignedRoles)
                {
                    authorize = am.IsUserInRole(httpContext.User.Identity.Name, roles);
                    if (authorize)
                        return authorize;
                }
            }
            return authorize;
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Home/UnAuthorized");
        }
    }
}