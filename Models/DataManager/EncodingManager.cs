using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Swertres.Web.Models.DB;
using Swertres.Web.Models.ViewModels;

namespace Swertres.Web.Models.DataManager
{
    public class EncodingManager
    {
        public IEnumerable<EncodeModel> GetAllBet(long drawID)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var bets = db.Bets.Where(o => o.DrawID.Equals(drawID));
                if (bets.Any())
                    return bets.Select(o => new EncodeModel {
                        BetID = o.BetID,
                        Number = o.Number,
                        Amount = o.Amount,
                        IsRumble = o.IsRumble,
                        IsDouble = o.IsDouble,
                    });
                else
                    return Enumerable.Empty<EncodeModel>();
            }
        }

        public void AddBet(EncodeDetailModel dto)
        {
            User.SetCurrentUser();

            using (SwertresEntities db = new SwertresEntities())
            {
                Bet newBet = new Bet()
                {
                    Number = dto.Bet.Number,
                    Amount = dto.Bet.Amount,
                    IsRumble = dto.Bet.IsRumble,
                    IsDouble = dto.Bet.IsDouble,
                    SourceID = dto.Coordinators.SelectedSourceID,
                    DrawID = dto.Draw.DrawID,
                    RowCreatedUserID = CurrentUser.ID,
                    RowCreatedDateTime = DateTime.Now 
                };

                db.Bets.Add(newBet);
                db.SaveChanges();
            }
        }

        public List<BetModel> GetBetList(long drawID, long userID, long sourceID)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                if(sourceID > 0)
                {
                    return (from bet in db.Bets
                            join source in db.Sources on bet.SourceID equals source.SourceID
                            where bet.DrawID == drawID && bet.RowCreatedUserID == userID && bet.SourceID == sourceID
                            select new BetModel
                            {
                                BetID = bet.BetID,
                                Number = bet.Number,
                                Amount = bet.Amount,
                                IsDouble = bet.IsDouble,
                                IsRumble = bet.IsRumble,
                                SourceName = source.SourceName
                            }).ToList();
                }

                return (from bet in db.Bets
                             join source in db.Sources on bet.SourceID equals source.SourceID
                             where bet.DrawID == drawID && bet.RowCreatedUserID == userID
                             select new BetModel
                             {
                                 BetID = bet.BetID,
                                 Number = bet.Number,
                                 Amount = bet.Amount,
                                 IsDouble = bet.IsDouble,
                                 IsRumble = bet.IsRumble,
                                 SourceName = source.SourceName
                             }).ToList();

            }
        }

        public List<BetSummaryModel> GetOverAllBetSummary(long drawID)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                return db.GetBetOverAllSummary(drawID).Select(o => new BetSummaryModel
                {
                    Number = o.Number,
                    TotalAmountNormal = o.TotalAmountNormal,
                    TotalAmountRumble = o.TotalAmountRumble,
                    TotalAmountNormalExcess = o.TotalAmountNormalExcess,
                    TotalAmountRumbleExcess = o.TotalAmountRumbleExcess
                }).ToList(); 
            }
        }
    }
}