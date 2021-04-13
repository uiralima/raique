namespace Raique.Microservices.Authenticate.Domain
{
    public class User : Base
    {
        public int UserId { get; set; }
        public string Key { get; set; }
        public string Password { get; set; }
        public string CheckKey { get; set; }
        public string AppKey { get; set; }
    }
}
