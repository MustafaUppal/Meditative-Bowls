using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BackNavigatorPro
{
    public class PersistantData
    {
        public static void Save(int currentID)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();

            FileStream file = null;

            if (!File.Exists(Application.persistentDataPath + "/appData.dat"))
                file = File.Create(Application.persistentDataPath + "/appData.dat");
            else
                file = File.OpenWrite(Application.persistentDataPath + "/appData.dat");

            binaryFormatter.Serialize(file, currentID);
            file.Close();
        }

        public static int Load()
        {
            if (!File.Exists(Application.persistentDataPath + "/appData.dat"))
                Save(0);

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/appData.dat", FileMode.Open);
            int currentID = (int)binaryFormatter.Deserialize(file);

            file.Close();

            return currentID;
        }
    }
}