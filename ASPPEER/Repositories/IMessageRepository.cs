using System;
using System.Collections.Generic;
using System.Linq;
using Fare;

namespace ASPPEER.Repositories
{
    /// <summary>
    /// Interface for dependencies.
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Gets a random message and add it to the List. 
        /// </summary>
        /// <param name="users">Users list to get sender and receiver ids.</param>
        /// <returns>Generated Message.</returns>
        MessageClass GetRandomMessage(List<User> users);

        /// <summary>
        /// Gets all messages from list.
        /// </summary>
        /// <returns>List of messages.</returns>
        IEnumerable<MessageClass> GetAllMessages();

        /// <summary>
        /// Adds new message to the repository.
        /// </summary>
        /// <param name="newMessage">New message.</param>
        /// <param name="users">Users in system.</param>
        void AddMessage(MessageClass newMessage, List<User> users);
    }

    /// <summary>
    /// Class that keeps messages collection.
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private List<MessageClass> _messages = new List<MessageClass>();

        /// <summary>
        /// Gets a random message and add it to the List. 
        /// </summary>
        /// <param name="users">Users list to get sender and receiver ids.</param>
        /// <returns>Generated Message.</returns>
        public MessageClass GetRandomMessage(List<User> users)
        {
            Random rnd = new Random();
            // Generation variables, which generates strings by regex expression.
            Xeger subjectGenerator = new(@"[0-9A-Za-z]{1,12}");
            Xeger messageGenerator = new(@"[0-9A-Za-z]{1,12}");
            string senderId = users[rnd.Next(users.Count)].Email;
            string receiverId = users[rnd.Next(users.Count)].Email;

            var newMessage = new MessageClass(subjectGenerator.Generate(), messageGenerator.Generate(), senderId,
                receiverId);

            _messages.Add(newMessage);

            return newMessage;
        }

        /// <summary>
        /// Gets all messages from list.
        /// </summary>
        /// <returns>List of messages.</returns>
        public IEnumerable<MessageClass> GetAllMessages() => _messages;

        /// <summary>
        /// Adds new message to the repository.
        /// </summary>
        /// <param name="newMessage">New message.</param>
        /// <param name="users">Users in system.</param>
        /// <exception cref="ArgumentException">Exception if there are no users with senders or receivers Id.</exception>
        public void AddMessage(MessageClass newMessage, List<User> users)
        {
            User sender = users.FirstOrDefault(u => u.Email == newMessage.SenderId);
            User receiver = users.FirstOrDefault(u => u.Email == newMessage.ReceiverId);
                 if (sender == null)
                     throw new ArgumentException("There are no user with such email " + newMessage.SenderId);
                 if (receiver == null) 
                     throw new ArgumentException("There are no user with such email " + newMessage.ReceiverId);
            _messages.Add(newMessage);
        } 
    }
}