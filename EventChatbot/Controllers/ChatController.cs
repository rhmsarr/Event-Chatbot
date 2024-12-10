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
        if(message!=null)
            MessageRepository.addMessage("User", message); //saves the user's input in a list

        var tcs = new TaskCompletionSource<bool>(); //to render the view only after getting the response from the bot

        var timeoutTask = Task.Delay(15000); // 30 seconds timeout
        var responseTask = tcs.Task; // The task that will be completed once the response is received
        
        await _rabbitMqService.SendMessageAsync(message, MessageRepository.getMessages());
       
        _rabbitMqService.ListenForResponse(rep =>
                    {
                        MessageRepository.addMessage("Bot", rep);// saves the chatbot's response in the list
                        tcs.SetResult(true);
                    });


        var CompletedTask = await Task.WhenAny(responseTask, timeoutTask); //wait for the chatbot response
       
        
    
        ViewBag.chatHistory = MessageRepository.getMessages(); //gets both the old and the new messages 
        return View("Index");
    }
      [HttpPost]
        public async Task<IActionResult> Index(User model) {  
            // Action method that takes a `User` model as input.
            // The purpose is to process user data, save it, and return a view.

            if(ModelState.IsValid)
            {
               
                

                UserRepository.saveUserJson(model);

                // Save the user model to a repository (assumed to handle persistence or further processing).
                UserRepository.SaveUser(model);

                // Return the "test" view, passing the model as a parameter.
                // This view is expected to display or use ChatGPT's reply along with the user data.
                var tcs = new TaskCompletionSource<bool>(); //to render the view only after getting the response from the bot

                MessageRepository.ClearHistory();

                string message = "Greet the user and then recommend events based on the user's availability and the provided documents. Address the user directly.";
                await _rabbitMqService.SendMessageAsync(message, MessageRepository.getMessages());
            
                _rabbitMqService.ListenForResponse(rep =>
                            {
                                MessageRepository.addMessage("Bot", rep);// saves the chatbot's response in the list

                                tcs.SetResult(true);
                            });


                await tcs.Task; //wait for the chatbot response
            
                
            
                ViewBag.chatHistory = MessageRepository.getMessages(); //gets both the old and the new messages 
                return View("Index");
                        
            }
            else{
                ViewData["openModal"] = true;
                return View(model);
            }
        }

      
    

   
    }
}