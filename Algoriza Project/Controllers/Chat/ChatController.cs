using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interface;
using System.Security.Claims;

namespace AlgorizaProject.Controllers.Chat
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _AuthService;

        public ChatController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _AuthService = authService;
        }

        [HttpGet("history/{otherUserID}")]
        public async Task<IActionResult> GetChatHistory(int otherUserID, [FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            try
            {
                var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
                int userID = 0;
                if (int.TryParse(userStringID, out int IntID))
                {
                    userID = IntID;
                }
                var messages = await _unitOfWork.ChatService.GetChatHistory(userID, otherUserID, skip, take);
                return Ok(messages);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetUserChats()
        {
            try
            {
                var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
                int userID = 0;
                if (int.TryParse(userStringID, out int IntID))
                {
                    userID = IntID;
                }
                var chats = await _unitOfWork.ChatService.GetUserChats(userID);
                return Ok(chats);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnreadCount()
        {
            try
            {
                var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
                int userID = 0;
                if (int.TryParse(userStringID, out int IntID))
                {
                    userID = IntID;
                }
                var count = await _unitOfWork.ChatService.GetUnreadCount(userID);
                return Ok(new { count });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("mark-read/{senderID}")]
        public async Task<IActionResult> MarkMessagesAsRead(int senderID)
        {
            try
            {
                var userStringID = _AuthService.GetClaim(ClaimTypes.NameIdentifier);
                int userID = 0;
                if (int.TryParse(userStringID, out int IntID))
                {
                    userID = IntID;
                }
                await _unitOfWork.ChatService.MarkMessagesAsRead(userID, senderID);
                return Ok(new { message = "Messages marked as read" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("online-users")]
        public async Task<IActionResult> GetOnlineUsers()
        {
            try
            {
                var users = await _unitOfWork.ChatService.GetOnlineUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("user-online/{userID}")]
        public async Task<IActionResult> IsUserOnline(int userID)
        {
            try
            {
                var isOnline = await _unitOfWork.ChatService.IsUserOnline(userID);
                return Ok(new { isOnline });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

      
    }
}

