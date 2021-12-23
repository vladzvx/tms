using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace TMS.Lib.Utils
{
    public static class IdConverter
    {
        public static DateTime GetFromInt32(int dateTime)
        {
            return DateTime.SpecifyKind(DateTimeOffset.FromUnixTimeSeconds(dateTime).DateTime, DateTimeKind.Utc);
        }

        public static (int Int32, long Int64) Convert(ObjectId objectId)
        {
            byte[] bytes = objectId.ToByteArray();
            return (BitConverter.ToInt32(new byte[4] { bytes[3], bytes[2], bytes[1], bytes[0] }), BitConverter.ToInt64(bytes, 4));
        }
        public static ObjectId Convert(DateTime dateTime, long id)
        {
            if (dateTime.Kind != DateTimeKind.Utc) throw new ArgumentException("dateTime parameter must be in UTC fromat!");
            byte[] bytes = new byte[12];
            DateTimeOffset temp = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            byte[] secondBytes = BitConverter.GetBytes((int)temp.ToUnixTimeSeconds());
            bytes[0] = secondBytes[3];
            bytes[1] = secondBytes[2];
            bytes[2] = secondBytes[1];
            bytes[3] = secondBytes[0];
            BitConverter.GetBytes(id).CopyTo(bytes, 4);
            return new ObjectId(bytes);
        }
        public static ObjectId Convert(int dateTime, long id)
        {
            byte[] bytes = new byte[12];
            byte[] secondBytes = BitConverter.GetBytes(dateTime);
            bytes[0] = secondBytes[3];
            bytes[1] = secondBytes[2];
            bytes[2] = secondBytes[1];
            bytes[3] = secondBytes[0];
            BitConverter.GetBytes(id).CopyTo(bytes, 4);
            return new ObjectId(bytes);
        }
    }
}
