//
//  MyClass.cs
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
using System.Collections.Generic;
using Cairo;

namespace Cairo.R7
{
	// TODO: Try make ContextWrapper IDisposable
	// TODO: Add arrows

	public class ContextWrapper
	{
		#region Context

		protected Context context;

		public Context Context 
		{
			get { return context; }
		}

		#endregion

		#region Dimensions, center and corners

		private int width;
		private int height;

		public int Width 
		{
			get { return width; }
		}
		public int Height
		{
			get { return height; }
		}

		public PointD Center 
		{
			get { return new PointD (width / 2.0, height / 2.0); }
		}

		public PointD TopLeft 
		{
			get { return new PointD (0, 0); }
		}

		public PointD TopRight 
		{
			get { return new PointD (width - 1, 0); }
		}

		public PointD BottomLeft 
		{
			get { return new PointD (0, height - 1); }
		}

		public PointD BottomRight
		{
			get { return new PointD (width - 1, height - 1); }
		}

		// TODO: Xlib (wigdet) and Image surfaces have Width and Height, others may not?

		/*
		public bool HasLimits 
		{
			get { context.Target == SurfaceType.Xlib; }
		}*/

		#endregion

		#region Constructors

		protected ContextWrapper ()
		{
			matrixStack = new Stack<Matrix> ();
			colorStack = new Stack<Color2> ();
		}

		public ContextWrapper (Gtk.Widget w) : this ()
		{
			context = Gdk.CairoHelper.Create (w.GdkWindow);

			width = w.Allocation.Width;
			height = w.Allocation.Height;
		}

		public ContextWrapper (object o) : this ()
		{ 
			if (o is Gtk.Widget)
			{
				context = Gdk.CairoHelper.Create ((o as Gtk.Widget).GdkWindow);

				width = (o as Gtk.Widget).Allocation.Width;
				height = (o as Gtk.Widget).Allocation.Height;
			}
			else if (o is Surface)
			{
				context = new Context (o as Surface);

				if (o is ImageSurface)
				{
					width = (o as ImageSurface).Width;
					height = (o as ImageSurface).Height;
				}
			}
		}

		public ContextWrapper (Surface surface) : this ()
		{
			context = new Context (surface);

			if (surface is ImageSurface)
			{
				width = (surface as ImageSurface).Width;
				height = (surface as ImageSurface).Height;
			}
		}

		public ContextWrapper (Context context) : this ()
		{
			this.context = context;

			if (this.context.Target is ImageSurface)
			{
				width = (this.context.Target as ImageSurface).Width;
				height = (this.context.Target as ImageSurface).Height;
			}
		}

		#endregion

		public void Close()
		{
			((IDisposable)context.Target).Dispose ();
			((IDisposable)context).Dispose ();
		}

		#region Transform matrix stack

		protected Stack<Matrix> matrixStack;

		public void PushMatrix ()
		{
			matrixStack.Push(context.Matrix);
		}

		public void PushMatrix (Matrix m)
		{
			matrixStack.Push (m);
		}

		public Matrix PopMatrix()
		{
			context.Matrix = matrixStack.Pop ();

			return context.Matrix;
		}

		public Matrix PeekMatrix()
		{
			return matrixStack.Peek();
		}

		#endregion

		#region Color & color stack

		protected Stack<Color2> colorStack;	

		private Color2 color = new Color2 (0, 0, 0);

		public Color2 Color
		{
			get { return color; }
			set 
			{
				// prevColor = color;
				color = value;
				context.Color = value.ToCairoColor();
			}
		}

		public void PushColor ()
		{
			colorStack.Push (color);
		}

		public void PushColor (Color2 color2)
		{
			colorStack.Push (color2);
		}

		public Color2 PopColor ()
		{
			this.Color = colorStack.Pop ();

			return this.Color;
		}

		public Color2 PeekColor ()
		{
			return colorStack.Peek ();
		}

		#endregion

		#region Draw operations

		// TODO: Native Ellipse (without transforms).

