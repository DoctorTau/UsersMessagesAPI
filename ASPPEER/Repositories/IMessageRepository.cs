using System;
using System.Collections.Generic;
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
            Xeger subjectGenerator = new(@"[0-9A-Za-z]{1,12}");
            Xeger messageGenerator = new(@"[0-9A-Za-z]{1,12}");
            string senderId = users[rnd.Next(users.Count)].UserName;
            string receiverId = users[rnd.Next(users.Count)].UserName;

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
    }
}