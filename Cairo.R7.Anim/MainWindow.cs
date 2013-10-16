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
using Gtk;
using Cairo;
using Cairo.R7;
using GLib.R7; // for Timer

namespace Cairo.R7.Anim
{
	public partial class MainWindow : Gtk.Window
	{
		public MainWindow () : base(Gtk.WindowType.Toplevel)
		{
			Build ();

			timer = new Timer (50,  OnTimer, true);
		}

		Timer timer;
			
		protected void OnTimer (object sender, EventArgs e)
		{
			drawingarea1.QueueDraw ();
			drawingarea2.QueueDraw ();
			drawingarea3.QueueDraw ();
		}
			
		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}
			
		double alpha = 0;
		Color2 color1 = new Color2 ("blue", 0.5);
		Color2 color2 = new Color2 ("green", 0.5);
		Color2 color3 = new Color2 ("red", 0.5);
			
		private void DrawScene (ContextWrapper cw, double alpha)
		{
			var R = 60.0;
			var dR = R - 10.0;
		
			cw.Translate(cw.Center);

			var scale = Math.Min (cw.Width, cw.Height) / 250.0;

			cw.Context.Scale (scale, scale);
			cw.Context.Rotate(alpha);

			cw.Circle(0, -dR, R);
			cw.Color = color1;
			cw.Context.Fill();
			
			cw.Context.Rotate(Angle.PI(2,3));
			
			cw.Circle(0, -dR, R);
			cw.Color = color2;
			cw.Context.Fill();
			
			cw.Context.Rotate (Angle.PI(2,3));
			
			cw.Circle(0, -dR, R);
			cw.Color = color3;
			cw.Context.Fill();
		}

		bool turn = false;
		
		private void ModifyScene ()
		{
			var cinc = 0.01;

			if (turn) 
			{
				color1 = new Color (0, color1.G + cinc, color1.B - cinc, 0.5);
				color2 = new Color (color2.R + cinc, color2.G - cinc, 0, 0.5);
				color3 = new Color (color3.R - cinc, 0, color3.B + cinc, 0.5);
				
				if (color1.G >= 1.0)
					turn = false;
			}
			else 
			{
				color1 = new Color (0, color1.G - cinc, color1.B + cinc, 0.5);
				color2 = new Color (color2.R - cinc, color2.G + cinc, 0, 0.5);
				color3 = new Color (color3.R + cinc, 0, color3.B - cinc, 0.5);
				
				if (color1.B >= 1.0)
					turn = true;
			}

			alpha += Math.PI / 100;
		}
		
		protected virtual void OnDrawingarea1ExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			var cw = new ContextWrapper (o);

			DrawScene (cw, alpha);
			ModifyScene();

			cw.Close ();
		}

		protected void OnPauseActionToggled (object sender, EventArgs e)
		{
			timer.Enabled = !timer.Enabled;
		}
	}
}
