using Bonded.Models;
using Microsoft.AspNetCore.Mvc;
using Bonded.Models.ViewModels;
using Bonded.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Runtime.Intrinsics.X86;
namespace Bonded.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;
        private readonly UserManager<User> _userManager;

        public ChatController(IChatRepository chatRepository,  UserManager<User> userManager)
        {
            _chatRepository = chatRepository;
            _userManager = userManager;
            
        }

        // View all chats for the current user
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";
            if (userIdValue == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User"); // Redirect if no user is logged in
            }

            var chats = await _chatRepository.GetChatAsync(userIdValue);

            List<ChatViewModel1> list = new List<ChatViewModel1>();
            foreach (var chat in chats)
            {
                var messages = _chatRepository.GetConvoForAChat(chat.Id);
                if (userIdValue == chat.UserOneId)
                {
                    list.Add(new ChatViewModel1 { ChatId = chat.Id, Messages = messages, UserONE = await _userManager.FindByIdAsync(userIdValue), UserTWO = await _userManager.FindByIdAsync(chat.UserTwoId) });
                }
                else 
                {
                    list.Add(new ChatViewModel1 { ChatId = chat.Id, Messages = messages, UserONE = await _userManager.FindByIdAsync(chat.UserTwoId), UserTWO =await _userManager.FindByIdAsync(chat.UserOneId) });
                }
            }
          
            return View(list);
        }

        // View a specific chat
        [HttpGet]
        public async Task<IActionResult> ChatView(string receiverId)
        {
            string? userId = HttpContext.Session.GetString("UserId");
            string userIdValue = userId ?? "";

            if (userIdValue == "")

            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }
            //make a viewmodel

            
            Chat chat = _chatRepository.GetOrCreateChat(userIdValue, receiverId);
            List<Convo> messages =  _chatRepository.GetConvoForAChat(chat.Id);
           
            List<ConvoViewModel> cvm = new List<ConvoViewModel>();
            if (messages != null)
            {
                foreach (Convo message in messages)
                {
                    if (message != null)
                    {
                        cvm.Add(new ConvoViewModel { Message = message, Receiver = await _userManager.FindByIdAsync(message.ReceiverId), Sender = await _userManager.FindByIdAsync(message.SenderId) });
                    }
                }
            }

            ChatViewModel chatViewModel = new ChatViewModel
            {
                ChatId = chat.Id,
                UserONE = await _userManager.FindByIdAsync(chat.UserOneId),
                UserTWO = await _userManager.FindByIdAsync(chat.UserTwoId),
                Messages = cvm
            };


            if (chatViewModel == null)
            {
                return NotFound("Chat not found");
            }
            
            ViewBag.CurrentUserId = userIdValue;
            ViewBag.ReceiverId = receiverId;
            var user1 = await _userManager.FindByIdAsync(receiverId);
            ViewBag.ProfilePicture = user1.ProfilePicture;
            ViewBag.Username = user1.UserName;
            return View(chatViewModel);
        }

        // Send a new message
        [HttpPost]
        public async Task<IActionResult> SendMessage(int chatId, string receiverId, string text)
        {

            string? userId = HttpContext.Session.GetString("UserId");
            string senderId = userId ?? "";
            if (senderId == "")
            {
                TempData["ErrorMessage"] = "Signin to view more!";
                return RedirectToAction("Login", "User");
            }

            if (string.IsNullOrWhiteSpace(text))
            {
                ModelState.AddModelError("Text", "Message cannot be empty.");
                return RedirectToAction("ChatView", new { receiverId });
            }

            await _chatRepository.SendMessageAsync(chatId, senderId, receiverId, text);
            return RedirectToAction("ChatView", new { receiverId });
        }
        [HttpGet]
        public IActionResult DeleteMessage(int msgId)
        {
            // Ensure the user is authorized to delete the message
            string receiverId = _chatRepository.GetMessageById(msgId).ReceiverId;
            _chatRepository.DeleteMessage(msgId);
            return RedirectToAction("ChatView", new { receiverId }); // Redirect to the chat page
        }
        [HttpGet]
        public IActionResult DeleteChat(int chatId)
        {
            _chatRepository.DeleteMessagesOfAChat(chatId);
            _chatRepository.DeleteChat(chatId);
            return RedirectToAction("Index","Chat");
            // Ensure the user is authorized to delete the message
             // Redirect to the chat page
        }

    }
}

