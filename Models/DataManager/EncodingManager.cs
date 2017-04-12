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
        public enum BetType {
               Target = 1,
               Rumble =2,
               Double = 3
        }

        public IEnumerable<EncodeModel> GetAllBet(long drawID)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var bets = db.Bets.Where(o => o.DrawID.Equals(drawID));
                if (bets.Any())
                    return bets.Select(o => new EncodeModel {
                        BetID = o.BetID,
                        Number = o.Number,
                        AmountTarget = o.AmountTarget,
                        AmountRumble = o.AmountRumble,
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
                    AmountTarget = dto.Bet.AmountTarget,
                    AmountRumble = dto.Bet.AmountRumble,
                    IsRumble = dto.Bet.AmountRumble > 0? true: false,
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

        public BetListModel GetBetList(long drawID, long userID, long sourceID, string betNumber)
        {
            BetListModel res = new BetListModel();
            using (SwertresEntities db = new SwertresEntities())
            {
                if(sourceID > 0)
                {
                    var encoderData = (from bet in db.Bets
                                       join source in db.Sources on bet.SourceID equals source.SourceID
                                       where bet.DrawID == drawID && bet.RowCreatedUserID == userID && bet.SourceID == sourceID
                                       select new BetModel
                                       {
                                           BetID = bet.BetID,
                                           Number = bet.Number,
                                           AmountTarget = bet.AmountTarget,
                                           AmountRumble = bet.AmountRumble,
                                           IsDouble = bet.IsDouble,
                                           IsRumble = bet.IsRumble,
                                           SourceName = source.SourceName
                                       });

                    var coordinator = db.Sources.Where(o => o.SourceID.Equals(sourceID)).First();

                    var total = encoderData.Sum(o => o.AmountTarget) + encoderData.Sum(o => o.AmountRumble);
                    decimal share =(Convert.ToDecimal(coordinator.Share) / 100m) * Convert.ToDecimal(total);
                    var net = total - share;

                    res.Total = total;
                    res.Net = net;
                    res.Share = share;

                    if (betNumber != string.Empty)
                    {
                        res.BetList = encoderData.Where(o => o.Number.Equals(betNumber)).ToList();
                        return res;
                    }


                    res.BetList = encoderData.OrderByDescending(o => o.BetID).ToList();
                    return res;
                }

                var adminData = (from bet in db.Bets
                        join source in db.Sources on bet.SourceID equals source.SourceID
                        where bet.DrawID == drawID && bet.RowCreatedUserID == userID
                        select new BetModel
                        {
                            BetID = bet.BetID,
                            Number = bet.Number,
                            AmountTarget = bet.AmountTarget,
                            AmountRumble = bet.AmountRumble,
                            IsDouble = bet.IsDouble,
                            IsRumble = bet.IsRumble,
                            SourceName = source.SourceName
                        });

                var totalOverAll = adminData.Sum(o => o.AmountTarget) + adminData.Sum(o => o.AmountRumble);
                res.Total = totalOverAll;
                res.Net = 0;
                res.Share = 0;

                if (betNumber != string.Empty)
                {
                    res.BetList = adminData.Where(o => o.Number.Equals(betNumber)).ToList();
                    return res;
                }

                res.BetList = adminData.OrderByDescending(o => o.BetID).ToList();

                return res;
            }
        }

        public List<BetSummaryModel> GetBetSummary(long drawID, BetType type)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                switch (type)
                {
                    case BetType.Target:
                        {
                            return db.GetTargetBetSummary(drawID).Select(o => new BetSummaryModel
                            {
                                Number = o.Number.ToString().PadLeft(3, '0'),
                                Total = o.Total,
                                Excess = o.TotalExcess
                            }).ToList();

                        }
                    case BetType.Rumble:
                        {
                            return db.GetRumbleBetSummary(drawID).Select(o => new BetSummaryModel
                            {
                                Number = o.Number.ToString().PadLeft(3, '0'),
                                Total = o.Total,
                                Excess = o.TotalExcess,
                                IsDouble = o.IsDouble
                            }).ToList();

                        }

                    case BetType.Double:
                        {
                            return db.GetDoubleBetSummary(drawID).Select(o => new BetSummaryModel
                            {
                                Number = o.Number.ToString().PadLeft(3,'0'),
                                Total = o.Total,
                                Excess = o.TotalExcess
                            }).ToList();

                        }

                    default:
                        {
                            return new List<BetSummaryModel>();
                        }
                }
            }
        }

        public BetListModel GetOverAllBetList(long drawID, long sourceID, string betNumber)
        {
            BetListModel res = new BetListModel();
            using (SwertresEntities db = new SwertresEntities())
            {
                if (sourceID > 0)
                {
                    var encoderData = (from bet in db.Bets
                                       join source in db.Sources on bet.SourceID equals source.SourceID
                                       join user in db.Users on bet.RowCreatedUserID equals user.UserID
                                       where bet.DrawID == drawID && bet.SourceID == sourceID
                                       select new BetModel
                                       {
                                           BetID = bet.BetID,
                                           Number = bet.Number,
                                           AmountTarget = bet.AmountTarget,
                                           AmountRumble = bet.AmountRumble,
                                           IsDouble = bet.IsDouble,
                                           IsRumble = bet.IsRumble,
                                           SourceName = source.SourceName,
                                           EncoderName = user.FirstName
                                       });

                    var coordinator = db.Sources.Where(o => o.SourceID.Equals(sourceID)).First();

                    var total = encoderData.Sum(o => o.AmountTarget) + encoderData.Sum(o => o.AmountRumble);
                    decimal share = (Convert.ToDecimal(coordinator.Share) / 100m) * Convert.ToDecimal(total);
                    var net = total - share;

                    res.Total = total;
                    res.Net = net;
                    res.Share = share;

                    if (betNumber != string.Empty)
                    {
                        res.BetList = encoderData.Where(o => o.Number.Equals(betNumber)).ToList();
                        return res;
                    }


                    res.BetList = encoderData.OrderByDescending(o => o.BetID).ToList();
                    return res;
                }

                var adminData = (from bet in db.Bets
                                 join source in db.Sources on bet.SourceID equals source.SourceID
                                 join user in db.Users on bet.RowCreatedUserID equals user.UserID
                                 where bet.DrawID == drawID
                                 select new BetModel
                                 {
                                     BetID = bet.BetID,
                                     Number = bet.Number,
                                     AmountTarget = bet.AmountTarget,
                                     AmountRumble = bet.AmountRumble,
                                     IsDouble = bet.IsDouble,
                                     IsRumble = bet.IsRumble,
                                     SourceName = source.SourceName,
                                     EncoderName = user.FirstName
                                 });

                var totalOverAll = adminData.Sum(o => o.AmountTarget) + adminData.Sum(o => o.AmountRumble);
                res.Total = totalOverAll;
                res.Net = 0;
                res.Share = 0;

                if (betNumber != string.Empty)
                {
                    res.BetList = adminData.Where(o => o.Number.Equals(betNumber)).ToList();
                    return res;
                }

                res.BetList = adminData.OrderByDescending(o => o.BetID).ToList();

                return res;
            }
        }
    }
}