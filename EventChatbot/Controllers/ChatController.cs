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
       public IActionResult Index(User model) 
    {  
    // Action method that takes a `User` model as input.
    // The purpose is to process user data, save it, and return a view.

    // Create a new anonymous object to structure the user data.
    var userData = new
    {
        Name = model.Name,  // Include the user's name from the model.
        Interests = model.Interests,  // Include the user's interests from the model.
        Availability = new  // Include the user's availability details structured by days of the week.
        {
            Monday = new { AM = model.Availability.Monday.AM, PM = model.Availability.Monday.PM },  // Availability for Monday.
            Tuesday = new { AM = model.Availability.Tuesday.AM, PM = model.Availability.Tuesday.PM },  // Availability for Tuesday.
            Wednesday = new { AM = model.Availability.Wednesday.AM, PM = model.Availability.Wednesday.PM },  // Availability for Wednesday.
            Thursday = new { AM = model.Availability.Thursday.AM, PM = model.Availability.Thursday.PM },  // Availability for Thursday.
            Friday = new { AM = model.Availability.Friday.AM, PM = model.Availability.Friday.PM },  // Availability for Friday.
            Saturday = new { AM = model.Availability.Saturday.AM, PM = model.Availability.Saturday.PM },  // Availability for Saturday.
            Sunday = new { AM = model.Availability.Sunday.AM, PM = model.Availability.Sunday.PM }  // Availability for Sunday.
        }
    };

        // Serialize the structured user data to a JSON string with indentation for readability.
        string json = JsonConvert.SerializeObject(userData, Formatting.Indented);

        // Define the file path where the JSON data will be saved.
        string filepath = Path.Combine(Directory.GetCurrentDirectory(), "data/user_data.txt");

        // Write the JSON data to the specified file path.
        System.IO.File.WriteAllText(filepath, json);

        // Save the user model to a repository (assumed to handle persistence or further processing).
        UserRepository.SaveUser(model);

        // Return the "test" view, passing the model as a parameter.
        // This view is expected to display or use ChatGPT's reply along with the user data.
        return View("test", model);
    }

     private static MessageRepository messageRepository = new MessageRepository();
  
    private void loadEvents() // it accesses the data in the event file, though it is still not used for providing a response
    {
        string eventsFilePath = Path.Combine(Directory.GetCurrentDirectory(),"data/events_data.json");
        string json = System.IO.File.ReadAllText(eventsFilePath);
    }
    [HttpPost]
    public IActionResult SendMessage(string message)
    {
       messageRepository.addMessage("User", message); //saves the user's input in a list
       
       loadEvents();// once integrated properly it should allow the chatbot to access the data file in order to provide a response based on the user's message
       
       string response = GenerateResponse(); 
       
       messageRepository.addMessage("Bot", response); // saves the chatbot's response in the list
    
       ViewBag.chatHistory = messageRepository.getMessages(); //gets both the old and the new messages 

       return View("Index");
    }
    
    private string GenerateResponse() // this method is to make the chatbot generate a response based on only the last message sent
    {
        string lastMessage = messageRepository.getLastMessage().Content.ToLower();
        return $"You said: {lastMessage}. This is a placeholder response."; //forget this line i only added so the code won't give an error
        
    }
    

   
    }
}