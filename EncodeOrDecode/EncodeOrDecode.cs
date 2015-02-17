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

namespace CryptoAutomata.EncodeOrDecode
{

	class EncodeOrDecode
	{

		public static void Main (string[] args)
		{
			byte[] text;
			byte[] key;
			string path;
			bool exit = GetParams (args, out text, out key, out path);
			if (exit) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Exiting ...");
				Console.ResetColor ();
				return;
			}
			PrintParams (args, text, key, path);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine ("Generating ciphered text ...");
			Console.ResetColor ();
			long totalElapsedTime = 0;
			long cipherTextElapsedTime;
			byte[] cipheredText = XOr (text, key, out cipherTextElapsedTime);
			totalElapsedTime += cipherTextElapsedTime;
			long saveElapsedTime;
			SaveCipheredText (path, cipheredText, out saveElapsedTime);
			totalElapsedTime += saveElapsedTime;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Successfully generated cipheredText!");
			Console.ResetColor ();
			Console.WriteLine ("Total elapsed time: {0:n0} ms", totalElapsedTime);
		}

		private static bool GetParams (string[] args, out byte[] text, out byte[] key, out string path)
		{
			// defaults, for compiler
			text = new byte[0];
			key = new byte[0];
			path = String.Empty;

			try {
				if ((!File.Exists (args [0])) || (!File.Exists (args [1]))) {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("Text or Key path specified does not exist!");
					Console.ResetColor ();
					return true;
				}
				text = File.ReadAllBytes (args [0]);
				key = File.ReadAllBytes (args [1]);
				path = args [2];
			} catch (IndexOutOfRangeException) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Wrong number of arguments specified, requires exactly 3!");
				Console.ResetColor ();
				return true;
			} catch (FormatException) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Arguments of invalid wrong format!");
				Console.ResetColor ();
				return true;
			}
			return CheckFileName (path);
		}

		private static bool CheckFileName (string path)
		{
			if (File.Exists (path)) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write ("Filename {0} already exists, continue anyways? [Y, n]: ", path);
				Console.ResetColor ();
				ConsoleKey consoleKey = Console.ReadKey (false).Key;
				if ((ConsoleKey.Y == consoleKey) || (ConsoleKey.Enter == consoleKey)) {
					if (ConsoleKey.Enter != consoleKey) {
						Console.WriteLine ();
					}
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine ("Original will file be overwritten");
					Console.ResetColor ();
				} else {
					Console.WriteLine ();
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.ResetColor ();
					return true;
				}
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Will save ciphered text as filename: {0}", path);
			Console.ResetColor ();
			return false;
		}

		private static void PrintParams (string[] args, byte[] text, byte[] key, string path)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Parameters valid:");
			Console.ResetColor ();
			Console.WriteLine ("Path/To/Text: {0}", args [0]);
			Console.WriteLine ("Text Length: {0:n0}", text.Length);
			Console.WriteLine ("Path/To/Key: {0}", args [1]);
			Console.WriteLine ("Key Length: {0:n0}", key.Length);
			Console.WriteLine ("Path/To/CipheredText: {0}", path);
		}

		private static byte[] XOr (byte[] text, byte[] key, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Ciphering ...");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			byte[] cipheredText = new byte[text.Length];
			Parallel.For (0, text.Length, i => {
				int index = i % key.Length;
				cipheredText [i] = (byte)(text [i] ^ key [index]);
			});
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return cipheredText;
		}

		private static void SaveCipheredText (string path, byte[] cipheredText, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Saving ciphered text at path: {0} ... ", path);
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			File.WriteAllBytes (path, cipheredText);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
		}
	
	}

}
