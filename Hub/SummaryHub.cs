using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace Swertres.Web.SigHub
{
    public class SummaryHub : Hub
    {
        public static void UpdateTargetSummary()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SummaryHub>();
            context.Clients.All.updateTargetSummaryData();
        }

        public static void UpdateRumbleSummary()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SummaryHub>();
            context.Clients.All.updateRumbleSummaryData();
        }

        public static void UpdateDoubleSummary()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SummaryHub>();
            context.Clients.All.updateDoubleSummaryData();
        }
    }
}