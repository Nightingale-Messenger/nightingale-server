using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs
{
    class UserHub : Hub
    {
        [Authorize]
        public async Task Send(string message, string to)
        {
            var user = Context.User;
            var userName = user.Identity.Name;
        }
    }
}