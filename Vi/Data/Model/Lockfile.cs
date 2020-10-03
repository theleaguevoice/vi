namespace Vi.Data.Model
{
    public class Lockfile
    {
        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Protocol { get; set; }
        
        public override string ToString() => $"wss://{Username}:{Password}@{Address}/";
    }
}