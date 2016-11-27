using Scumle.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Scumle.Helpers
{

    // OBS: Heavy inspired by http://www.c-sharpcorner.com/UploadFile/manishkdwivedi/save-a-observablecollection-to-application-storage-in-window/ for learning purpose. 
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
        #endregion

    }
}
