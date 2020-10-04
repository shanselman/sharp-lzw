/*

	This is a simple implementation of the well-known LZW algorithm. 
    Copyright (C) 2011  Stamen Petrov <stamen.petrov@gmail.com>

    This program is free software; you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation; either version 2 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpLZW
{
    public class LZWDecoder
    {
        public Dictionary<string, int> dict = new Dictionary<string, int>();
        int codeLen = 8;
        ANSI table;
        public LZWDecoder()
        {
            table = new ANSI();
            dict = table.Table;         
        }

        public string DecodeFromCodes(byte[] bytes)
        {
            string output = bytes.GetBinaryString();            

            return Decode(output);
        }

        public string Decode(string output)
        {
            StringBuilder sb = new StringBuilder();

            int i = 0;
            string w = "";
            int prevValue = -1; 

            while (i < output.Length)
            {
                if (i + codeLen + 8 <= output.Length)
                {
                    w = output.Substring(i, codeLen);
                }
                else if (i + codeLen <= output.Length)
                {
                    int encodedLen = i + codeLen;
                    int trimBitsLen = output.Length - encodedLen;

                    w = output.Substring(i, codeLen - (8 - trimBitsLen)) + output.Substring(output.Length - (8 - trimBitsLen), (8 - trimBitsLen));
                }
                else
                {
                    break;
                }

                i += codeLen;

                int value = Convert.ToInt32(w, 2);

                string key = dict.FindKey(value);
                string prevKey = dict.FindKey(prevValue);

                if (prevKey == null)
                {
                    prevKey = "";
                }

                if (key == null)
                {
                    //handles the situation cScSc
                    key = prevKey;

                    sb.Append(prevKey + key.Substring(0, 1));
                }
                else
                {
                    sb.Append(key);
                }

                string finalKey = prevKey + key.Substring(0, 1);

                if (dict.ContainsKey(finalKey) == false)
                {
                    dict[finalKey] = dict.Count;
                }

                if (Convert.ToString(dict.Count, 2).Length > codeLen)
                    codeLen++;

                prevValue = value;
            }

            return sb.ToString();
        }

    }
}