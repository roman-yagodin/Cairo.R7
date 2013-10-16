// 
//  Redhound.GLib.Utils.cs
//  
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//  
//  Copyright (c) 2012 Roman M. Yagodin
//
//  Last modified: 12/12/06
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
using GLib;

namespace GLib.R7
{
	/// <summary>
	/// Timer wrapper for GLib.Timeout
	/// </summary>
	public class Timer
	{
		#region TimerWorker

		/// <summary>
		/// One-time timer worker
		/// </summary>
		private class TimerWorker
		{
			private bool enabled = true;
			private Timer host;
			private event EventHandler timerHandler;
	
			/// <summary>
			/// Gets or sets a value indicating whether this <see cref="Redhound.GLib.Timer+TimerWorker"/> is enabled.
			/// </summary>
			/// <value>
			/// <c>true</c> if timer enabled; otherwise, <c>false</c>.
			/// </value>
			public bool Enabled
			{
				get { return enabled; }
				set { enabled = value; }
			}
	
			/// <summary>
			/// Initializes a new instance of the <see cref="Redhound.GLib.Timer+TimerWorker"/> class.
			/// </summary>
			/// <param name='interval'>
			/// Interval, ms.
			/// </param>
			/// <param name='timerHandler'>
			/// Timer handler method.
			/// </param>
			/// <param name='host'>
			/// Host timer object.
			/// </param>
			public TimerWorker (int interval, EventHandler timerHandler, Timer host)
			{
				this.host = host;
				this.timerHandler += new EventHandler(timerHandler);
				global::GLib.Timeout.Add((uint)interval, onTimer);
			}

			private bool onTimer ()
			{
				if (Enabled)
					timerHandler (this.host, new EventArgs());
	
				return Enabled;
			}
		}

		#endregion

		private TimerWorker timer;
		private int interval;
		private EventHandler timerHandler;

		/// <summary>
		/// Gets or sets the interval.
		/// </summary>
		/// <value>
		/// The interval, ms.
		/// </value>
		public int Interval
		{
			get	{ return interval; }
			set
			{  
				if (interval != value)
				{
					// minimum interval is 1
					interval = Math.Max (1, value); 
					if (timer.Enabled)
						Start ();
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="Redhound.GLib.Timer"/> is enabled.
		/// </summary>
		/// <value>
		/// <c>true</c> if timer enabled; otherwise, <c>false</c>.
		/// </value>
		public bool Enabled
		{
			get { return (timer != null) ? timer.Enabled : false; }
			set { if (value) Start (); else  Stop (); }
		}

		/// <summary>
		/// Start this timer.
		/// </summary>
		public void Start ()
		{
			if (timer != null) timer.Enabled = false;
			timer = new TimerWorker (interval, timerHandler, this);
		}

		/// <summary>
		/// Stop this timer.
		/// </summary>
		public void Stop ()
		{
			timer.Enabled = false;
		}

		/// <summary>
		/// Close this timer, <seealso cref="Redhound.GLib.Timer.Stop" /> 
		/// </summary>
		public void Close ()
		{
			timer.Enabled = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Redhound.GLib.Timer"/> class.
		/// </summary>
		/// <param name='interval'>
		/// Interval, ms.
		/// </param>
		/// <param name='timerHandler'>
		/// Timer handler method.
		/// </param>
		/// <param name='startNow'>
		/// Start timer just after create.
		/// </param>
		public Timer (int interval, EventHandler timerHandler, bool startNow = false) 
		{
			this.interval = interval;
			this.timerHandler = timerHandler;

			if (startNow)
				this.Start();
		}
	}
}

