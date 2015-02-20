// 
// CryptoAutomata/CryptoAutomata/GenKey.cs
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
using RandomAutomata;

namespace CryptoAutomata
{

	static class GenKey
	{

		public static bool DeriveKey (
			string pathToOriginal,
			ref string pathToKey
		) {
			ConsoleManager.Header ("Entered Key-Deriver");
			Console.WriteLine ();
			string password = GetPassword ();
			ulong seed;
			int length;
			bool exit = ParseArguments (
				password,
				pathToOriginal,
				ref pathToKey,
				out seed,
				out length
			);
			if (exit) {
				ConsoleManager.Warning ("Exiting!");
				return true;
			}
			PrintParams (password, seed, pathToOriginal, length, pathToKey);
			long totalElapsedTime = 0;
			long constructRandomSequenceElapsedTime;
			RandomSequence randomSequence = ConstructRandomSequence (seed, out constructRandomSequenceElapsedTime);
			totalElapsedTime += constructRandomSequenceElapsedTime;
			long getRandomBytesElapsedTime;
			byte[] randomBytes = GetRandomBytes (randomSequence, length, out getRandomBytesElapsedTime);
			totalElapsedTime += getRandomBytesElapsedTime;
			long saveKeyElapsedTime;
			SaveKey (pathToKey, randomBytes, out saveKeyElapsedTime);
			totalElapsedTime += saveKeyElapsedTime;
			Console.WriteLine ();
			ConsoleManager.Completed ("Successfully generated key!");
			ConsoleManager.Information ("Total elapsed time: {0:n0} ms", totalElapsedTime);
			return false;
		}

		private static string GetPassword ()
		{
			string password = ConsoleManager.Enter ("Password (8 to 64 chars): ");
			if ((8 > password.Length) || (64 < password.Length)) {
				ConsoleManager.Warning ("Password invalid, please try again");
				password = GetPassword ();
			}
			return password;
		}

		private static bool ParseArguments (
			string password,
			string pathToOriginal,
			ref string pathToKey,
			out ulong seed,
			out int length
		) {
			// defaults, for compiler
			seed = 0;
			length = 0;
			seed = 0;
			foreach (char c in password) {
				seed <<= 64 / password.Length;
				seed ^= (ulong)c;
			}
			length = File.ReadAllBytes (pathToOriginal).Length;
			return CheckFileName (ref pathToKey);
		}

		private static bool CheckFileName (
			ref string pathToKey
		) {
			while (File.Exists (pathToKey)) {
				Console.WriteLine ();
				ConsoleManager.Warning ("Path/To/Key already exists");
				ConsoleManager.Information ("Currently specified {0}", pathToKey);
				string answer = ConsoleManager.Enter ("Continue anyways? [yes, rename, quit]: ").ToLower ();
				if (answer.StartsWith ("y")) {
					File.Delete (pathToKey);
				} else if (answer.StartsWith ("r")) {
					pathToKey = ConsoleManager.Enter ("New Path/To/Key: ");
				} else if (answer.StartsWith ("q")) {
					return true;
				} else {
					ConsoleManager.Warning ("Invalid answer, please try again");
				}
			}
			return false;
		}

		private static void PrintParams (
			string password, 
			ulong seed,
			string pathToOriginal, 
			int length,
			string pathToKey
		) {
			Console.WriteLine ();
			ConsoleManager.Information ("Password: {0}", password);
			ConsoleManager.Information ("Seed: {0:n0}", seed);
			ConsoleManager.Information ("Path/To/Original: {0}", pathToOriginal);
			ConsoleManager.Information ("Length: {0:n0}", length);
			ConsoleManager.Information ("Path/To/Key: {0}", pathToKey);
		}

		private static RandomSequence ConstructRandomSequence (
			ulong seed,
			out long elapsedTime
		) {
			Console.WriteLine ();
			ConsoleManager.Waiting ("Constructing RandomSequence generator ...");
			Stopwatch stopwatch = new Stopwatch ();
			ConsoleManager.Information ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			RandomSequence randomSequence = new RandomSequence (seed);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			ConsoleManager.Information ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomSequence;
		}

		private static byte[] GetRandomBytes (
			RandomSequence randomSequence,
			int length,
			out long elapsedTime
		) {
			Console.WriteLine ();
			ConsoleManager.Waiting ("Getting random bytes ... ");
			Stopwatch stopwatch = new Stopwatch ();
			ConsoleManager.Information ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			byte[] randomBytes = randomSequence.GetNextBytes (length);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			ConsoleManager.Information ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomBytes;
		}

		private static void SaveKey (
			string pathToKey,
			byte[] randomBytes, 
			out long elapsedTime
		) {
			Console.WriteLine ();
			ConsoleManager.Waiting ("Saving key ... ");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			ConsoleManager.Information ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			File.WriteAllBytes (pathToKey, randomBytes);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			ConsoleManager.Information ("Elapsed time: {0:n0} ms", elapsedTime);
		}

	}

}

