using Newtonsoft.Json;
using System.Xml.Linq;

namespace Domain.Extensions
{
    public static class JsonExt
    {
        public static string Serialize(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(this string obj)
        {
            return JsonConvert.DeserializeObject<T>(obj);
        }

        public static string SerializeXNode(this string xmlPath)
        {
            return JsonConvert.SerializeXNode(XDocument.Load(new StreamReader(xmlPath)));
        }
    }
}
