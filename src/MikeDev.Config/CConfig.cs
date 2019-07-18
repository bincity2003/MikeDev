using System;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;

namespace MikeDev.Config
{
    public class CConfig
    {
        XElement _Storage;
        List<string> _Names;

        /// <summary>
        /// Create new empty config.
        /// </summary>
        public CConfig()
        {
            _Storage = new XElement("configuration");
            _Names = new List<string>();
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

            _Names = new List<string>();
            foreach (var item in _Storage.Elements())
            {
                _Names.Add(item.Name.LocalName);
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
        /// Get or set the value of an attribute.
        /// </summary>
        /// <param name="name">Name of the attribute.</param>
        /// <returns>Value of the attribute.</returns>
        public string this[string name]
        {
            get => _Storage.Element(name).Value;
            set => _Storage.Element(name).Value = value;
        }

        /// <summary>
        /// Get the number of attributes in this config.
        /// </summary>
        public int Count => _Names.Count;

        /// <summary>
        /// Get the name of attributes in this config.
        /// </summary>
        public string[] Attributes => _Names.ToArray();

        /// <summary>
        /// Export CConfig to file.
        /// </summary>
        /// <param name="cconfigFile">Path to CConfig file. Will be overwritten.</param>
        public void Export(string cconfigFile)
        {
            System.IO.File.Create(cconfigFile);
            _Storage.Save(cconfigFile);
        }

        /// <summary>
        /// Add a new attribute and its value.
        /// </summary>
        /// <param name="name">Name of the attribute.</param>
        /// <param name="value">Value of the attribute.</param>
        public void Add(string name, string value)
        {
            if (!_Names.Contains(name))
            {
                _Storage.Add(new XElement(name, value));
            }
            else
            {
                throw new ArgumentException("Name already exists!");
            }
        }

        /// <summary>
        /// Remove an attribute.
        /// </summary>
        /// <param name="name">Name of the attribute.</param>
        public void Remove(string name)
        {
            if (_Names.Contains(name))
            {
                XNode node = _Storage.Elements().Where(x => x.Name.LocalName == name).ToList()[0];
                node.Remove();
            }
            else
            {
                throw new ArgumentException("Name not exists!");
            }
        }
    }
}
