using Domain.Models.DataBase.Chat;
using Domain.Models.DataBase.UserPersona;
using Domain.Models.Requests.ChatDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IChatService 
    {
        Task<MessageDto> SaveMessage(int senderID, SendMessageDto messageDto);
        Task<List<MessageDto>> GetChatHistory(int userID, int otherUserID, int skip = 0, int take = 50);
        Task<List<ChatHistoryDto>> GetUserChats(int userID);
        Task MarkMessagesAsRead(int userID, int senderID);
        Task<int> GetUnreadCount(int userID);
        Task AddConnection(string connectionID, int userID);
        Task RemoveConnection(string connectionID);
        Task<List<string>> GetUserConnections(int userID);
        Task<List<OnlineUserDto>> GetOnlineUsers();
        Task<bool> IsUserOnline(int userID);
        Task<UserConnection> GetConnectionEntity(string connectionID);
        Task UpdateConnection(UserConnection connection);
    }
}
