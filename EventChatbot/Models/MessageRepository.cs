using System.Collections.Generic;
namespace EventChatbot.Models
{
    public static class MessageRepository
    {
        private static List<Message> chat;

        static MessageRepository()
        {
            chat = new List<Message>();
        }

        public static void addMessage(string sender, string content)
        {
          chat.Add(new Message (sender,content));
        }

        public static Message getLastMessage()
        {
            return chat[chat.Count - 1];      
        }

        public static List<Message> getMessages()
        {
            return chat;
        }

    }

}