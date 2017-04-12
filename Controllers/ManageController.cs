using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Swertres.Web.Models.DataManager;
using Swertres.Web.Models.ViewModels;
using Swertres.Web.Models.Helpers;

namespace Swertres.Web.Controllers
{
    [AuthorizeRoles("Admin")]
    public class ManageController : Controller
    {
        AdminManager _am;
        public ManageController()
        {
            _am = new AdminManager();
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        #region Draw
        public ActionResult Draw()
        {
            var newDraw = _am.CreateDraw();
            return View(newDraw);
        }

        [HttpPost]
        public ActionResult Draw(CreateDrawModel dto)
        {
            if (ModelState.IsValid)
            {
                var result = _am.AddNewDraw(dto);
                var newDraw = _am.CreateDraw();

                if (result == string.Empty)
                    ModelState.AddModelError("", "Can't create a new entry because there's an existing active draw.");

                return View(newDraw);
            }

            return View(dto);
        }

        public ActionResult DrawListPartial()
        {
            var dto = new DrawListModel() { DrawList = _am.GetDraws() };
            return PartialView(dto);
        }

        [HttpPost]
        public ActionResult UpdateDrawTime(long drawID, string selectedDrawTime)
        {
            _am.UpdateDrawTime(drawID, selectedDrawTime);
            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult CloseCurrentDraw(long drawID)
        {
            _am.CloseCurrentDraw(drawID);
            return Json(new { success = true });
        }
        #endregion

        #region Source
        public ActionResult Source()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Source(CreateSourceModel dto)
        {
            if (ModelState.IsValid)
            {
                _am.AddNewSource(dto);
                RedirectToAction("Source");
            }

            return View(dto);
        }

        public ActionResult SourceListPartial()
        {
            var dto = new SourceListModel() { SourceList = _am.GetSource() };
            return PartialView(dto);
        }

        public ActionResult UpdateSource(long sourceID, string name, string share)
        {
            if (share.Equals(string.Empty))
                share = "0";

            var dto = new SourceModel();
            dto.SourceID = sourceID;
            dto.SourceName = name;
            dto.Share = Convert.ToInt32(share);

            _am.UpdateSource(dto);
            return Json(new { success = true });
        }
        #endregion

        #region User
        public ActionResult Users()
        {
            var dto = new NewUserModel { User = new UserModel(), Role = new UserRoleModelValidation { Roles = _am.GetRoles() } };
            
            return View(dto);
        }

        [HttpPost]
        public ActionResult Users(NewUserModel dto)
        {
            if (ModelState.IsValid)
            {
                _am.AddUser(dto);
                RedirectToAction("Users");
            }

            var dtoDefault = new NewUserModel { User = new UserModel(), Role = new UserRoleModelValidation { Roles = _am.GetRoles() } };
            return View(dtoDefault);
        }

        public ActionResult UserListPartial()
        {
            var dto = _am.GetAllUsers();
            return PartialView(dto);
        }

        public ActionResult UpdateUser(long userID, int roleID, string firstName,string lastName, string userName)
        {
            var dto = new UserModel();
            dto.UserID = userID;
            dto.RoleID = roleID;
            dto.FirstName = firstName;
            dto.LastName = lastName;
            dto.UserName = userName;

            _am.UpdateUser(dto);
            return Json(new { success = true });
        }
        #endregion
    }
}