using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Linq;

namespace SDB.Compression
{
    public class Exponent
    {

        public static BigInteger Sqrt(BigInteger n)
        {
            if (n == 0) return 0;
            if (n > 0)
            {
                int bitLength = Convert.ToInt32(Math.Ceiling(BigInteger.Log(n, 2)));
                BigInteger root = BigInteger.One << (bitLength / 2);

                while (!isSqrt(n, root))
                {
                    root += n / root;
                    root /= 2;
                }

                return root;
            }

            throw new ArithmeticException("NaN");
        }

        private static Boolean isSqrt(BigInteger n, BigInteger root)
        {
            BigInteger lowerBound = root * root;
            BigInteger upperBound = (root + 1) * (root + 1);

            return (n >= lowerBound && n < upperBound);
        }

        public static BigInteger ReadExponent(IEnumerable<Byte> parts, UInt32 exponentBase = UInt32.MaxValue)
        {
            // Assumption 1: The number of parts is odd
            BigInteger x = 0;
            BigInteger expBase = exponentBase;
            Byte[] tbb = parts.Take(2).ToArray();
            UInt16 exp = BitConverter.ToUInt16(tbb);
            Byte[] fbb = new Byte[4];
            int i = 0;
            foreach (Byte part in parts)
            {
                // [UInt16, UInt32]
                // [0,1   , 2,3,4,5]

                if (i < 3)
                {
                    fbb[i - 2] = part;
                }
                else
                {
                    fbb[i - 2] = part;


                    UInt32 mul = BitConverter.ToUInt32(fbb);
                    x += BigInteger.Pow(expBase, exp) * mul;
                    i = -1; // will get incremented to 0
                }

                i++;
                exp--;
            }

            if (i == 4)
            {
                fbb[2] = fbb[0];
                fbb[3] = fbb[1];
                fbb[0] = tbb[0];
                fbb[1] = tbb[1];
                UInt32 add = BitConverter.ToUInt32(fbb);

                x += add;
            }
            else
            {
                throw new Exception("Reached an impossible state for this compression");
            }

            return x;
        }


        public static List<UInt32> Exponents(BigInteger x, UInt32 exponent = UInt32.MaxValue)
        {
            /*
            Format of the numbers ^({UInt16}{UInt32})*{UInt32}$

            Build a BigInteger (a whole file binary) using this formula

            */
            UInt16 exp = (UInt16)(BigInteger.Log(x, exponent));
            var l = new List<UInt32> { (UInt32)exp };
            do
            {
                // This will 
                UInt16 newExp = (UInt16)BigInteger.Log(x, exponent);
                BigInteger powed = BigInteger.Pow(exponent, newExp);
                var bigMul = BigInteger.Divide(x, powed);
                UInt32 mul = 1;

                if (bigMul <= UInt32.MaxValue)
                {
                    mul = (UInt32)bigMul;
                }
                else
                {
                    mul = UInt32.MaxValue;
                }


                l.Add((UInt32)(exp - newExp));
                l.Add(mul);

                x -= powed * mul;
                exp = newExp;
                //exp--;

            } while (x > UInt32.MaxValue);
            UInt32 remainder = (UInt32)x;
            l.Add(remainder);

            int i = 3;
            for (; i < l.Count() - 1 && l[i] == 1; i += 2) ;


            if (i >= l.Count() - 2)
            {
                for (i -= 2; i > 0; i -= 2)
                {
                    l.RemoveAt(i);
                }
            }

            return l;
        }

        public static void WriteExponents(List<UInt32> xs, string filepath)
        {
            using (var bw = System.IO.File.Create(filepath))
            {
                if (xs[1] != 0)
                {
                    WriteExponentsImplicit(xs, bw);

                }
                else
                {
                    WriteExponentsExplicit(xs, bw);
                }
            }
        }

        private static void WriteExponentsImplicit(List<uint> xs, System.IO.FileStream bw)
        {
            int i = 1;
            var batchShort = BitConverter.GetBytes(xs[0]).Take(2).ToArray();
            bw.Write(batchShort, 0, 2);
            for (; i < xs.Count; i++)
            {
                var batchUint = BitConverter.GetBytes(xs[i]);
                bw.Write(batchUint, 0, 4);
            }
        }

        private static void WriteExponentsExplicit(List<uint> xs, System.IO.FileStream bw)
        {
            int i = 1;
            for (; i < xs.Count - 1; i += 2)
            {
                var batchShort = BitConverter.GetBytes(xs[i - 1]).Take(1).ToArray();
                bw.Write(batchShort, 0, 2);

                var batchUint = BitConverter.GetBytes(xs[i]);
                bw.Write(batchUint, 0, 4);
            }
            var batchAddUint = BitConverter.GetBytes(xs[i - 1]);
            bw.Write(batchAddUint, 0, 4);
        }

    }
}
