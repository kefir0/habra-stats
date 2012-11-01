using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace HabraStatsService
{
    public static class Uploader
    {
        public static void Publish(string data, string fileName)
        {
            // TODO: make this better
            var creds = File.ReadAllLines(@"e:\HabrCache\creds.txt").Where(l => !string.IsNullOrEmpty(l)).ToArray();
            for (var i = 0; i < creds.Length/3; i++)
            {
                var retry = 5;
                while (retry > 0)
                {
                    try
                    {
                        Upload(creds[i*3], 21, string.Empty, fileName, Encoding.UTF8.GetBytes(data), creds[i*3 + 1], creds[i*3 + 2]);
                        break;
                    }
                    catch (Exception ex)
                    {
                        HabraStatsSvc.Log("Error during upload to: " + creds[i*3], 13, ex.ToString());
                        Thread.Sleep(3000);
                    }
                    retry--;
                }
            }
        }

        public static void Upload(string server, int port, string targetFolder, string fileName, byte[] data, string username, string password)
        {
            var url = string.Format("ftp://{0}:{1}{2}/{3}", server, port, targetFolder, fileName);
            var ftp = (FtpWebRequest) WebRequest.Create(url);
            ftp.Credentials = new NetworkCredential(username, password);
            ftp.KeepAlive = false;
            ftp.UseBinary = true;
            ftp.Method = WebRequestMethods.Ftp.UploadFile;

            using (var requestStream = ftp.GetRequestStream())
            {
                using (var writer = new BinaryWriter(requestStream))
                {
                    writer.Write(data);
                }
            }
        }
    }
}