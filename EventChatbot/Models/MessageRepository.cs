using System.Collections.Generic;
namespace EventChatbot.Models
{
    public static class MessageRepository
    {
        private static Stack<Message> chat;

        static MessageRepository()
        {
            chat = new Stack<Message>();
        }

        public static void addMessage(string sender, string content)
        {
          chat.Push(new Message (sender,content));
        }

        public static void ClearHistory()
        {
            chat.Clear();      
        }

        public static Stack<Message> getMessages()
        {
            return chat;
        }

    }

}