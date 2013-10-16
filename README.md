cairo-r7
========

Mono.Cairo wrapper and utility library for .NET / Mono

About
=====

Mono.Cairo gives us ability to use great Cairo 2D graphics library features in .NET or Mono programs, 
but it lacks some flexibility - at least, to my taste, but I think that many people who using Mono.Cairo 
in they projects feel that way too.

Cairo.R7 project is about to make Mono.Cairo more friendly by extending Cairo.Context with some useful 
drawing methods and other features (like matrix and color stacks), and also providing some additional 
structures and classes in a toolbox - all just to bring more fun in drawing with Cairo.

Warning: The project code is in early stage of development, so API is not complete and not stable. 

Example
=======

A basic sample (original Mono.Cairo): 

<pre>using Cairo;
...

protected void OnDrawingAreaExposeEvent (object o, Gtk.ExposeEventArgs args)
{
   var gc = Gdk.CairoHelper.Create ((o as Widget).GdkWindow);
   
   var center = new PointD ( 
      (o as Widget).Allocation.Width,
      (o as Widget).Allocation.Heigth
   ); 
    
   gc.Translate (center.X, center.Y);
   gc.Arc (0, 0, 100, 0, 2 * Math.PI);

   gc.Color = new Color (1, 1, 0);
   gc.FillPreserve();
   gc.Color = new Color (0, 0, 0);
   gc.Stroke();

   ((IDisposable)gc.Target).Dispose ();
   ((IDisposable)gc).Dispose ();
}</pre>

A basic sample (using Cairo.R7): 

<pre>using Cairo;
using Cairo.R7;
...

protected void OnDrawingAreaExposeEvent (object o, Gtk.ExposeEventArgs args)
{
   var cw = new ContextWrapper(o);
   
   cw.Translate (cw.Center);
   cw.Circle (0, 0, 100);
    
   cw.Color = new Color2 ("yellow");
   cw.Context.FillPreserve();
   cw.Color = new Color2 ("#000");
   cw.Context.Stroke();

   cw.Close();
}</pre>
