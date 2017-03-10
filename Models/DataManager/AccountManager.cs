using Swertres.Web.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Swertres.Web.Models.DataManager
{
    public class AccountManager
    {
        public string GetUserPassword(string userName)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var user = db.Users.Where(o => o.UserName.ToLower().Equals(userName.ToLower()));
                if (user.Any())
                    return user.SingleOrDefault().Password;
                else
                    return string.Empty;
            }
        }

        
    }

    public static class CurrentUser
    {
        public static long ID { get; set; }
        public static string Name { get; set; }
        public static bool IsAuthenticated { get; set; }
    }

    public static class User
    {
        public static void SetCurrentUser()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            using (SwertresEntities db = new SwertresEntities())
            {
                var user = db.Users.Where(o => o.UserName.ToLower().Equals(userName.ToLower())).Single();

                CurrentUser.ID = user.UserID;
                CurrentUser.Name = $"{user.FirstName} {user.LastName}";
                CurrentUser.IsAuthenticated = true;
            }
        }
    }
}