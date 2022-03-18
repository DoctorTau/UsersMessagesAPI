namespace ASPPEER
{
    /// <summary>
    /// User class.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Name of the user.
        /// </summary>
       public string UserName { get; set; } 
       
        /// <summary>
        /// User's email, which is an id  as well.
        /// </summary>
       public string Email { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="userName">User's name.</param>
        /// <param name="email">User's Id.</param>
       public User(string userName, string email)
       {
           this.UserName = userName;
           this.Email = email;
       }
    }
}