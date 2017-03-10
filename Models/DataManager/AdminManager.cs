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
                    User.SetCurrentUser();
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

        public void AddNewSource(CreateSourceModel dto)
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                var source = new Source()
                {
                    SourceName = dto.SourceName
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
                    return db.Sources.Select(o => new SourceModel { SourceID = o.SourceID, SourceName =o.SourceName }).ToList();
                else
                    return db.Sources.Where(o => o.SourceID == id).Select(o => new SourceModel { SourceID = o.SourceID, SourceName = o.SourceName }).ToList();
            }
        }

        public List<SourceModel> GetCoordinator()
        {
            using (SwertresEntities db = new SwertresEntities())
            {
                return db.Sources.Select(o => new SourceModel { SourceID = o.SourceID, SourceName = o.SourceName }).ToList();
            }
        }
    }
}