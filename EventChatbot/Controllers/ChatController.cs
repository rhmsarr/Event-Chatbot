using Microsoft.AspNetCore.Mvc;
using EventChatbot.Models;

namespace EventChatbot.Controllers{
    public class ChatController : Controller{
        public IActionResult Index(){
            return View();
        }
    }
}