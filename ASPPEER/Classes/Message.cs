namespace ASPPEER
{
    /// <summary>
    /// Message class.
    /// </summary>
    public class MessageClass
    {
        /// <summary>
        /// Subject of letter.
        /// </summary>
        public string Subject{get; set; }
        
        /// <summary>
        /// Text of message.
        /// </summary>
        public string Message{get; set; }

        /// <summary>
        /// Sender's Id.
        /// </summary>
        public string SenderId { get; set; }

        /// <summary>
        /// Receiver's Id.
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="subject">Subject of the letter.</param>
        /// <param name="message">Text of the letter.</param>
        /// <param name="senderId">Sender's Id.</param>
        /// <param name="receiverId">Receiver's Id.</param>
        public MessageClass(string subject, string message, string senderId, string receiverId)
        {
            Subject = subject;
            Message = message;
            SenderId = senderId;
            ReceiverId = receiverId;
        }
    }
}