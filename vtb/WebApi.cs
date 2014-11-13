using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;

namespace vtb
{
    public static class WebAPI
    {

        /// <summary>
        /// Retorna dados a partir de um XML e serializa o XML para um objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static T Get<T>(String Url) where T : class
        {
            T resultado = null;

            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(T));

                WebRequest request = WebRequest.Create(Url);
                request.Method = "GET";
                request.Timeout = 1000 * 60;
                WebResponse resp = request.GetResponse();

                string result = string.Empty;
                using (System.IO.Stream stream = resp.GetResponseStream())
                {
                    using (System.IO.StreamReader sr = new System.IO.StreamReader(stream))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                }

                var textStream = new MemoryStream(Encoding.UTF8.GetBytes(result));

                resultado = (T)xml.Deserialize(textStream);
            }
            catch (Exception) { }

            return resultado;
        }
    }
}
