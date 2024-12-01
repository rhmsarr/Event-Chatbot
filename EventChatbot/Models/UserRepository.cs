namespace EventChatbot.Models{

    public static class UserRepository{

        public static User user { get; set; }
        public static void SaveUser(User u){
            user = new User();
            user = u;
        }
        public static void InitializeWeek(){
            
            if(user.Availability.Monday == null){
                user.Availability.Monday = new Day();
            }
            if(user.Availability.Tuesday == null){
                user.Availability.Tuesday = new Day();
            }
            if(user.Availability.Wednesday == null){
                user.Availability.Wednesday = new Day();
            }
            if(user.Availability.Thursday == null){
                user.Availability.Thursday = new Day();
            }
            if(user.Availability.Friday == null){
                user.Availability.Friday = new Day();
            }
            if(user.Availability.Saturday == null){
                user.Availability.Saturday = new Day();
            } 
            if(user.Availability.Sunday == null){
                user.Availability.Sunday = new Day();
            }
          
        }
        
    }
}