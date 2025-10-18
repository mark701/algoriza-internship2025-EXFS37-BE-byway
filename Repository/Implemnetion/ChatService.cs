using Domain.Models.DataBase.Chat;
using Domain.Models.Requests.ChatDTO;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implemnetion
{
    public class ChatService : IChatService 
    {
        private readonly ApplicationDbContext _context;

        public ChatService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<MessageDto> SaveMessage(int senderID, SendMessageDto messageDto)
        {
            var message = new ChatMessage
            {
                SenderID = senderID,
                ReceiverID = messageDto.ReceiverID,
                MessageText = messageDto.MessageText,
                IsRead = false,
                SentDateTime = DateTime.UtcNow
            };

            _context.chatMessages.Add(message);
            await _context.SaveChangesAsync();

            var sender = await _context.users.FindAsync(senderID);
            var receiver = await _context.users.FindAsync(messageDto.ReceiverID);

            return new MessageDto
            {
                MessageID = message.MessageID,
                SenderID = message.SenderID,
                SenderName = sender?.UserName,
                ReceiverID = message.ReceiverID,
                ReceiverName = receiver?.UserName,
                MessageText = message.MessageText,
                IsRead = message.IsRead,
                SentDateTime = message.SentDateTime
            };
        }

        public async Task<List<MessageDto>> GetChatHistory(int userID, int otherUserID, int skip = 0, int take = 50)
        {
            var messages = await _context.chatMessages
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => (m.SenderID == userID && m.ReceiverID == otherUserID) ||
                           (m.SenderID == otherUserID && m.ReceiverID == userID))
                .OrderByDescending(m => m.SentDateTime)
                .Skip(skip)
                .Take(take)
                .Select(m => new MessageDto
                {
                    MessageID = m.MessageID,
                    SenderID = m.SenderID,
                    SenderName = m.Sender.UserName,
                    ReceiverID = m.ReceiverID,
                    ReceiverName = m.Receiver.UserName,
                    MessageText = m.MessageText,
                    IsRead = m.IsRead,
                    SentDateTime = m.SentDateTime
                })
                .ToListAsync();

            return messages.OrderBy(m => m.SentDateTime).ToList();
        }

        public async Task<List<ChatHistoryDto>> GetUserChats(int userID)
        {
            var allMessages = await _context.chatMessages
                .Where(m => m.SenderID == userID || m.ReceiverID == userID)
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .ToListAsync();

            var userIDs = allMessages
                .Select(m => m.SenderID == userID ? m.ReceiverID : m.SenderID)
                .Distinct()
                .ToList();

            var onlineUsers = await _context.userConnections
                .Select(uc => uc.UserID)
                .Distinct()
                .ToListAsync();

            var chatList = new List<ChatHistoryDto>();

            foreach (var otherUserID in userIDs)
            {
                var userMessages = allMessages
                    .Where(m => (m.SenderID == userID && m.ReceiverID == otherUserID) ||
                               (m.SenderID == otherUserID && m.ReceiverID == userID))
                    .OrderByDescending(m => m.SentDateTime)
                    .ToList();

                var lastMessage = userMessages.FirstOrDefault();
                var unreadCount = userMessages.Count(m => m.ReceiverID == userID && !m.IsRead);

                var otherUser = await _context.users.FindAsync(otherUserID);

                chatList.Add(new ChatHistoryDto
                {
                    UserID = otherUserID,
                    FirstName = otherUser?.FirstName,
                    LastName = otherUser?.LastName,

                    LastMessage = lastMessage?.MessageText,
                    LastMessageTime = lastMessage?.SentDateTime,
                    UnreadCount = unreadCount,
                    IsOnline = onlineUsers.Contains(otherUserID)
                });
            }

            return chatList.OrderByDescending(c => c.LastMessageTime).ToList();
        }

        public async Task MarkMessagesAsRead(int userID, int senderID)
        {
            var unreadMessages = await _context.chatMessages
                .Where(m => m.ReceiverID == userID && m.SenderID == senderID && !m.IsRead)
                .ToListAsync();

            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<int> GetUnreadCount(int userID)
        {
            return await _context.chatMessages
                .CountAsync(m => m.ReceiverID == userID && !m.IsRead);
        }

        public async Task AddConnection(string connectionID, int userID)
        {
            var connection = new UserConnection
            {
                ConnectionID = connectionID,
                UserID = userID,
                ConnectedAt = DateTime.UtcNow
            };

            _context.userConnections.Add(connection);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveConnection(string connectionID)
        {
            var connection = await _context.userConnections.FindAsync(connectionID);
            if (connection != null)
            {
                _context.userConnections.Remove(connection);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<string>> GetUserConnections(int userID)
        {
            return await _context.userConnections
                .Where(uc => uc.UserID == userID)
                .Select(uc => uc.ConnectionID)
                .ToListAsync();
        }

        public async Task<List<OnlineUserDto>> GetOnlineUsers()
        {
            var onlineUserIDs = await _context.userConnections
                .Select(uc => uc.UserID)
                .Distinct()
                .ToListAsync();

            var users = await _context.users
                .Where(u => onlineUserIDs.Contains(u.UserID))
                .Select(u => new OnlineUserDto
                {
                    UserID = u.UserID,
                    UserName = u.UserName,
                    FirstName = u.FirstName,
                    LastName = u.LastName
                })
                .ToListAsync();

            return users;
        }

        public async Task<UserConnection> GetConnectionEntity(string connectionID)
        {
            return await _context.userConnections.FindAsync(connectionID);
        }

        public async Task UpdateConnection(UserConnection connection)
        {
            _context.userConnections.Update(connection);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsUserOnline(int userID)
        {
            return await _context.userConnections.AnyAsync(uc => uc.UserID == userID);
        }




    }
}
