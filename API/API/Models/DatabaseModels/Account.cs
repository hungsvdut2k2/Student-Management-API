namespace API.Models.DatabaseModels
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        public int? RefreshToken { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
    }
}
