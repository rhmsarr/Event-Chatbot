using Newtonsoft.Json;  

namespace EventChatbot.Models{

    public static class UserRepository{

        public static User user { get; set; }
        public static void SaveUser(User u){
            user = new User();
            user = u;
        }
        
        public static void saveUserJson(User u){
            var userData = new
                {
                    Name = u.Name,  // Include the user's name from the model.
                    Interests = u.Interests,  // Include the user's interests from the model.
                    Availability = new  // Include the user's availability details structured by days of the week.
                    {
                        Monday = new { AM = u.Availability.Monday.AM, PM = u.Availability.Monday.PM },  // Availability for Monday.
                        Tuesday = new { AM = u.Availability.Tuesday.AM, PM = u.Availability.Tuesday.PM },  // Availability for Tuesday.
                        Wednesday = new { AM = u.Availability.Wednesday.AM, PM = u.Availability.Wednesday.PM },  // Availability for Wednesday.
                        Thursday = new { AM = u.Availability.Thursday.AM, PM = u.Availability.Thursday.PM },  // Availability for Thursday.
                        Friday = new { AM = u.Availability.Friday.AM, PM = u.Availability.Friday.PM },  // Availability for Friday.
                        Saturday = new { AM = u.Availability.Saturday.AM, PM = u.Availability.Saturday.PM },  // Availability for Saturday.
                        Sunday = new { AM = u.Availability.Sunday.AM, PM = u.Availability.Sunday.PM }  // Availability for Sunday.
                    }
                };

                // Serialize the structured user data to a JSON string with indentation for readability.
                string json = JsonConvert.SerializeObject(userData, Formatting.Indented);

                // Define the file path where the JSON data will be saved.
                string filepath = Path.Combine(Directory.GetCurrentDirectory(), "data/user_data.txt");

                // Write the JSON data to the specified file path.
                System.IO.File.WriteAllText(filepath, json);
        }
        
    }
}