using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ReinforcementLearning.Utils
{
    public static class Serializer
    {
        public static void SerializeObject(string filename, TrainingResult objectToSerialize)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(filename, FileMode.Create))
            {
                formatter.Serialize(fileStream, objectToSerialize);
            }
        }

        public static TrainingResult DeserializeObject(string filename)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fileStream = new FileStream(filename, FileMode.Open))
            {
                return (TrainingResult)formatter.Deserialize(fileStream);
            }
        }

    }
}
