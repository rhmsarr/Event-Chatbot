namespace EventChatbot.Models{

    public static class UserRepository{

        public static User user { get; set; }
        public static void SaveUser(User u){
            user = u;
        }
        public static void InitializeWeek(){
            
            user.Availability[0].Name = "Monday";
            user.Availability[1].Name = "Tuesday";
            user.Availability[2].Name = "Wednesday";
            user.Availability[3].Name = "Thursday";
            user.Availability[4].Name = "Friday";
            user.Availability[5].Name = "Saturday";
            user.Availability[6].Name = "Sunday";
        }
        
    }
}