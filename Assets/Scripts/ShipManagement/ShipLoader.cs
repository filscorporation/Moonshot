using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace ShipManagement
{
    public class ShipLoader : MonoBehaviour
    {
        public static ShipLoader Instance;
        
        #region Properties

        

        #endregion

        #region Attributes

        private byte[] cache;

        #endregion
        
        #region Public methods

        public void SaveShip(Ship ship)
        {
            cache = ObjectToByteArray(ship.ToData());
        }

        public Ship LoadShip()
        {
            if (cache == null || !cache.Any())
                return null;

            return Ship.FromData((Ship.ShipData) ByteArrayToObject(cache));
        }

        #endregion

        #region Private methods

        private void Awake()
        {
            Instance = this;
        }

        private static byte[] ObjectToByteArray(object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        
        private static object ByteArrayToObject(byte[] arrBytes)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                object obj = binForm.Deserialize(memStream);
                return obj;
            }
        }

        #endregion
    }
}