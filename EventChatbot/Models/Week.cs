namespace EventChatbot.Models{
    public class Week{
        public Day? Monday {get; set;}
        public Day? Tuesday {get; set;}
        public Day? Wednesday {get; set;}
        public Day? Thursday {get; set;}
        public Day? Friday {get; set;}
        public Day? Saturday {get; set;}
        public Day? Sunday {get; set;}
        public Week(){
            Monday = new Day();
            Tuesday = new Day();
            Wednesday = new Day();
            Thursday = new Day();
            Friday = new Day();
            Saturday = new Day();
            Sunday = new Day();
        }
    }
    
}