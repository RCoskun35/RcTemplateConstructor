using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.HUB
{
    [AllowAnonymous]
    public class ExampleHub : Hub
    {
        public async Task ExampleNotification(string message)
        {
            await Clients.All.SendAsync(message);
        }
    }
}
