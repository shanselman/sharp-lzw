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

using SharpLZW;
using System.IO;

namespace TestLZW
{
    class Program
    {
        static string fileToCompress = "Test.txt";
        static string encodedFile = "TestOutput.txt";
        static string decodedFile = "TestDecodedOutput.txt";

        static void Main(string[] args)
        {
            Console.WriteLine("Generate ANSI table ...");
            
            ANSI ascii = new ANSI();
            ascii.WriteToFile();

            Console.WriteLine("ANSI table generated.");

            Console.WriteLine("Start encoding " + fileToCompress + " ...");

            string text = File.ReadAllText(fileToCompress, System.Text.ASCIIEncoding.Default);
            LZWEncoder encoder = new LZWEncoder();
            byte[] b = encoder.EncodeToByteList(text);
            File.WriteAllBytes(encodedFile, b);

            Console.WriteLine("File " + fileToCompress + " encoded to " + encodedFile);

            Console.WriteLine("Start decoding " + encodedFile);

            LZWDecoder decoder = new LZWDecoder();
            byte[] bo = File.ReadAllBytes(encodedFile);
            string decodedOutput = decoder.DecodeFromCodes(bo);
            File.WriteAllText(decodedFile, decodedOutput, System.Text.Encoding.Default);

            Console.WriteLine("File " + encodedFile + " decoded to " + decodedFile);
        }
    }
}
