using Vi.Abstraction;
using Vi.Data.Model;

namespace Vi.Handlers
{
    public class LockFileHandler : IFileHandler<Lockfile>
    {
        public Lockfile Decode(string value)
        {
            var parts = value.Split(':');
            
            return new Lockfile
            {
                Address = "127.0.0.1",
                Username = "riot",
                Password = parts[3],
                Port = int.Parse(parts[2]),
                Protocol = parts[4]
            };
        }
    }
}