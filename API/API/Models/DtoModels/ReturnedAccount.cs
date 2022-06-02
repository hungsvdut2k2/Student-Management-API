namespace API.Models.DtoModels
{
    public class ReturnedAccount
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
        public LoginDto Account { get; set; }
    }
}
