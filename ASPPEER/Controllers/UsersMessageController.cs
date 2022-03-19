using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ASPPEER.Repositories;
using Microsoft.Extensions.Logging;

namespace ASPPEER.Controllers
{
    /// <summary>
    /// Controller that provides interaction with users and messages.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UsersMessageController : ControllerBase
    {
        private readonly IUserRepository _usersRepository;
        private readonly IMessageRepository _messagesRepository;

        private readonly ILogger<UsersMessageController> _logger;

        private Random _rnd  = new Random();

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="usersRepository">Repository of users.</param>
        /// <param name="messagesRepository">Repository of messages.</param>
        /// <param name="logger">Logger.</param>
        public UsersMessageController(IUserRepository usersRepository,
            IMessageRepository messagesRepository,
            ILogger<UsersMessageController> logger)
        {
            _usersRepository = usersRepository;
            _messagesRepository = messagesRepository;

            this._logger = logger;
        }
        
        /// <summary>
        /// Post method that generates users and adds them to the repository.
        /// </summary>
        /// <param name="count">Amount of users to generate. By the default is random number from 1 to 10.</param>
        /// <returns>Collection of new users.</returns>
        [HttpPost("random-users")]
        public ActionResult<IReadOnlyList<User>> GetRandomUsers(int? count = null)
        {
            count ??= _rnd.Next(1, 11);
            var response = Enumerable.Range(0, (int) count)
                .Select(x => _usersRepository.GetRandomUser())
                .ToList();
            return Ok(response);
        }

        /// <summary>
        /// Post method that generates messages and adds them to the repository.
        /// </summary>
        /// <param name="count">Amount of messages to generate. By the default is random number from 1 to 10.</param>
        /// <returns>Collection of new messages.</returns>
        [HttpPost("random-messages")]
        public ActionResult<IReadOnlyList<MessageClass>> GetRandomMessages(int? count)
        {
            count ??= _rnd.Next(1, 11);
            if (((List<User>)_usersRepository.GetAllUsers()).Count == 0) return NotFound("There are no users to have any messages.");
            var response = Enumerable.Range(0, (int) count)
                .Select(x => _messagesRepository.GetRandomMessage((List<User>)this._usersRepository.GetAllUsers()))
                .ToList();
            return Ok(response);
        }

        /// <summary>
        /// Returns information about user by his email.
        /// </summary>
        /// <param name="email">Email of user.</param>
        /// <returns>Information about user.</returns>
        [HttpGet("get-user-by-email")]
        public ActionResult<User> GetUserByEmail(string email)
        {
            var user = _usersRepository.GetAllUsers().FirstOrDefault(u => u.Email == email);
            if (user == null) return NotFound("User not found");
            return Ok(user);
        }

        
        /// <summary>
        /// Gets all users. You can set limit of users to get and start point.
        /// </summary>
        /// <param name="limit">Limit of users to return.  By default returns all users.</param>
        /// <param name="offset">Start position in list.Default is 0.</param>
        /// <returns>Collection of users if all parameters are correct.</returns>
        [HttpGet("get-all-user")]
        public ActionResult<IReadOnlyList<User>> GetAllUsers(int limit = 0, int offset = 0)
        {
            if(limit < 0 || offset < 0)
                return BadRequest("Limit and offset must be greater or equals zero.");
            if(limit == 0)
                return Ok(_usersRepository.GetAllUsers().Skip(offset).ToList());
            return Ok(_usersRepository.GetAllUsers().Skip(offset).Take(limit).ToList());
        }

        /// <summary>
        /// Returns all messages collection by sender's and receiver's id.
        /// </summary>
        /// <param name="senderId">Sender's id.</param>
        /// <param name="receiverId">Receiver's id.</param>
        /// <returns>Collection of messages.</returns>
        [HttpGet("get-message-by-sender-and-receiver")]
        public ActionResult<IReadOnlyList<MessageClass>> GetMessageBySenderAndReceiver(string senderId,
            string receiverId) =>
            Ok(_messagesRepository.GetAllMessages()
                .Where(u => u.SenderId == senderId && u.ReceiverId == receiverId).ToList());

        /// <summary>
        /// Returns all messages collection by sender's id.
        /// </summary>
        /// <param name="senderId">Sender's id.</param>
        /// <returns>Collection of messages.</returns>
        [HttpGet("get-message-by-sender")]
        public ActionResult<IReadOnlyList<MessageClass>> GetMessageBySender(string senderId) =>
            Ok(_messagesRepository.GetAllMessages()
                .Where(u => u.SenderId == senderId).ToList());

        /// <summary>
        /// Returns all messages collection by receiver's id.
        /// </summary>
        /// <param name="receiverId">Receiver's id.</param>
        /// <returns>Collection of messages.</returns>
        [HttpGet("get-message-by-receiver")]
        public ActionResult<IReadOnlyList<MessageClass>> GetMessageByReceiver(string receiverId) =>
            Ok(_messagesRepository.GetAllMessages()
                .Where(u => u.ReceiverId == receiverId).ToList());

        /// <summary>
        /// Adds user to the repository by email and username.
        /// </summary>
        /// <param name="username">New user's username.</param>
        /// <param name="email">New user's email.</param>
        /// <returns>Created user or error.</returns>
        [HttpPost("Add-user")]
        public ActionResult<User> AddUser(string username, string email)
        {
            var newUser = new User(username, email);
            try
            {
                _usersRepository.AddUser(newUser);
            }
            catch (ArgumentException)
            {
                return Problem("User with email " + newUser.Email + " already exists.");
            }

            return Ok(newUser);
        }

        [HttpPost("Add-message")]
        public ActionResult<MessageClass> AddMessage(string subject, string messageText, string senderId,
            string receiverId)
        {
            MessageClass newMessage = new MessageClass(subject, messageText, senderId, receiverId);
            try
            {
                _messagesRepository.AddMessage(newMessage,
                    _usersRepository.GetAllUsers().ToList());
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(newMessage);

        }
    }
}