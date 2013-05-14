using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bionte.Toolbox
{
    public static class ArrayTools
    {

        public static List<byte[]> SplitBytes(this byte[] source, byte delimiter)
        {
            List<byte[]> ret = new List<byte[]>();

            List<byte> temp = new List<byte>();

            for (int i = 0; i < source.Length; i++ )
            {

                if (source[i] != delimiter)
                {
                    temp.Add(source[i]);

                    if (i == source.Length - 1)
                    {
                        ret.Add(temp.ToArray());
                    }
                }
                else
                {
                    
                    ret.Add(temp.ToArray());
                    temp.Clear();
                }
            }
            
            return ret;

        }

        public static List<byte[]> SplitBytes(this byte[] source, byte[] pattern)
        {

            List<byte[]> ret = new List<byte[]>();
            long start = 0;
            long end = 0;

            for (; start <= source.Length; )
            {
                byte pat = 0;

                if (source.Contains(ref pattern, (int)start, out end))
                {

                    long size = end - start;
                    Byte[] dest = new byte[size];
                    //if (start > 0) start;
                    System.Buffer.BlockCopy(source, (int)start, dest, 0, (int)size);
                    
                    start = end;
                    
                    //if (start == end) start++;
                    
                    ret.Add(dest);
                    //Console.WriteLine(System.Text.Encoding.ASCII.GetString(dest));
                    //Console.WriteLine("------------------------------------------------");
                }
                else
                {
                    long size = end - start;
                    Byte[] dest = new byte[size];
                    System.Buffer.BlockCopy(source, (int)start, dest, 0, (int)size);
                    //end++;
                    start = end;
                    ret.Add(dest);
                    //Console.WriteLine(System.Text.Encoding.ASCII.GetString(dest));
                    //Console.WriteLine("------------------------------------------------");
                    break;
                }
                //if (start == end) start++;
                    

                 
            }

            return ret;

        }

        public static bool Contains(this byte[] buffer, ref byte[] sequence, int from, out long position)
        {
            int currOffset = 0;

            for (position = from; position < buffer.Length; position++)
            {
                byte b = buffer[position];
                if (b == sequence[currOffset])
                {
                    if (currOffset == sequence.Length - 1) return true;
                    currOffset++;
                    continue;
                }

                if (currOffset == 0) continue;
                position -= currOffset;
                currOffset = 0;
            }

            return false;
        }

        public static string Unpack(this byte[] buffer)
        {
            int low;
            int hi;

            StringBuilder sb = new StringBuilder(buffer.Length * 2);

            for (long i = 0; i <= buffer.Length - 1; i++)
            {
                low = buffer[i] & 0xf;
                hi = (buffer[i] >> 4);

                if ((i == 0) && (hi == 15))
                {
                    hi = 0;
                }

                sb.AppendFormat("{0}{1}", Convert.ToString(hi), Convert.ToString(low));
            }
            //return System.Convert.ToInt32(sb.ToString());
            return sb.ToString();
        }

        public static byte[] Pack(this byte[] buffer)
        {
            //byte[] number = new byte[11] { 0, 1, 0, 1, 0, 1, 0, 9, 1, 9, 2 };

            int size = 0;
            bool isEven = false;

            if (buffer.Length == 0)
            {
                byte[] ret = new byte[0];
                return ret;
            }
            else if (buffer.Length == 1)
            {
                byte[] ret = new byte[1];
                ret[0] = (byte)(buffer[0] * 10);
                return ret;
            }
            else if ((buffer.Length % 2) > 0)
            {
                isEven = true;
                size = (int)Decimal.Truncate((buffer.Length / 2) + 1);
            }
            else
            {
                size = buffer.Length / 2;
            }

            byte[] res = new byte[size];

            int j = 0;
            for (int i = buffer.Length - 1; i >= 0; i -= 2)
            {

                byte lower = (byte)(buffer[i]);
                byte upper = 0;

                if ((i == 0) && (isEven == true))
                {
                    upper = 0;
                }
                else
                {
                    upper = (byte)(buffer[i - 1] * 10);
                }

                res[j] = (byte)(upper + lower);

                j++;
            }
            return res;
        }

        //public static byte[] Unpack(this byte[] buffer)
        //{

        //}    
    }
}
