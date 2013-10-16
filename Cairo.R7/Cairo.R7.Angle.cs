//
//  Cairo.R7.Angle.cs
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

namespace Cairo.R7
{
	public class Angle
	{
		/// <summary>
		/// Radians to degrees multiplier.
		/// </summary>
		public static double Degrees = Math.PI / 180;

		/// <summary>
		/// Degrees to radians multiplier.
		/// </summary>
		public static double Radians = 180 / Math.PI;

		/// <summary>
		/// PI fraction.
		/// </summary>
		/// <param name="numerator">Fraction numerator.</param>
		/// <param name="denominator">Fraction denominator.</param>
		/// <example>var pi32 = Angle.PI(3,2);</example>
		public static double PI (double numerator, double denominator)
		{
			return numerator * Math.PI / denominator;
		}
	}
}

