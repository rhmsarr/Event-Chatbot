using System.Collections.Generic;
namespace EventChatbot.Models
{
    public class MessageRepository
    {
        private List<Message> chat;

        public MessageRepository()
        {
            chat = new List<Message>();
        }

        public void addMessage(string sender, string content)
        {
          chat.Add(new Message (sender,content));
        }

        public Message getLastMessage()
        {
            return chat[chat.Count - 1];      
        }

        public List<Message> getMessages()
        {
            return chat;
        }

    }

}