using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swertres.Web.Models.DB;
using Swertres.Web.Models.ViewModels;
using Swertres.Web.Models.Helpers;

namespace Swertres.Web.Models.DataManager
{
   
    public class AdminManager
    {

        #region Draws
        public CreateDrawModel CreateDraw()
        {
            string controlNumber = string.Empty;
            using (SwertresEntities db = new SwertresEntities())
            {
                var lastDraw = db.Draws.OrderByDescending(o => o.DrawID).Select(o => o.ControlNumber);
                if (lastDraw.Any())
                    controlNumber = Security.GenerateControlNumber(Convert.ToInt64(lastDraw.First()));
                else
                    controlNumber = Security.GenerateControlNumber();
            }

            CreateDrawModel draw = new CreateDrawModel()
            {
                ControlNumber = controlNumber,
                DrawTime = new DefaultDrawTime
                {
                    SelectedDrawTime = "1",
                    DrawTime = GetDrawTime()
                }
            };

            return draw;
        }
    
        public string AddNewDraw(CreateDrawModel dto)
        {

            using (SwertresEntities db = new SwertresEntities())
            {
                if (!HasActiveDraw())
                {
                    //User.SetCurrentUser();
                    Draw draw = new Draw()
                    {
                        ControlNumber = dto.ControlNumber,
                        StartDateTime = Convert.ToDateTime(GetDrawTime().Find(o => o.Value.Equals(dto.DrawTime.SelectedDrawTime)).Text),
                        IsEnded = false,
                        RowCreatedDateTime = DateTime.Now,
                        RowCreatedUserID = CurrentUser.ID
                    };

                    db.Draws.Add(draw);
                    db.SaveChanges();

                    return draw.ControlNumber;
                }
                else
                    return string.Empty;
            }
        }

