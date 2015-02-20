//
//  CryptoAutomata/CryptoAutomata/CryptoAutomata.cs
//
//  Author:
//       Isabell Cowan <isabellcowan@gmail.com>
//
//  Copyright (c) 2015 Isabell Cowan
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;

namespace CryptoAutomata
{

	public static class CryptoAutomata
	{

		public static void Main ()
		{
			ConsoleManager.Neutral ();
			ConsoleManager.Header ("CryptoAutomata v1.1.0");
			ConsoleManager.Information ("Isabell Cowan -- 2015");
			ConsoleManager.Information ("<isabellcowan@gmail.com>");
			string fileName;
			bool exit = GetArguments (out fileName);
			if (Exit (exit)) {
				return;
			}
			string keyFileName = (
				tempDirName
				+ "/"
				+ fileName
				+ ".key"
			);
			exit = GenKey.DeriveKey (
				fileName,
				ref keyFileName
			);
			if (Exit (exit)) {
				return;
			}
			string cipherFileName = GetCipherFileName (fileName);
			exit = EncodeOrDecode.Cipher (
				fileName,
				keyFileName,
				ref cipherFileName
			);
			if (Exit (exit)) {
				return;
			}
			Console.WriteLine ();
			ConsoleManager.Completed ("CryptoAutomata job complete!");
			Exit (true);
		}

		private const string tempDirName = "temp";
		private static readonly DirectoryInfo tempDir = (
			Directory.CreateDirectory (tempDirName)
		);

		private static bool GetArguments (out string pathToOriginal)
		{
			Console.WriteLine ();
			ConsoleManager.Information ("Files in current directory:");
			string[] filesInCurrentDirectory = Directory.GetFiles (
				Directory.GetCurrentDirectory ()
			);
			foreach (string file in filesInCurrentDirectory) {
				string[] splitFile = file.Split (
					new char[2] { '/', '\\' }
				);
				string relativePath = splitFile [splitFile.GetUpperBound (0)];
				if (
					(relativePath.StartsWith ("RandomAutomata"))
					|| (relativePath.StartsWith ("CryptoAutomata"))
				) {
					continue;
				}
				ConsoleManager.Information (relativePath);
			}
			pathToOriginal = ConsoleManager.Enter (
				"Enter filename to cipher (including extention): "
			);
			return CheckFileName (ref pathToOriginal);
		}

		private static bool CheckFileName (ref string pathToOriginal)
		{
			while (!File.Exists (pathToOriginal)) {
				ConsoleManager.Warning ("Path/To/Original does not exist");
				ConsoleManager.Information (
					"Currently specified: {0}",
					pathToOriginal
				);
				string answer = ConsoleManager.Enter (
					"How to continue? [rename, quit]: "
				);
				answer = answer.ToLower ();
				if (answer.StartsWith ("r")) {
					pathToOriginal = ConsoleManager.Enter (
						"New Path/To/Original: "
					);
				} else if (answer.StartsWith ("q")) {
					return true;
				} else {
					ConsoleManager.Warning ("Invalid answer, please try again");
				}
			}
			ConsoleManager.Completed ("Found Path/To/Original!");
			return false;
		}

		private static string GetCipherFileName (string fileName)
		{
			string cipherFileName;
			if (fileName.EndsWith (".crypt")) {
				cipherFileName = fileName.Remove (
					fileName.Length - 6,
					6
				);
			} else {
				cipherFileName = fileName + ".crypt";
			}
			return cipherFileName;
		}

		private static bool Exit (bool exit)
		{
			if (!exit) {
				return false;
			}
			tempDir.Delete (true);
			Console.WriteLine ();
			ConsoleManager.Waiting ("Press enter to exit ...");
			Console.ReadLine ();
			return true;
		}
	
	}

}
