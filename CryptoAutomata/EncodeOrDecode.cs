// 
// CryptoAutomata/EncodeOrDecode/EncodeOrDecode.cs
// 
// Author:
//     Isabell Cowan <isabellcowan@gmail.com>
//
// Copyright (c) 2015 Isabell Cowan
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace CryptoAutomata
{

	public static class EncodeOrDecode
	{

		public static bool Cipher (
			string pathToOriginal,
			string pathToKey,
			ref string pathToCipher
		) {
			ConsoleManager.Header ("Entered En/Decoder");
			byte[] text;
			byte[] key;
			bool exit = ParseArguments (
				pathToOriginal,
				pathToKey,
				ref pathToCipher,
				out text,
				out key
			);
			if (exit) {
				ConsoleManager.Warning ("Exiting!");
				return true;
			}
			PrintParams (
				pathToOriginal,
				text,
				pathToKey, 
				key,
				pathToCipher
			);
			long totalElapsedTime = 0;
			long cipherTextElapsedTime;
			byte[] cipheredText = XOr (text, key, out cipherTextElapsedTime);
			totalElapsedTime += cipherTextElapsedTime;
			long saveElapsedTime;
			SaveCipheredText (pathToCipher, cipheredText, out saveElapsedTime);
			totalElapsedTime += saveElapsedTime;
			Console.WriteLine ();
			ConsoleManager.Completed ("Successfully generated ciphered text!");
			ConsoleManager.Information ("Total elapsed time: {0:n0} ms", totalElapsedTime);
			return false;
		}

		private static bool ParseArguments (
			string pathToOriginal,
			string pathToKey,
			ref string pathToCipher,
			out byte[] text,
			out byte[] key
		) {
			text = new byte[0];
			key = new byte[0];
			// defaults, for compiler
			text = File.ReadAllBytes (pathToOriginal);
			key = File.ReadAllBytes (pathToKey);
			return CheckFileName (ref pathToCipher);
		}

		private static bool CheckFileName (
			ref string pathToCipher
		) {
			Console.WriteLine ();
			while (File.Exists (pathToCipher)) {
				ConsoleManager.Warning ("Path/To/Cipher already exists");
				ConsoleManager.Information ("Currently specified {0}", pathToCipher);
				string answer = ConsoleManager.Enter ("Continue anyways? [yes, rename, quit]: ").ToLower ();
				if (answer.StartsWith ("y")) {
					File.Delete (pathToCipher);
				} else if (answer.StartsWith ("r")) {
					pathToCipher = ConsoleManager.Enter ("New Path/To/Cipher: ");
				} else if (answer.StartsWith ("q")) {
					return true;
				} else {
					ConsoleManager.Warning ("Invalid answer, please try again");
				}
			}
			return false;
		}

		private static void PrintParams (
			string pathToOriginal,
			byte[] text,
			string pathToKey,
			byte[] key,
			string pathToCipher
		) {
			Console.WriteLine ();
			ConsoleManager.Information ("Path/To/Original: {0}", pathToOriginal);
			ConsoleManager.Information ("Text Length: {0:n0}", text.Length);
			ConsoleManager.Information ("Path/To/Key: {0}", pathToKey);
			ConsoleManager.Information ("Key Length: {0:n0}", key.Length);
			ConsoleManager.Information ("Path/To/Cipher: {0}", pathToCipher);
		}

		private static byte[] XOr (
			byte[] text, 
			byte[] key,
			out long elapsedTime
		) {
			Console.WriteLine ();
			ConsoleManager.Waiting ("Ciphering ...");
			Stopwatch stopwatch = new Stopwatch ();
			ConsoleManager.Information ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			byte[] cipheredText = new byte[text.Length];
			Parallel.For (0, text.Length, i => {
				int index = i % key.Length;
				cipheredText [i] = (byte)(text [i] ^ key [index]);
			});
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			ConsoleManager.Information ("Elapsed time: {0:n0} ms", elapsedTime);
			return cipheredText;
		}

		private static void SaveCipheredText (
			string pathToCipher,
			byte[] cipherText,
			out long elapsedTime
		) {
			Console.WriteLine ();
			ConsoleManager.Waiting ("Saving cipher ... ");
			Stopwatch stopwatch = new Stopwatch ();
			ConsoleManager.Information ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			File.WriteAllBytes (pathToCipher, cipherText);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			ConsoleManager.Information ("Elapsed time: {0:n0} ms", elapsedTime);
		}
	
	}

}