        public List<DrawModel> GetDraws(string controlNumber = "")
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                if (controlNumber.Equals(string.Empty))
                    return db.Draws.AsEnumerable().Select(o => new DrawModel { DrawID = o.DrawID, ControlNumber = o.ControlNumber, SelectedDrawTime = o.StartDateTime == null? string.Empty : o.StartDateTime.GetValueOrDefault().ToString("hh:mm tt"), IsEnded = o.IsEnded }).ToList();
                else
                    return db.Draws.AsEnumerable().Where(o => o.ControlNumber == controlNumber).Select(o => new DrawModel { DrawID = o.DrawID, ControlNumber = o.ControlNumber, SelectedDrawTime = o.StartDateTime == null ? string.Empty : o.StartDateTime.GetValueOrDefault().ToString("hh:mm tt"), IsEnded = o.IsEnded }).ToList();
            }
        }

        public DrawModel GetActiveDraw()
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var result = db.Draws.AsEnumerable().Where(o => o.IsEnded == false).Select(o => new DrawModel { DrawID = o.DrawID, ControlNumber = o.ControlNumber, SelectedDrawTime = o.StartDateTime == null ? string.Empty : o.StartDateTime.GetValueOrDefault().ToString("hh:mm tt"), IsEnded = o.IsEnded });
                if (result.Any())
                    return result.Single();

                return new DrawModel();
            }
        }

        public bool HasActiveDraw()
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                return db.Draws.Where(o => o.IsEnded == false).Any();
            }
        }

        public List<DrawTime> GetDrawTime()
        {
            List<DrawTime> time = new List<DrawTime>();
            time.Add(new DrawTime { Text = "11:AM", Value = "1" });
            time.Add(new DrawTime { Text = "3:PM", Value = "2" });
            time.Add(new DrawTime { Text = "7:PM", Value = "3" });
            return time;
        }

        public void UpdateDrawTime(long drawID, string selectedDrawTime)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var draw = db.Draws.Where(o => o.DrawID == drawID);
                if (draw.Any())
                {
                    var d = draw.First();
                    d.StartDateTime = Convert.ToDateTime(GetDrawTime().Find(o => o.Value.Equals(selectedDrawTime)).Text.Replace(":",""));
                    d.RowModifiedDateTime = DateTime.Now;
                    d.RowModifiedUserID = CurrentUser.ID;

                    db.SaveChanges();
                }
            }
        }

        public void CloseCurrentDraw(long drawID)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var draw = db.Draws.Where(o => o.DrawID == drawID);
                if (draw.Any())
                {
                    var d = draw.First();
                    d.IsEnded = true;
                    d.RowModifiedDateTime = DateTime.Now;
                    d.RowModifiedUserID = CurrentUser.ID;
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region Sources
        public void AddNewSource(CreateSourceModel dto)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var source = new Source()
                {
                    SourceName = dto.SourceName,
                    Share = dto.Share
                };

                db.Sources.Add(source);
                db.SaveChanges();
            }
        }

        public List<SourceModel> GetSource(long id = 0)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                if (id == 0)
                    return db.Sources.Select(o => new SourceModel { SourceID = o.SourceID, SourceName =o.SourceName, Share = o.Share == null ? 0 : o.Share }).ToList();
                else
                    return db.Sources.Where(o => o.SourceID == id).Select(o => new SourceModel { SourceID = o.SourceID, SourceName = o.SourceName, Share = o.Share == null ? 0 : o.Share }).ToList();
            }
        }

        public List<SourceModel> GetCoordinator()
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                return db.Sources.Select(o => new SourceModel {
                    SourceID = o.SourceID,
                    SourceName = o.SourceName,
                    Share = o.Share == null ? 0 : o.Share }
                ).ToList();
            }
        }

        public void UpdateSource(SourceModel dto)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var result = db.Sources.Where(o => o.SourceID.Equals(dto.SourceID));
                if (result.Any())
                {
                    var source = result.First();
                    source.SourceName = dto.SourceName;
                    source.Share = dto.Share;
                    db.SaveChanges();
                }

            }
        }
        #endregion

        #region Users

        public long AddUser(NewUserModel dto)
        {
            long id = 0;
            using (SwertresEntities db = new SwertresEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        DB.User newUser = new DB.User()
                        {
                            FirstName = dto.User.FirstName,
                            LastName = dto.User.LastName,
                            UserName = dto.User.UserName,
                            Password = Security.GenerateSaltedHash(dto.User.Password)
                        };

                        db.Users.Add(newUser);
                        db.SaveChanges();
                        id = newUser.UserID;

                        DB.UserRole role = new DB.UserRole()
                        {
                            UserID = id,
                            RoleID = dto.Role.SelectedRoleID
                        };

                        db.UserRoles.Add(role);
                        db.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch(Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }

            return id;
        }

        public IEnumerable<UserModel> GetAllUsers()
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                return (from user in db.Users
                        join userRoles in db.UserRoles on user.UserID equals userRoles.UserID
                        join roles in db.Roles on userRoles.RoleID equals roles.RoleID
                        select new { user, roles }).ToList().Select(o => 
                        new UserModel
                        {
                            UserID = o.user.UserID,
                            UserName = o.user.UserName,
                            FirstName = o.user.FirstName,
                            LastName = o.user.LastName,
                            IsLocked = o.user.IsLocked == null ? false : true,
                            LockedDate = o.user.LockedDate,
                            ResetByID = o.user.PasswordResetUserID,
                            ResetByName = GetUser(o.user.UserID).FullName,
                            ResetDate = o.user.PasswordResetDate,
                            RoleName = o.roles.RoleName,
                            RoleID = o.roles.RoleID
                            //Role = new UserRoleModelValidation { SelectedRoleID = o.roles.RoleID, Roles = GetRoles(0) }
                        }).ToList();
            }
        }

        public UserModel GetUser(long userID)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var user = db.Users.Where(o => o.UserID == userID);
                if (user.Any())
                {
                    return user.Select(o => new UserModel
                    {
                        UserID = o.UserID,
                        FirstName = o.FirstName,
                        LastName = o.LastName,
                        UserName = o.UserName,
                        Password = o.Password
                    }).First();
                }

                return new UserModel();
            }
        }

        public void UpdateUser(UserModel dto)
        {
           
            using (SwertresEntities db = new SwertresEntities())
            {
                using (var dbContextTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var result = db.Users.Where(o => o.UserID.Equals(dto.UserID));
                        if (result.Any())
                        {
                            var user = result.First();
                            user.FirstName = dto.FirstName;
                            user.LastName = dto.LastName;
                            user.UserName = dto.UserName;

                            db.SaveChanges();
                        }



                        var role = db.UserRoles.Where(o => o.UserID.Equals(dto.UserID)).FirstOrDefault();
                        role.RoleID = dto.RoleID;
                        db.SaveChanges();

                        dbContextTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbContextTransaction.Rollback();
                    }
                }
            }
        }

        public List<ViewModels.UserRole> GetRoles(long id = 0)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                if (id == 0)
                    return db.Roles.Select(o => new ViewModels.UserRole
                    {
                        RoleID = o.RoleID,
                        RoleName = o.RoleName,
                        RoleDescription = o.RoleDescription
                    }).ToList();
                else
                    return db.Roles.Where(o => o.RoleID == id).Select(o => new ViewModels.UserRole
                    {
                        RoleID = o.RoleID,
                        RoleName = o.RoleName,
                        RoleDescription = o.RoleDescription
                    }).ToList();
            }
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var user = db.Users.Where(o => o.UserName.Equals(userName))?.FirstOrDefault();
                if (user != null)
                {
                    var roles = from q in db.UserRoles
                                join r in db.Roles on q.RoleID equals r.RoleID
                                where r.RoleName.Equals(roleName) && q.UserID.Equals(user.UserID)
                                select r.RoleName;

                    if (roles != null)
                    {
                        return roles.Any();
                    }
                }

                return false;
            }
        }
        #endregion
    }
}