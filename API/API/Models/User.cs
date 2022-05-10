namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte [] PasswordHash { get; set; }
        public byte [] PasswordSalt { get; set; }
        public string Role { get; set; }
        // Add user and user information relationship : 1 -> 1
        public UserInformation UserInformation { get; set; }
        public string UserInformationId { get; set; } 
    }
}
