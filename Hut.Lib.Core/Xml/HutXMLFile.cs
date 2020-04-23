/******************************************************************************
 * Hut XML File Serializer
 * 
 * - Must Using With Serializable Type(T).
 * 
 * Author : Daegung Kim
 * Version: 1.0.0
 * Update : 2016-12-23
 ******************************************************************************/
using System;
using System.IO;
using System.Xml.Serialization;

namespace Hut
{
    public class HutXMLFile<T>
    {
        public static T read(string filename)
        {
            T doc = default(T);

            if (File.Exists(filename))
            {
                using (FileStream reader = new FileStream(filename, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    try
                    {
                        doc = (T)serializer.Deserialize(reader);
                        if (doc == null)
                            throw new NullReferenceException();
                    }
                    catch (NullReferenceException e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    reader.Close();
                }
            }

            return doc;
        }

        public static void write(string filename, T doc)
        {
            using (FileStream writer = new FileStream(filename, FileMode.OpenOrCreate))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                try
                {
                    serializer.Serialize(writer, doc);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                writer.Close();
            }
        }
    }
}