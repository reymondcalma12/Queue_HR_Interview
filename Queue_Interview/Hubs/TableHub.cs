
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Queue.DataAccess.Repository;
using Queue.Models;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.SignalR;
using Queue_Interview.Extensions;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace Queue_System.Hubs
{
    public class TableHub : Hub, IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHttpContextAccessor context;
		private readonly PresenceTracker tracker;

		public TableHub(IUnitOfWork unitOfWork, IHttpContextAccessor context, PresenceTracker tracker)
        {
            this.unitOfWork = unitOfWork;
            this.context = context;
			this.tracker = tracker;
		}

        public override async Task OnConnectedAsync()
        {

            var userLogin = context.HttpContext.Session.GetString("User");

            if (userLogin != null)
            {
				var isUsed = await tracker.UserConnected(userLogin.GetUserId(), Context.ConnectionId);
				if (isUsed) await Clients.Others.SendAsync("UserIsOnline", userLogin.GetUserId());
			}

			var currentUsers = await tracker.GetOnlineUsers();
			await Clients.Caller.SendAsync("ActiveUsers", currentUsers);
            var result = await unitOfWork.Stages.GetStagesTable();
            List<Stage> filteredStage = new List<Stage>();

            if (currentUsers != null && currentUsers.Any())
			{
                foreach(var stage in result)
                {
                    if(currentUsers.Any(x => x.Split('-')[0] == stage.StageId.ToString()))
                    {
                        var cStage = stage;
                        var tables = cStage.tables;
                        cStage.tables = tables.Where(x => 
                            !currentUsers.Any(c => c.Split('-')[0] == stage.StageId.ToString() 
                                && c.Split('-')[1] == x.TableId.ToString())
                            ).ToList();
                        
                        filteredStage.Add(cStage);
                    }
                    else
                    {
                        filteredStage.Add(stage);
                    }
                }

                await Clients.Others.SendAsync("GetOfflineTables", filteredStage);
                await Clients.Caller.SendAsync("GetOfflineTables", filteredStage);
            }
			else
			{

                await Clients.Others.SendAsync("GetOfflineTables", result);
                await Clients.Caller.SendAsync("GetOfflineTables", result);
            }

            await base.OnConnectedAsync();
        }

		public override async Task OnDisconnectedAsync(Exception exception)
		{
            var userLogin = context.HttpContext.Session.GetString("User");

            if (userLogin != null)
            {
                var isOffline = await tracker.UserDisconnected(userLogin.GetUserId(), Context.ConnectionId);
                if (isOffline) await Clients.Others.SendAsync("UsersIsOffline", userLogin.GetUserId());

            }

            var currentUsers = await tracker.GetOnlineUsers();
            var result = await unitOfWork.Stages.GetStagesTable();
            List<Stage> filteredStage = new List<Stage>();

            if (currentUsers != null && currentUsers.Any())
            {
                foreach (var stage in result)
                {
                    if (currentUsers.Any(x => x.Split('-')[0] == stage.StageId.ToString()))
                    {
                        var cStage = stage;
                        var tables = cStage.tables;
                        cStage.tables = tables.Where(x =>
                            !currentUsers.Any(c => c.Split('-')[0] == stage.StageId.ToString()
                                && c.Split('-')[1] == x.TableId.ToString())
                            ).ToList();

                        filteredStage.Add(cStage);
                    }
                    else
                    {
                        filteredStage.Add(stage);
                    }
                }

                await Clients.Others.SendAsync("GetOfflineTables", filteredStage);
            }
            else
            {

                await Clients.Others.SendAsync("GetOfflineTables", result);
            }

          

            await base.OnDisconnectedAsync(exception);

        }
    } 
}
