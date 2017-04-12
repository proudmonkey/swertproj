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

        #region Encoding
        // GET: Encoding
        [Authorize]
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
            if (dto.Bet.AmountRumble == null && dto.Bet.AmountTarget == null)
            {
                ModelState.AddModelError("", "You must enter a Target or Rumble amount.");
                return View(dtoDefualt);
            }

            if (ModelState.IsValid)
            {
                _em.AddBet(dto);
                bool isRumble = dto.Bet.AmountRumble > 0 ? true : false;

                if (dto.Bet.AmountTarget > 0)
                    SigHub.SummaryHub.UpdateTargetSummary();

                if (isRumble)
                    SigHub.SummaryHub.UpdateRumbleSummary();

                if (dto.Bet.IsDouble)
                    SigHub.SummaryHub.UpdateDoubleSummary();
            }

            return View(dtoDefualt);
        }

        public ActionResult EncodePartial(long drawID,long userID =0, long sourceID = 0, string bet = "")
        {
            long currentUserID = (userID > 0)? userID: CurrentUser.ID;

            var dto = _em.GetBetList(drawID, currentUserID, sourceID, bet);
            return PartialView(dto);
        }

        #endregion

        #region Coordinator
        [Authorize]
        public ActionResult Coordinator()
        {

            var drawModel = _am.GetActiveDraw();
            var sourceModel = _am.GetSource();

            var dto = new EncodeDetailModel { Bet = new EncodeModel(), Draw = drawModel, Coordinators = new SourceModelValidation { Sources = sourceModel } };
            return View(dto);
        }

        public ActionResult CoordinatorPartial(long drawID, long sourceID = 0, string bet = "")
        {
            var dto = _em.GetOverAllBetList(drawID, sourceID, bet);
            return PartialView(dto);
        }
        #endregion

        #region Summary
        [Authorize]
        public ActionResult BetSummary()
        {
            var dto = _am.GetActiveDraw();
            return View(dto);
        }

        public ActionResult TargetBetSummaryPartial(long drawID)
        {
            var dto = _em.GetBetSummary(drawID, EncodingManager.BetType.Target);
            return PartialView(dto);
        }

        public ActionResult RumbleBetSummaryPartial(long drawID)
        {
            var dto = _em.GetBetSummary(drawID, EncodingManager.BetType.Rumble);
            return PartialView(dto);
        }

        public ActionResult DoubleBetSummaryPartial(long drawID)
        {
            var dto = _em.GetBetSummary(drawID, EncodingManager.BetType.Double);
            return PartialView(dto);
        }

        public ActionResult BetTargetExcessSummaryPartial(long drawID)
        {
            var dto = _em.GetBetSummary(drawID, EncodingManager.BetType.Target).Where(o => o.Excess > 0).ToList();
            return PartialView(dto);
        }

        public ActionResult BetRumbleExcessSummaryPartial(long drawID)
        {
            var dto = _em.GetBetSummary(drawID, EncodingManager.BetType.Rumble).Where(o => o.Excess > 0).ToList();
            return PartialView(dto);
        }

        #endregion

        public ActionResult HasActiveDraw()
        {
            var result = _am.GetActiveDraw();
            if(result.ControlNumber.Equals(string.Empty))
                return Json(new { success = false });

            return Json(new { success = true });
        }
    }
}