using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ExperimentCode.Adapter
{
    static class IPAgentSettings
    {
        public static string CacheDirectory = ".\\Cache\\";
    }

    public class IPAgent
    {

        public static void HttpGetFile(string URL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            if (!response.ContentType.ToLower().StartsWith("text/"))
            {
                //Value = SaveBinaryFile(response, FileName); 
                byte[] buffer = new byte[1024];
                Stream outStream = System.IO.File.Create(URL.Substring(URL.LastIndexOf("/")+1));
                Stream inStream = response.GetResponseStream();
                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);
                outStream.Close();
                inStream.Close();
            }
        }


    }
}
