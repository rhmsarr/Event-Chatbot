using Microsoft.AspNetCore.Mvc;
using EventChatbot.Models;
using Newtonsoft.Json;  
using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventChatbot.Controllers{
    public class ChatController : Controller{

        private readonly RabbitMqService _rabbitMqService;

        public ChatController(RabbitMqService rabbitMqService){
            _rabbitMqService = rabbitMqService;
        }

        [HttpGet]
        public IActionResult Index(){
            bool openModalOnLoad= false;
            //if the user never entered their information, show the modal first
            if(UserRepository.user == null){
                openModalOnLoad = true;
            }
            ViewData["openModal"] = openModalOnLoad;

            ViewBag.chatHistory = MessageRepository.getMessages(); //passes all the previous messages to the view

            return View();
        }
        
      
  
   

   [HttpPost] 
    public async Task<IActionResult> GetResponse(string message)
    {
        MessageRepository.addMessage("User", message); //saves the user's input in a list
        
        await _rabbitMqService.SendMessageAsync(message, MessageRepository.getMessages());
       
        _rabbitMqService.ListenForResponse(rep =>
                    {
                        MessageRepository.addMessage("Bot", rep);// saves the chatbot's response in the list
                    });

      
       
        
    
        ViewBag.chatHistory = MessageRepository.getMessages(); //gets both the old and the new messages 
        return View("Index");
    }

      [HttpPost]
        public IActionResult Index(User model) {  
            // Action method that takes a `User` model as input.
            // The purpose is to process user data, save it, and return a view.

            if(ModelState.IsValid)
            {
               
                

                UserRepository.saveUserJson(model);

                // Save the user model to a repository (assumed to handle persistence or further processing).
                UserRepository.SaveUser(model);

                // Return the "test" view, passing the model as a parameter.
                // This view is expected to display or use ChatGPT's reply along with the user data.
                return View("test", model);
            }
            else{
                ViewData["openModal"] = true;
                return View(model);
            }
        }

       
    
    private string GenerateResponse() // this method is to make the chatbot generate a response based on only the last message sent
    {
        string lastMessage = MessageRepository.getLastMessage().Content.ToLower();
        return $"You said: {lastMessage}. This is a placeholder response."; //forget this line i only added so the code won't give an error
        
    }
    

   
    }
}