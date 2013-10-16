//
//  Cairo.R7.Color2.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2013 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Lesser General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Lesser General Public License for more details.
//
//  You should have received a copy of the GNU Lesser General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
// using Gdk;
// using Cairo;

namespace Cairo.R7
{
	public partial struct Color2
	{
		public double R;
		public double G;
		public double B;
		public double A;

		// TODO: Add HSV color and calculated Y

		#region Utils
			
		/// <summary>
		/// Only returns double in [0, 1.0] interval
		/// </summary>
		/// <param name="d">Some double value.</param>
		private static double Norm (double d)
		{
			d = d <= 1.0? d : 1.0;
			d = d >= 0? d : 0.0;

			return d;
		}

		/// <summary>
		/// Converts color given in string representation 
		/// (GDK color name, HTML triplet in #fff and #777777 forms)
		/// to a Color2 structure
		/// </summary>
		/// <returns>Color2 structure.</returns>
		/// <param name="colorString">Color string.</param>
		private static Color2 FromColorString (string colorString)
		{
			// default / fallback
			var color = new Color2 (0, 0, 0);

			try
			{
				if (string.IsNullOrWhiteSpace (colorString))
					throw new Exception ();

				if (colorString.StartsWith ("#", StringComparison.InvariantCultureIgnoreCase))
				{
					int r, g, b;

					if (colorString.Length == "#fff".Length)
					{		
						r = Convert.ToInt32 (colorString.Substring (1, 1), 16);
						g = Convert.ToInt32 (colorString.Substring (2, 1), 16);
						b = Convert.ToInt32 (colorString.Substring (3, 1), 16);

						color.R = (r * 16 + r) / 255.0;
						color.G = (g * 16 + g) / 255.0;
						color.B = (b * 16 + b) / 255.0;

					}
					else if (colorString.Length == "#f0f0f0".Length)
					{
						r = Convert.ToInt32 (colorString.Substring (1, 2), 16);
						g = Convert.ToInt32 (colorString.Substring (3, 2), 16);
						b = Convert.ToInt32 (colorString.Substring (5, 2), 16);

						color.R = r / 255.0;
						color.G = g / 255.0;
						color.B = b / 255.0;
					}
				}
				else // Gdk
				{
					Gdk.Color gdkColor = new Gdk.Color ();
					if (Gdk.Color.Parse (colorString, ref gdkColor))
					{
						color.R = gdkColor.Red / 65535.0;
						color.G = gdkColor.Green / 65535.0;
						color.B = gdkColor.Blue / 65535.0;
					}
				}
			}
			catch
			{
				// set explicit in case some parse operations 
				// were successful, but others were not
				color = new Color2 (0, 0, 0);

				// TODO: Must write to log
				Console.WriteLine ("Warning: Could not parse color, falling back to black");
			}

			return color;
		}

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Cairo.R7.Color2"/> struct.
		/// </summary>
		/// <param name="r">The red component.</param>
		/// <param name="g">The green component.</param>
		/// <param name="b">The blue component.</param>
		/// <param name="a">The alpha component.</param>
		public Color2 (double r, double g, double b, double a = 1.0)
		{
			R = Norm(r);
			G = Norm(g);
			B = Norm(b);
			A = Norm(a);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Cairo.R7.Color2"/> struct.
		/// </summary>
		/// <param name="colorString">Color string.</param>
		/// <param name="a">The alpha component, default is 1.0.</param>
		public Color2 (string colorString, double a = 1.0)
		{
			var color = FromColorString (colorString);
			R = color.R;
			G = color.G;
			B = color.B;
			A = Norm(a); 
		}

		#endregion

		#region Operators and conversion

		// TODO: Add implicit/explicit typecasts to/from Gdk.Color and System.Drawing.Color

		public static explicit operator Cairo.Color (Color2 color2)
		{
			// Explicit operator to convert Color2 values to Cairo.Color
			// Examples:
			// cw.Color = new Color2 ("orange");
			// cw.Context.Color = new Color2 ("orange").ToCairoColor();
			// cw.Context.Color = (Cairo.Color)(new Color2 ("orange"));
		
			// Implicit cast can lead to errors:
			// cw.Context.Color = new Color2 ("orange");
			// so color setting bypass store in ContextWrapper property.

			return new Cairo.Color (
				color2.R, 
				color2.G, 
				color2.B,
				color2.A
			);
		}

		public static implicit operator Color2 (Cairo.Color color)
		{
			// implicit operator to convert Cairo.Color values to Color2
			// Example:
			// Color2 color2 = new Cairo.Color (1, 0, 0);

			return new Color2 (
				color.R, 
				color.G, 
				color.B, 
				color.A
			);
		}


		// TODO: Add method ToSystemDrawingColor()

		public Cairo.Color ToCairoColor ()
		{
			return new Cairo.Color (R, G, B, A);
		}

		public Gdk.Color ToGdkColor ()
		{
			return new Gdk.Color (
				(byte) Math.Round (Norm (R) * 255), 
				(byte) Math.Round (Norm (G) * 255), 
				(byte) Math.Round (Norm (B) * 255)
			); 
		}

		#endregion
	}
}

