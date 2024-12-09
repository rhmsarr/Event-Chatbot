using Microsoft.AspNetCore.Mvc;
using EventChatbot.Models;
using Newtonsoft.Json;  

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

            ViewBag.chatHistory = messageRepository.getMessages(); //passes all the previous messages to the view

            return View();
        }
        [HttpPost]
      
    private static MessageRepository messageRepository = new MessageRepository();
  
    private void loadEvents() // it accesses the data in the event file, though it is still not used for providing a response
    {
        string eventsFilePath = Path.Combine(Directory.GetCurrentDirectory(),"data/events_data.json");
        string json = System.IO.File.ReadAllText(eventsFilePath);
    }
    
    public IActionResult SendMessage(string message)
    {
       messageRepository.addMessage("User", message); //saves the user's input in a list
       
       loadEvents();// once integrated properly it should allow the chatbot to access the data file in order to provide a response based on the user's message
       
       string response = GenerateResponse(); 
       
       messageRepository.addMessage("Bot", response); // saves the chatbot's response in the list
    
       ViewBag.chatHistory = messageRepository.getMessages(); //gets both the old and the new messages 
    }
      [HttpPost]
        public IActionResult Index(User model) {  
            // Action method that takes a `User` model as input.
            // The purpose is to process user data, save it, and return a view.

            // Create a new anonymous object to structure the user data.
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

       return View("Index");
    }
    
    private string GenerateResponse() // this method is to make the chatbot generate a response based on only the last message sent
    {
        string lastMessage = messageRepository.getLastMessage().Content.ToLower();
        return $"You said: {lastMessage}. This is a placeholder response."; //forget this line i only added so the code won't give an error
        
    }
    

   
    }
}