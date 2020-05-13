/******************************************************************************
 * Hut Json File Serializer(general)
 *
 * - Must Using With Serializable Type(T).
 * - Dependency from Newtonsoft.Json.
 *
 *
 * Author : Daegung Kim
 * Version: 1.0.2
 * Update : 2017-05-08
 * TODO   : make more specific options from json data.
 *          eg, convert enumeration to string and vice versa
 ******************************************************************************/

using System;
using System.IO;
using Newtonsoft.Json;

namespace Hut
{
    public class HutJsonFile<T>
    {
        public static T Read(string filename, bool includetype = false)
        {
            T doc = default;

            if (File.Exists(filename))
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        if (includetype)
                            doc = JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                        else
                            doc = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                        if (doc == null)
                            throw new NullReferenceException();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    reader.Close();
                }
            }

            return doc;
        }

        public static void Write(string filename, T doc, bool includetype = false)
        {
            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    if (includetype)
                        writer.Write(JsonConvert.SerializeObject(doc, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
                    else
                        writer.Write(JsonConvert.SerializeObject(doc, Formatting.Indented));
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