//
//  MainWindow.cs
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
using Gtk;
using Cairo;
using Cairo.R7;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnDrawingarea1ExposeEvent (object o, ExposeEventArgs args)
	{
		var cw = new ContextWrapper (o);

		cw.Translate (cw.Center);

		cw.Context.LineWidth = 1;

		cw.Circle (0, 0, 50);

		cw.Context.StrokePreserve ();

		cw.PushColor ();
		cw.Color = new Color2 ("#FFC0CB");
		cw.Context.Fill ();


		cw.Circle (-50, -50, 100);

		cw.PopColor ();
		cw.Context.StrokePreserve ();
		cw.Color = new Color2 ("yellow", 0.5);
		cw.Context.Fill ();

		cw.Color = new Color2 ("black");
		cw.RoundedRectangle (new Rectangle(50, 50, 100, 50), 15);
		cw.Context.Stroke ();

		cw.Context.IdentityMatrix ();
		cw.Translate (cw.Center);

		var rnd = new Random ();
		var points = new PointD[15];
		for (var i = 0; i < points.Length; i++) {
			var p = new PointD (rnd.Next (200) - 100, rnd.Next (200) - 100);
			points[i] = p;
		}

		cw.Polygon (points);
		cw.Context.StrokePreserve ();
		cw.Context.Fill ();



		cw.Close ();
	}
}
