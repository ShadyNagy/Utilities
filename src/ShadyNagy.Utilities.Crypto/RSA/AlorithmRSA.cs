using System;
using System.IO;
using ShadyNagy.Utilities.Crypto.Helper;

namespace ShadyNagy.Utilities.Crypto.RSA
{
    public class AlgorithmRsa : IAlgorithm
    {
		private readonly int _nBits;
		private readonly BigIntegerFixed _pExponent;
        private readonly BigIntegerFixed _pModulus;

        public AlgorithmRsa(int bits, string modulus, string exponent)
		{
			try
			{
                _nBits = bits;
                var xx = Convert.FromBase64String(exponent);
                _pExponent = new BigIntegerFixed(Convert.FromBase64String(exponent));
                _pModulus = new BigIntegerFixed(Convert.FromBase64String(modulus));
			}
			catch (Exception ex)
			{
				throw new ArgumentException("Missed data for RSA algorithm", ex);
			}
		}        

		public byte[] Encrypt(byte[] data, bool isPadding=true)
		{
            var toEncrypt = data;
            if (isPadding)
            {
                toEncrypt = AddPadding(data, _nBits);
            }            
            
            var src = new BigIntegerFixed(toEncrypt);
			var res = src.ModPow(_pExponent, _pModulus);

            var bytesResult = res.getBytes();
            if (!isPadding)
            {
                //need to remove padding
                bytesResult = RemovePadding(bytesResult);
            }
			return bytesResult;
		}
        public static byte[] RemovePadding(byte[] bs)
        {
            if (bs == null)
            {
                return null;
            }

            if (bs.Length < 5)
            {
                return bs;
            }

            var s = new MemoryStream();

            for(var i=0; i< bs.Length; i++)
            {
                if (i + 5 > bs.Length)
                {
                    break;
                }
                if (bs[i] != 0x00 || bs[i + 1] != 0x01 || bs[i + 2] != 0x01)
                {
                    continue;
                }
                var length = bs[i+3] | (bs[i+4] << 8);
                if (length <= 0 || i+5+length > bs.Length)
                {
                    break;
                }
                s.Write(bs, i+5, length);
                return s.ToArray();
            }

            return bs;
        }

        public static byte[] AddPadding(byte[] bs, int bits)
        {
            if (bs == null)
            {
                return null;
            }

            const int nMinPadding = 8 + 3;
            var nMaxPadding = nMinPadding + 16;
            var nMaxBytes = bits / 8;
            if ((bs.Length + 4) + nMinPadding > nMaxBytes) throw new Exception("Data is too long for this algorithm");

            var s = new MemoryStream();

            var r = new System.Random();

            var nMaxPaddingAccordingToMaxBytes = nMaxBytes - (bs.Length+4);
            if (nMaxPaddingAccordingToMaxBytes < nMaxPadding)
            {
                nMaxPadding = nMaxPaddingAccordingToMaxBytes;
            }

            var nPaddingBytes = nMinPadding;
            if (nMaxPadding > nMinPadding)
            {
                nPaddingBytes += r.Next(nMaxPadding - nMinPadding);
            }

            s.WriteByte(0);
            s.WriteByte(2);
            for (var i = 0; i < nPaddingBytes - 1; i++)
            {
                s.WriteByte((byte)r.Next(1, 255));
            }
            s.WriteByte(0);
            s.WriteByte(1);
            s.WriteByte(1);
            s.WriteByte((byte)(bs.Length & 0xFF));
            s.WriteByte((byte)((bs.Length >> 8) & 0xFF));
            s.Write(bs, 0, bs.Length);

            var nRest = nMaxBytes - (int)s.Length;
            while (nRest-- > 0)
            {
                s.WriteByte((byte)r.Next(255));
            }

            return s.ToArray();
        }
    }
}