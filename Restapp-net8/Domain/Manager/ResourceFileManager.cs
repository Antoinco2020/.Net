using System.Web;
using System.Xml.Linq;

namespace Domain.Manager
{
    public sealed class ResourceFileManager
    {
        private static volatile ResourceFileManager instance;
        private static object syncRoot = new object();
        private const string filePathName = @"\App_GlobalResources\Resource.resx";



        public XDocument _resourceManager;
        private ResourceFileManager() { }



        public void SetResources(string filename = filePathName)
        {
            if (_resourceManager == null)
            {
                _resourceManager = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + filename);
            }
        }
        public string GetConfigData(string keyToCheck)
        {
            try
            {
                XElement result = _resourceManager.Root.Descendants("data")
                .Where(k => k.Attribute("name").Value == keyToCheck)
                .Select(k => k)
                .FirstOrDefault();
                return HttpUtility.HtmlDecode(result.Element("value").FirstNode.ToString());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public bool SetConfigData(string keyToCheck, string value)
        {
            try
            {
                XElement result = _resourceManager.Root.Descendants("data")
                .Where(k => k.Attribute("name").Value == keyToCheck)
                .Select(k => k)
                .FirstOrDefault();
                if (result is null)
                {
                    throw new Exception($"Non ho trovato record nell'xml con nome {keyToCheck}");
                }
                result.Element("value").SetValue(value);
                _resourceManager.Save(AppDomain.CurrentDomain.BaseDirectory + filePathName);
                _resourceManager = XDocument.Load(AppDomain.CurrentDomain.BaseDirectory + filePathName);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static ResourceFileManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null) { }
                        instance = new ResourceFileManager();
                    }
                }
                return instance;
            }
        }
    }
}
