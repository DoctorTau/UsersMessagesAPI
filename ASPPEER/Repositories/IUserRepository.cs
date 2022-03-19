using System;
using System.Collections.Generic;
using System.Linq;
using Fare;

namespace ASPPEER.Repositories
{
    /// <summary>
    /// Interface for dependencies.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets random user.
        /// </summary>
        /// <returns>Random user.</returns>
        User GetRandomUser();
        /// <summary>
        /// Returns all users from repository.
        /// </summary>
        /// <returns>Collection of users.</returns>
        IEnumerable<User> GetAllUsers();

        /// <summary>
        /// Adds user to repository.
        /// </summary>
        /// <param name="user">New user.</param>
        void AddUser(User user);

    }

    /// <summary>
    /// Class that keeps users collection.
    /// </summary>
    public class UsersRepository : IUserRepository
    {

        private List<User> _users = new List<User>();

        /// <summary>
        /// Returns new random user and adds it to the collection.
        /// </summary>
        /// <returns>New generated user.</returns>
        public User GetRandomUser()
        {
            User newUser;
            do
            {
                // Generation variables, which generates strings by regex expression.
                Xeger nameGenerator = new(@"[0-9A-Za-z]{6,12}");
                Xeger emailGenerator = new(@"[A-Z0-9a-z]+@[a-z]+\.[a-z]{2,4}");
                newUser = new User(nameGenerator.Generate(), emailGenerator.Generate());
            } while (_users.FirstOrDefault(u => u.Email == newUser.Email) != null);
            _users.Add(newUser);
            _users = _users.OrderBy(u => u.Email).ToList();
            return newUser;
        }

        /// <summary>
        /// Returns collection of all users in repository.
        /// </summary>
        /// <returns>Collection  of users.</returns>
        public IEnumerable<User> GetAllUsers() => _users;

        /// <summary>
        /// Adds user to the list.
        /// </summary>
        /// <param name="user">New user.</param>
        /// <exception cref="ArgumentException">Exception if user with this  email already exists.</exception>
        public void AddUser(User user)
        {
            if(_users.FirstOrDefault(u => u.Email == user.Email) != null)
                throw new ArgumentException("User already exists");
            _users.Add(user);
            _users = _users.OrderBy(u => u.Email).ToList();
        }
    }
    
}