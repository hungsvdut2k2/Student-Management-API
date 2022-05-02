namespace API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string Role { get; set; } = "Student";
        // 1 -> 1 : 1 user -> 1 information
        public UserInformation UserInformation { get; set; }
        public int UserInformationId { get; set; }
        
    }
}