		/// <summary>
		/// Draws a circle with the specified center (xc, yc) and radius on Cairo context
		/// </summary>
		/// <param name='xc'>
		/// Circle center X
		/// </param>
		/// <param name='yc'>
		/// Circle center Y
		/// </param>
		/// <param name='radius'>
		/// Circle radius.
		/// </param>
		public void Circle (double xc, double yc, double radius)
		{
			context.Arc (xc, yc, radius, 0, 2 * Math.PI);
		}

		public void Circle (PointD center, double radius)
		{
			context.Arc (center.X, center.Y, radius, 0, 2 * Math.PI);
		}

		public void RoundedRectangle (Rectangle rect, double r)
		{
			RoundedRectangle (rect.X, rect.Y, rect.Width, rect.Height, r);
		}

		public void RoundedRectangle (PointD point,double width, double height, double r)
		{
			RoundedRectangle (point.X, point.Y, width, height, r);
		}

		public void RoundedRectangle (double x, double y, double width, double height, double r)
		{
			context.NewPath ();

			context.Arc (x + width - r, y + r, r, -90 * Angle.Degrees, 0);
			context.Arc (x + width - r, y + height - r, r, 0, 90 * Angle.Degrees);
			context.Arc (x + r, y + height - r, r, 90 * Angle.Degrees, 180 * Angle.Degrees);
			context.Arc (x + r, y + r, r, 180 * Angle.Degrees, 270 * Angle.Degrees);

			context.ClosePath ();
		}
				
		public void Polyline (ICollection<PointD> points)
		{
			if (points.Count > 0)
			{
				var first = true;

				foreach (var point in points)
				{
					if (first)
					{
						context.MoveTo (point);
						first = false;
					}
					else
						context.LineTo (point);
				}
			}
		}

		public void Polyline (PointD[] points)
		{
			Polyline((ICollection<PointD>)points);
		}

		public void Polygon (ICollection<PointD> points)
		{
			if (points.Count > 0)
			{
				context.NewPath ();
				var first = true;

				foreach (var point in points)
				{
					if (first)
					{
						context.MoveTo (point);
						first = false;
					}
					else
						context.LineTo (point);
				}
				context.ClosePath ();
			}
		}

		public void Polygon (PointD[] points)
		{
			Polygon((ICollection<PointD>)points);
		}

		public void RegularPolygon (double xc, double yc, double radius, int parts)
		{
			// 2 variants

		}

		public void StarPolygon (double xc, double yc, double innerRadius, double outerRadius, int parts)
		{
			// 2 variants
		}


		#endregion

		/// <summary>
		/// Sets the clip rectangle from Gdk.Rectangle.
		/// </summary>
		/// <param name='context'>
		/// Cairo context.
		/// </param>
		/// <param name='rect'>
		/// Rectangle
		/// </param>
		public void SetClipRectangle (Gdk.Rectangle rect)
		{
			context.Translate (rect.Left, rect.Top);
			context.Rectangle (0, 0, rect.Width, rect.Height);
			context.Clip ();
		}

		/// <summary>
		/// Sets the clip rectangle from Cairo.Rectangle.
		/// </summary>
		/// <param name='context'>
		/// Cairo context.
		/// </param>
		/// <param name='rect'>
		/// Rectangle
		/// </param>
		public void SetClipRectangle (Cairo.Rectangle rect)
		{
			context.Translate (rect.X, rect.Y);
			context.Rectangle (0, 0, rect.Width, rect.Height);
			context.Clip ();
		}

		/*
		public static void SetClipRectangle (Context context, Widget widget)
		{
			int x, y, w, h;

			widget.GdkWindow.GetRootOrigin (out x, out y);
			widget.GdkWindow.GetSize (out w, out h);

			Console.WriteLine (h);
			context.Translate (0,0);
			context.Rectangle (20, 2, w-40,h-16);
			//context.Clip ();
			context.Stroke ();
		}
		*/

		#region Transformations

		/*
		public void TranslateFix ()
		{
			context.Translate (0.5, 0.5);
		}
		 */

		public void Translate (PointD point)
		{
			context.Translate (point.X, point.Y);
		}

		#endregion

		/*
		public static Context operator~ (ContextWrapper cw)
		{
			return cw.context;
		}*/
	}
}

