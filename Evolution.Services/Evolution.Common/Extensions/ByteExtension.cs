using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Evolution.Common.Extensions
{
    public static class ByteExtension
    { 
        public static int ToInt(this byte? value)
        {
            if (value == null)
                return 0;
            else
                return Convert.ToInt32(value);
        }
        /// <summary>
        /// This extencsion method should be used to get update count value.
        /// This checks for max size of tinyint(ie 255 ) and resets value to 1
        /// </summary> 
        public static byte CalculateUpdateCount(this byte? value)
        {
            int updCount = (value != null)? Convert.ToInt32(value) : 0;
            updCount = updCount+1;
            return Convert.ToByte(updCount > 255 ? 1 : updCount);
        }

        /// <summary>
        /// This extencsion method should be used to get update count value.
        /// This checks for max size of tinyint(ie 255 ) and resets value to 1
        /// </summary> 
        public static byte CalculateUpdateCount(this byte value)
        {
            int updCount =   Convert.ToInt32(value) + 1 ; 
            return Convert.ToByte(updCount > 255 ? 1 : updCount);
        }

        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
