using System.Text.Json.Serialization;

namespace API.Models.DatabaseModels
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Role { get; set; }
        [JsonIgnore]
        public UserInformation UserInformation { get; set; }
        public string UserInformationId { get; set; }
    }
}
