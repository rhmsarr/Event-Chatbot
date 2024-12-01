using Microsoft.AspNetCore.Mvc;
using EventChatbot.Models;

namespace EventChatbot.Controllers{
    public class ChatController : Controller{

        [HttpGet]
        public IActionResult Index(){
            bool openModalOnLoad= false;
            //if the user never entered their information, show the modal first
            if(UserRepository.user == null){
                openModalOnLoad = true;
            }
            ViewData["openModal"] = openModalOnLoad;
            return View();
        }
        [HttpPost]
        public IActionResult Index(User model){
            //send model to chatgpt
            
            UserRepository.SaveUser(model);
            //UserRepository.InitializeWeek();
            return View("test", model);//return the view with chatgpt's reply

        }

    }
}