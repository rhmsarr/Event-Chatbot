namespace EventChatbot.Models{

    public static class UserRepository{

        public static User user { get; set; }
        public static void SaveUser(User u){
            user = new User();
            user = u;
        }
        
        
    }
}