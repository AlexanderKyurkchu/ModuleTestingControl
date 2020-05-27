using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui_test_modul
{
   public  class Word
    {
        public static UInt16 FromByteArray(byte[] bytes)
        {
            // bytes[0] -> HighByte
            // bytes[1] -> LowByte
            return FromBytes(bytes[1], bytes[0]);
        }


        public static Int16 IntFromByteArray(byte[] bytes)
        {
            // bytes[0] -> HighByte
            // bytes[1] -> LowByte
            return IntFromBytes(bytes[1], bytes[0]);
        }



        public static UInt16 FromBytes(byte LoVal, byte HiVal)
        {
            return (UInt16)(HiVal * 256 + LoVal);
        }

        public static Int16 IntFromBytes(byte LoVal, byte HiVal)
        {
            return (Int16)(HiVal * 256 + LoVal);
        }



        public static UInt16[] ByteToUInt16(byte[] bytes)
        {
            UInt16[] values = new UInt16[bytes.Length / 2];
            int counter = 0;
            for (int cnt = 0; cnt < bytes.Length / 2; cnt++)
                values[cnt] = FromByteArray(new byte[] { bytes[counter++], bytes[counter++] });
            return values;
        }



        public static Int16[] IntByteToUInt16(byte[] bytes)
        {
            Int16[] values = new Int16[bytes.Length / 2];
            int counter = 0;
            for (int cnt = 0; cnt < bytes.Length / 2; cnt++)
                values[cnt] = IntFromByteArray(new byte[] { bytes[counter++], bytes[counter++] });
            return values;
        }

        public static float convert_uint_to_float2(uint hw, uint lw)
        {
            byte[] b1 = BitConverter.GetBytes((ushort)hw);

            byte[] b2 = BitConverter.GetBytes((ushort)lw);

            byte[] dest = new byte[4];

            Array.Copy(b2, 0, dest, 0, 2);
            Array.Copy(b1, 0, dest, 2, 2);

            return BitConverter.ToSingle(dest, 0);
        }

        public static byte [] convert_float_to_byte_array(float FloatData)
        {
            //short[] data = new short[2];

            byte[] tempdata = BitConverter.GetBytes(FloatData);
           
           // data = IntByteToUInt16(tempdata);

            return tempdata;
        }


    }
}
