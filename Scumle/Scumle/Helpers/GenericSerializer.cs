using Scumle.Model;
using Scumle.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Scumle.Helpers
{
    /// <summary>
    /// General serializer to serialize from and to XML for saving and loading
    /// </summary>
    public static class GenericSerializer
    { 
        #region Methods
        // Method to take an object and serialize to XML
        public static void convertToXML<T>(T objectToSave, string path) where T : new()
        {
            StreamWriter streamWriter = null;

            try
            {               
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                streamWriter = new StreamWriter(path, false);

                // Settings for XML (Optional I think)
                XmlWriterSettings settings = new XmlWriterSettings()
                {
                    Indent = true,
                    OmitXmlDeclaration = true,
                };

                serializer.Serialize(streamWriter, objectToSave);

            }
            finally
            {
                // Close writer
                if (streamWriter != null)
                {
                    streamWriter.Close();
                }
            }



        }

        // Method to convert XML string to object.
        public static T convertFromXML<T>(string path) where T : new()
        {
            StreamReader streamReader = null;

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                streamReader = new StreamReader(path);
                return (T)serializer.Deserialize(streamReader);

            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Close();
                }
            }





        }        

       // Method to serialize XML in memory
        public static string SerializeToXMLInMemory(List<ModelBase> objectToSave)
        {
           XmlSerializer serializer = new XmlSerializer(typeof(List<ModelBase>));

            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, objectToSave);
                stream.Position = 0;
                return Encoding.UTF8.GetString(stream.GetBuffer());
            }
        }

        // Method to deserialize from XML to object in memory.
        public static List<ModelBase> convertFromXMLInMemory(string dataInMemory)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<ModelBase>));

            if (string.IsNullOrEmpty(dataInMemory))
                return null;

            using (var stream = new MemoryStream())
            {
                var bytes = Encoding.UTF8.GetBytes(dataInMemory);
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                return (List<ModelBase>)serializer.Deserialize(stream);
            }
        }
        #endregion

    }
}
