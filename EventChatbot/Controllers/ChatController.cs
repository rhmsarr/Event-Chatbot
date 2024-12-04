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
            return View();
        }
        [HttpPost]
       public IActionResult Index(User model) {  
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


    }
}