using Domain.Models.Requests.ChatDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly IAuthService _AuthService;

        public ChatHub(IChatService chatService, IAuthService authService)
        {
            _chatService = chatService;
            _AuthService = authService;
        }   

        public override async Task OnConnectedAsync()
        {
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            int userID = 0;
            if (int.TryParse(userStringID, out int IntID))
            {
                userID= IntID;
            }
            if (userID!=0)
            {
                await _chatService.AddConnection(Context.ConnectionId, userID);

                // Notify all users that this user is online
                await Clients.All.SendAsync("UserOnline", userID);

                // Send online users list to the newly connected user
                var onlineUsers = await _chatService.GetOnlineUsers();
                await Clients.Caller.SendAsync("OnlineUsers", onlineUsers);
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            int userID = 0;
            if (int.TryParse(userStringID, out int IntID))
            {
                userID = IntID;
            }
            await _chatService.RemoveConnection(Context.ConnectionId);

            if (userID!=0)
            {
                // Check if user still has other connections
                var connections = await _chatService.GetUserConnections(userID);
                if (connections.Count == 0)
                {
                    // User is completely offline
                    await Clients.All.SendAsync("UserOffline", userID);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(SendMessageDto messageDto)
        {
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            int senderID = 0;
            if (int.TryParse(userStringID, out int IntID))
            {
                senderID = IntID;
            }
            if (senderID==0)
            {
                await Clients.Caller.SendAsync("Error", "User not authenticated");
                return;
            }

            try
            {
                // Save message to database
                var savedMessage = await _chatService.SaveMessage(senderID, messageDto);

                // Get receiver's connections
                var receiverConnections = await _chatService.GetUserConnections(messageDto.ReceiverID);

                // Send message to all receiver's connections
                foreach (var connectionID in receiverConnections)
                {
                    await Clients.Client(connectionID).SendAsync("ReceiveMessage", savedMessage);
                }

                // Send confirmation back to sender
                await Clients.Caller.SendAsync("MessageSent", savedMessage);
            }
            catch (Exception ex)
            {
                await Clients.Caller.SendAsync("Error", $"Failed to send message: {ex.Message}");
            }
        }

        public async Task MarkAsRead(int senderID)
        {
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            int userID = 0;
            if (int.TryParse(userStringID, out int IntID))
            {
                userID = IntID;
            }
            if (userID==0) return;

            await _chatService.MarkMessagesAsRead(userID, senderID);

            // Notify sender that messages were read
            var senderConnections = await _chatService.GetUserConnections(senderID);
            foreach (var connectionID in senderConnections)
            {
                await Clients.Client(connectionID).SendAsync("MessagesRead", userID);
            }
        }

        public async Task Typing(int receiverID)
        {
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            int senderID = 0;
            if (int.TryParse(userStringID, out int IntID))
            {
                senderID = IntID;
            }
            if (senderID==0) return;

            var receiverConnections = await _chatService.GetUserConnections(receiverID);
            foreach (var connectionID in receiverConnections)
            {
                await Clients.Client(connectionID).SendAsync("UserTyping", senderID);
            }
        }

        public async Task StopTyping(int receiverID)
        {
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            int senderID = 0;
            if (int.TryParse(userStringID, out int IntID))
            {
                senderID = IntID;
            }
            if (senderID == 0) return;

            var receiverConnections = await _chatService.GetUserConnections(receiverID);
            foreach (var connectionID in receiverConnections)
            {
                await Clients.Client(connectionID).SendAsync("UserStoppedTyping", senderID);
            }
        }

        public async Task Ping()
        {
            // Get current user ID from claims
            var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userStringID, out int userID)) return;

            // Find the connection in the database
            var connection = await _chatService.GetUserConnections(userID);

            // If the current connection exists, update its ConnectedAt timestamp
            var connEntity = await _chatService.GetConnectionEntity(Context.ConnectionId);
            if (connEntity != null)
            {
                connEntity.ConnectedAt = DateTime.UtcNow;
                await _chatService.UpdateConnection(connEntity);
            }
        }


    }

}
