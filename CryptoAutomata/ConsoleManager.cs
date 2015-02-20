//
//  CryptoAutomata/CryptoAutomata/ConsoleManager.cs
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

namespace CryptoAutomata
{

	static class ConsoleManager
	{

		public static string FormatBytes (
			int bytes
		) {
			const int scale = 1024;
			string[] orders = new string[] {
				"Bytes",
				"KB",
				"MB",
				"GB"
			};
			if (scale > bytes) {
				return String.Format (
					"{0:n0} {1}",
					bytes,
					orders [orders.GetLowerBound (0)]
				);
			}
			int max = (int)Math.Pow(
				scale,
				orders.GetUpperBound (0)
			);
			int working = max;
			for (
				int i = orders.GetUpperBound (0);
				i > orders.GetLowerBound (0);
				i--
			) {
				if (working <= bytes) {
					return String.Format (
						"{0:n2} {1}",
						Decimal.Divide (
							(decimal)bytes,
							(decimal)working
						),
						orders [i]
					);
				}
				working /= scale;
			}
			return String.Format (
				"{0:n2} {1}",
				bytes / max,
				orders [orders.GetUpperBound (0)]
			);
		}

		public static void Neutral ()
		{
			Console.BackgroundColor = ConsoleColor.Black;
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static string Enter (
			string text
		) {
			Console.ForegroundColor = ConsoleColor.Blue;
			Console.Write (text);
			Neutral ();
			return Console.ReadLine ();
		}

		public static void Completed (
			string text
		) {
			Console.ForegroundColor = ConsoleColor.Green;
			Write (text);
		}

		public static void Waiting (
			string text
		) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			Write (text);
		}

		public static void Information (
			string text
		) {
			Neutral ();
			Write (text);
		}

		public static void Information (
			string format,
			params object[] objs
		) {
			Information (
				String.Format (
					format,
					objs
				)
			);
		}

		public static void Warning (
			string text
		) {
			Console.ForegroundColor = ConsoleColor.Red;
			Write (text);
		}

		public static void Header (
			string text
		) {
			Console.WriteLine ();
			Console.ForegroundColor = ConsoleColor.Magenta;
			Write (text);
		}

		private static void Write (
			string text
		) {
			Console.WriteLine (text);
			Neutral ();
		}

	}

}

