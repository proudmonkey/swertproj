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
        public void Hello()
        {
            Clients.All.hello();
        }

        public static void Update()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SummaryHub>();
            context.Clients.All.updateSummaryData();
        }
    }
}