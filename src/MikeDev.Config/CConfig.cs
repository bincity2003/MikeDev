using System;
using System.Xml.Linq;

namespace MikeDev.Config
{
    public class CConfig
    {
        XElement _Storage;

        /// <summary>
        /// Create new empty config.
        /// </summary>
        public CConfig()
        {
            _Storage = new XElement("configuration");
        }

        /// <summary>
        /// Import existing CConfig from file.
        /// </summary>
        /// <param name="cconfigFile">Path to CConfig file.</param>
        public CConfig(string cconfigFile)
        {
            _Storage = XElement.Load(cconfigFile);

            if (_Storage.Name.LocalName != "configuration")
            {
                throw new Exception("Not a configuration file!");
            }
        }

        /// <summary>
        /// Try parse XElement to a CConfig.
        /// </summary>
        /// <param name="config">Existing XElement config.</param>
        public CConfig(XElement config)
        {
            _Storage = config;
        }

        /// <summary>
        /// Get the value of an attribute.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public string this[string name] => _Storage.Element(name).Value;

        /// <summary>
        /// Export CConfig to an XML-string.
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            return _Storage.ToString();
        }

        /// <summary>
        /// Export CConfig to file.
        /// </summary>
        /// <param name="cconfigFile">Path to CConfig file. Will be overwritten.</param>
        public void Export(string cconfigFile)
        {
            System.IO.File.Create(cconfigFile);
            _Storage.Save(cconfigFile);
        }
    }
}
