// 
// CryptoAutomata/GenKey/GenKey.cs
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
using System.IO;
using System.Threading.Tasks;
using RandomAutomata;
using System.Text;
using System.Diagnostics;

namespace CryptoAutomata.GenKey
{
	public static class GenKey
	{
		public static void Main (string[] args)
		{
			ulong seed;
			int length;
			string path;
			bool exit = GetParams (args, out seed, out length, out path);
			if (exit) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine ("Exiting ...");
				Console.ResetColor ();
				return;
			}
			PrintParams (seed, length, path);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine ("Generating key ...");
			Console.ResetColor ();
			long totalElapsedTime = 0;
			long constructRandomSequenceElapsedTime;
			RandomSequence randomSequence = ConstructRandomSequence (seed, out constructRandomSequenceElapsedTime);
			totalElapsedTime += constructRandomSequenceElapsedTime;
			long getRandomBytesElapsedTime;
			byte[] randomBytes = GetRandomBytes (randomSequence, length, out getRandomBytesElapsedTime);
			totalElapsedTime += getRandomBytesElapsedTime;
			long saveKeyElapsedTime;
			SaveKey (path, randomBytes, out saveKeyElapsedTime);
			totalElapsedTime += saveKeyElapsedTime;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Successfully generated key!");
			Console.ResetColor ();
			Console.WriteLine ("Total elapsed time: {0:n0} ms", totalElapsedTime);
		}

		private static bool GetParams (string[] args, out ulong seed, out int length, out string path)
		{
			// defaults, for compiler
			seed = 0;
			length = 0;
			path = String.Empty;
			try {
				seed = Convert.ToUInt64 (File.ReadAllText (args [0]));
				if (!File.Exists (args [1])) {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine ("Text does not exist!");
					Console.ResetColor ();
					return true;
				}
				length = File.ReadAllBytes (args [1]).Length;
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
			Console.WriteLine ("Will save key as filename: {0}", path);
			Console.ResetColor ();
			return false;
		}

		private static void PrintParams (ulong seed, int length, string path)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Parameters valid:");
			Console.ResetColor ();
			Console.WriteLine ("Seed: {0:n0}", seed);
			Console.WriteLine ("Length: {0:n0}", length);
			Console.WriteLine ("Path/To/Key: {0}", path);
		}

		private static RandomSequence ConstructRandomSequence (ulong seed, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Constructing RandomSequence generator ...");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			RandomSequence randomSequence = new RandomSequence (seed);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomSequence;
		}

		private static byte[] GetRandomBytes (RandomSequence randomSequence, int length, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Getting NextBytes for length: {0:n0} ... ", length);
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			byte[] randomBytes = randomSequence.GetNextBytes (length);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomBytes;
		}

		private static void SaveKey (string path, byte[] randomBytes, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Saving key at path: {0} ... ", path);
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			File.WriteAllBytes (path, randomBytes);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
		}

	}

}

