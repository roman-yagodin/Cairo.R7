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
using GLib.R7;

namespace Cairo.R7.Bubbles
{
	public partial class MainWindow : Gtk.Window
	{
		Bubble[] bubbles;
		Random rnd = new Random ();

		public MainWindow () : base(Gtk.WindowType.Toplevel)
		{
			Build ();

			InitScene (drawingarea1.Allocation.Width, drawingarea1.Allocation.Height);

			timer = new Timer (10, OnTimer, true);
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		Timer timer;
		int pass = 0;

		protected void OnTimer (object sender, EventArgs e)
		{
			if (pass == 0)
				drawingarea1.QueueDraw ();
					
			var slowness = 1 + (int)scaleSpeed.Adjustment.Upper - (int)scaleSpeed.Value;
			pass = (pass + 1) % slowness;
		}

		void InitScene (double w, double h)
		{
			bubbles = new Bubble[rnd.Next (20, 35)];

			for (int i=0; i < bubbles.Length; i++)
			{
				bubbles[i] = new Bubble ()
				{
					R = rnd.NextDouble () * 25 + 7,
					VX = rnd.NextDouble () * 2.0 - 1.0,
					VY = rnd.NextDouble () * 2.0 - 1.0,
					IsEaten = false,
					Color = new Color 
					(
						rnd.NextDouble (), 
						rnd.NextDouble (), 
						rnd.NextDouble (), 
						0.75
					),

					Center = new PointD (rnd.NextDouble () * w, rnd.NextDouble () * h)
				};
			}
		}

		private void DrawScene (ContextWrapper cw)
		{
			foreach (var b in bubbles)
				b.Draw(cw);
		}

		private void ModifyScene (double w, double h)
		{
			int bubblesLeft = 0;
			foreach (Bubble b in bubbles)
				if (!b.IsEaten)
					bubblesLeft++;
				
			if (bubblesLeft < 2)
			{	
				InitScene (w, h);
			}

			foreach (Bubble b in bubbles)
			{
				if (!b.IsEaten)
				{
					b.Move (w, h);
				
					foreach (Bubble b2 in bubbles)
						if (b.CanEatBubble(b2))
							b.EatBubble (b2);
				}
			}
		}

		protected virtual void OnDrawingarea1ExposeEvent (object o, Gtk.ExposeEventArgs args)
		{
			var cw = new ContextWrapper (o);

			// draw scene
			DrawScene (cw);

			// modify scene params
			ModifyScene (cw.Width, cw.Height);
			
			cw.Close ();
		}

		protected void OnScaleSpeedValueChanged (object sender, EventArgs e)
		{
			// stop / start timer depending of speed value
			timer.Enabled = (int)scaleSpeed.Value > 0;
		}

		protected void OnResetActionActivated (object sender, EventArgs e)
		{
			// kill all bubbles
			foreach (var b in bubbles)
				b.IsEaten = true;
		}
	}
}
