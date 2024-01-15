using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace FoodieSydney.Model
{
    public class JsonNetResult : ActionResult
    {
        public Encoding ContentEncoding { get; set; }
        public string ContentType { get; set; }
        public object Data { get; set; }

        public JsonSerializerSettings SerializerSettings { get; set; }
        public Formatting Formatting { get; set; }


        public JsonNetResult()
        {
            SerializerSettings = new JsonSerializerSettings();
        }

        public void ExecuteResult(ControllerContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            var response = context.HttpContext.Response;

            response.ContentType = string.IsNullOrEmpty(ContentType) == false
              ? ContentType
              : "application/json";

            //if (ContentEncoding != null)
            //    response.ContentEncoding = ContentEncoding;

            //if (Data != null)
            //{
            //    var writer = new JsonTextWriter(response.Output) { Formatting = Formatting };

            //    var serializer = JsonSerializer.Create(SerializerSettings);
            //    serializer.Serialize(writer, Data);

            //    writer.Flush();
            //}
        }
    }
}
