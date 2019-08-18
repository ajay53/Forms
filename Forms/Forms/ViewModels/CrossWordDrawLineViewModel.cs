using CrossWordHelper;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Forms.ViewModels
{
    public class CrossWordDrawLineViewModel
    {
        public static SKCanvasView canvas;
        public static SKPoint touchPressed;
        public static SKPoint touchReleased;

        SKPaint textPaint;
        SKPaint grayLinePaint;
        SKPaint redLinePaint;
        SKPaint bluelinePaint;
        SKPaint greenLinePaint;
        SKPaint transparentLinePaint;
        static SKPaint randomLinePaint;

        static List<SKPaint> linePaintList;
        static Random rnd = new Random();

        public static bool clearCanvas;
        public static bool changeLineColor = true;
        SKImageInfo info;
        SKSurface surface;
        SKCanvas skcanvas;

        public CrossWordDrawLineViewModel(SKCanvasView a_canvas)
        {
            canvas = a_canvas;
            canvas.PaintSurface += Canvas_PaintSurface;

            textPaint = new SKPaint
            {
                Color = SKColors.Black,
                TextSize = 75,
                TextAlign = SKTextAlign.Center
            };

            bluelinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.BlueViolet,
                StrokeWidth = 100,
                StrokeCap = SKStrokeCap.Round
            };

            greenLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.LawnGreen,
                StrokeWidth = 100,
                StrokeCap = SKStrokeCap.Round
            };

            redLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.OrangeRed,
                StrokeWidth = 100,
                StrokeCap = SKStrokeCap.Round
            };

            grayLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.SlateGray,
                StrokeWidth = 100,
                StrokeCap = SKStrokeCap.Round
            };

            transparentLinePaint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = SKColors.Transparent,
                StrokeWidth = 100,
                StrokeCap = SKStrokeCap.Round
            };

            linePaintList = new List<SKPaint>(new SKPaint[] {
                grayLinePaint, redLinePaint, bluelinePaint, greenLinePaint
            });

            MessagingCenter.Subscribe<MessageCenterHelper, Type>(this, "Sample", (sender, args) =>
            {
                Console.WriteLine("Message center issue");

                ValidateSelectedButtons();
            });
            
        }

        private void Canvas_PaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            info = args.Info;
            surface = args.Surface;
            skcanvas = surface.Canvas;
            Console.WriteLine("working");

            if (clearCanvas)
            {
                skcanvas.Clear();
                clearCanvas = false;
                return;
            }

            if (changeLineColor)
            {
                randomLinePaint = transparentLinePaint;
            }

            skcanvas.DrawLine(touchPressed, touchReleased, randomLinePaint);
        }

        public static void changeLineColorMethod()
        {
                int randomNumber = rnd.Next(linePaintList.Count);
                randomLinePaint = linePaintList.ElementAt(randomNumber);
        }

        void ValidateSelectedButtons()
        {
            var x = Math.Abs(Math.Ceiling(touchPressed.X - touchReleased.X));
            var y = Math.Abs(Math.Ceiling(touchPressed.Y - touchReleased.Y));

            //for elements at x and y axis
            if (touchPressed.X == touchReleased.X || touchPressed.Y == touchReleased.Y || Math.Abs(x - y) < 5)
            {
                canvas.InvalidateSurface();
            }

            //for elements at z axis
            //else if (Math.Abs(x - y) < 5)
            //{
            //    canvas.InvalidateSurface();
            //}

            else if (clearCanvas)
            {
                canvas.InvalidateSurface();
            }
        }
    }
}
