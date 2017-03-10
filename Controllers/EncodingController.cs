using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Swertres.Web.Models.DataManager;
using Swertres.Web.Models.ViewModels;

namespace Swertres.Web.Controllers
{
    public class EncodingController : Controller
    {
        EncodingManager _em;
        AdminManager _am;
        public EncodingController()
        {
            _em = new EncodingManager();
            _am = new AdminManager();
        }

        // GET: Encoding
        public ActionResult Encode()
        {
            var drawModel = _am.GetActiveDraw();
            var sourceModel = _am.GetSource();

            var dto = new EncodeDetailModel { Bet = new EncodeModel(), Draw = drawModel, Coordinators = new SourceModelValidation { Sources = sourceModel } };
            return View(dto);
        }

        [HttpPost]
        public ActionResult Encode(EncodeDetailModel dto)
        {
            var drawModel = _am.GetActiveDraw();
            var sourceModel = _am.GetSource();

            var dtoDefualt = new EncodeDetailModel { Bet = new EncodeModel(), Draw = drawModel, Coordinators = new SourceModelValidation { Sources = sourceModel } };
            if (ModelState.IsValid)
            {
                _em.AddBet(dto);
                SigHub.SummaryHub.Update();
            }

            return View(dtoDefualt);
        }

        public ActionResult EncodePartial(long drawID,long userID =0, long sourceID = 0)
        {
            long currentUserID = (userID > 0)? userID: CurrentUser.ID;

            var dto = _em.GetBetList(drawID, currentUserID, sourceID);
            return PartialView(dto);
        }

        public ActionResult BetSummary()
        {
            var dto = _am.GetActiveDraw();
            return View(dto);
        }

        public ActionResult BetSummaryPartial(long drawID)
        {
            var dto = _em.GetOverAllBetSummary(drawID);
            return PartialView(dto);
        }

        public ActionResult BetExcessSummaryPartial(long drawID)
        {
            var dto = _em.GetOverAllBetSummary(drawID);
            return PartialView(dto);
        }

        public ActionResult HasActiveDraw()
        {
            var result = _am.GetActiveDraw();
            if(result.ControlNumber.Equals(string.Empty))
                return Json(new { success = false });

            return Json(new { success = true });
        }
    }
}