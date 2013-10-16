#undef SYSTEM_DRAWING

#if SYSTEM_DRAWING

using System;
using System.Drawing; // add reference!

namespace Cairo.R7
{
	public partial struct Color2
	{
		public Color2 (System.Drawing.Color color)
		{
			R = color.R / 255.0;
			G = color.G / 255.0;
			B = color.B / 255.0;
			A = color.A / 255.0;
		}

		public Color2 (System.Drawing.KnownColor knownColor)
		{
			var color = System.Drawing.Color.FromKnownColor (knownColor);

			R = color.R / 255.0;
			G = color.G / 255.0;
			B = color.B / 255.0;
			A = color.A / 255.0;
		}

		// TODO: Add method ToSystemDrawingColor()
	}
}
#endif

