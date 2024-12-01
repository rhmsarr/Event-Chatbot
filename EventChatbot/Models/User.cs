namespace EventChatbot.Models{

    public class User{
        public string Name { get; set; } = string.Empty;
        public List<string>? Interests { get; set; }

        public Week? Availability {get; set;}

        //public Day[]? Availability { get; set; }
    }
}