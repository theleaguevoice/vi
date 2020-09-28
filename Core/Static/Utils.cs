using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Core.Static
{
    public class Utils
    {
        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static bool ReadFile(string path, out string content)
        {
            content = "";

            if (!File.Exists(path))
                return false;

            using var inStream = new FileStream(path, FileMode.Open, 
                FileAccess.Read, FileShare.ReadWrite);
            
            using var streamReader = new StreamReader(inStream);
            var file = streamReader.ReadToEnd();

            if (string.IsNullOrWhiteSpace(file))
                return false;

            content = file;
            return true;
        }
    }
}