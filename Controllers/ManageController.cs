using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Swertres.Web.Models.DataManager;
using Swertres.Web.Models.ViewModels;

namespace Swertres.Web.Controllers
{
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
        #endregion
    }
}