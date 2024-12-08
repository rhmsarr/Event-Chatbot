using System.ComponentModel.DataAnnotations;

namespace EventChatbot.Models{


    public class User{

        [Required(ErrorMessage = "Please enter your name")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage="Select at least one")]
        public List<string>? Interests { get; set; }
        [Required(ErrorMessage="Select at least one")]
        public Week? Availability {get; set;}

    }
}