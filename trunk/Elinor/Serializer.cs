using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Elinor
{
    public static class Serializer
    {
        internal static void SerializeObject(string filename, Profile objectToSerialize)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            var bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }

        internal static Profile DeSerializeObject(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            var bFormatter = new BinaryFormatter();
            var objectToSerialize = (Profile) bFormatter.Deserialize(stream);
            stream.Close();
            return objectToSerialize;
        }
    }
}