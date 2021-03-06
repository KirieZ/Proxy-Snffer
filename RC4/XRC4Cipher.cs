﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RappelzSniffer.RC4
{
	public class XRC4Cipher
	{
        #region Cipher
        class TImpl : RC4Cipher { }

		TImpl m_pImpl;

		public XRC4Cipher(string pKey)
		{
			m_pImpl = new TImpl();
			Clear();
			m_pImpl.Init(pKey);
		}

		public byte[] Peek(ref byte[] pSrc, int len)
		{
			RC4Cipher.State backup;
			m_pImpl.SaveStateTo (out backup);
			
			byte[] pDst = DoCipher(ref pSrc, len);

			m_pImpl.LoadStateFrom(ref backup);

            return pDst;
		}

		public byte[] DoCipher(ref byte[] pSrc, int len = 0)
		{
			if (len == 0) len = pSrc.Length;
			byte[] pDst = m_pImpl.Code(ref pSrc, len);

            return pDst;
		}
        #endregion

        #region Encode/Decode/Cleanup
        public byte[] Encode(ref byte[] pSrc, int len, bool bIsPeek = false)
		{
			if (bIsPeek)
				return Peek(ref pSrc, len);
			else
				return DoCipher(ref pSrc, len);
		}

		public byte[] Decode(ref byte[] pSrc, int len, bool bIsPeek = false)
		{
			if (bIsPeek)
				return Peek(ref pSrc, len);
			else
				return DoCipher(ref pSrc, len);
		}

		public void Clear()
		{
			m_pImpl.Init("Neat & Simple");
		}
        #endregion
    }
}
