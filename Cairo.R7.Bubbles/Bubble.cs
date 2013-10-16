//
//  Bubble.cs
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
using Cairo;
using Cairo.R7;

namespace Cairo.R7.Bubbles
{
	public class Bubble
	{
		// radius
		public double R;
		// center coords
		public PointD Center;
		// speeds
		public double VX;
		public double VY;
		// color
		public Color2 Color;
		// true, if already eaten
		public bool IsEaten;

		/// <summary>
		/// Bubble area
		/// </summary>
		/// <value>The s.</value>
		public double S
		{
			get { return Math.PI * R * R; }
		}

		public bool CanEatBubble (Bubble b)
		{
			if (!b.IsEaten)
			{
				var r2 = (this.Center.X - b.Center.X) * (this.Center.X - b.Center.X) + 
					(this.Center.Y - b.Center.Y) * (this.Center.Y - b.Center.Y);

				if (r2 < (this.R + b.R) * (this.R + b.R))
					if (this.R > b.R)
						return true;
			}
			return false;
		}

		/// <summary>
		/// How this bubble eats another bubble.
		/// </summary>
		/// <param name="b">The blue component.</param>
		public void EatBubble (Bubble b)
		{
			// area ratio
			var Sd = b.S / this.S;
			b.IsEaten = true;

			// mixing colors (depending on area ratio)
			this.Color = new Color (
				(b.Color.R * Sd + this.Color.R * (1 - Sd)), 
				(b.Color.G * Sd + this.Color.G * (1 - Sd)), 
				(b.Color.B * Sd + this.Color.B * (1 - Sd)));

			// increase radius (depending on area ratio)
			this.R = Math.Sqrt ((this.S + b.S) / Math.PI);

			// calculating speed (depending on area / "mass" ratio)
			this.VX += b.VX * Sd;
			this.VY += b.VY * Sd;
		}

		/// <summary>
		/// Draw the bubble.
		/// </summary>
		/// <param name="cw">Cairo context wrapper.</param>
		public void Draw (ContextWrapper cw)
		{
			if (!IsEaten)
			{
				cw.Color = Color;
				cw.Circle (Center.X, Center.Y, R);
				cw.Context.Fill ();
			}
		}

		/// <summary>
		/// Move bubble a bit according to its speed
		/// </summary>
		/// <param name="w">The width.</param>
		/// <param name="h">The height.</param>
		public void Move (double w, double h)
		{
			Center.X += VX;
			Center.Y += VY;

			if (Center.X > w || Center.X < 0)
				VX = -VX;

			if (Center.Y > h || Center.Y < 0)
				VY = -VY;
		}
	}

}

