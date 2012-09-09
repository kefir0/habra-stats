using System.IO;
using System.Net;
using System.Text;

namespace HabraStatsService
{
    public static class Uploader
    {
        public static void Publish(string data, string fileName)
        {
            var creds = File.ReadAllLines(@"e:\HabrCache\creds.txt");
            Upload(creds[0], 21, "/public_html", fileName, Encoding.UTF8.GetBytes(data), creds[1], creds[2]);
        }

        public static void Upload(string server, int port, string targetFolder, string fileName, byte[] data, string username, string password)
        {
            var url = string.Format("ftp://{0}:{1}{2}/{3}", server, port, targetFolder, fileName);
            var ftp = (FtpWebRequest) WebRequest.Create(url);
            ftp.Credentials = new NetworkCredential(username, password);
            ftp.KeepAlive = false;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;

            using (var writer = new BinaryWriter(ftp.GetRequestStream()))
            {
                writer.Write(data);
            }
        }
    }
}