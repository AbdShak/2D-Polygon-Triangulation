using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPolygonTriangulation
{
    class Program
    {
        static void Main(string[] args)
        {
            float bitmapWidth = 0.0f;

            #region "Read Input from polygonLines.txt file"
            string input = string.Empty;
            try
            {
                using (StreamReader sr = new StreamReader("polygonLines.txt"))
                {
                    input = sr.ReadToEnd();
                    Console.WriteLine("Input : " + input);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Can not read The file : ");
                Console.WriteLine(e.Message);
            }
            #endregion

            //(note: I separated this step form the next step so the code will be more readable)
            #region "Convert input(string) to (float)"
            string[] data = input.Split(' ');
            //List contains line Length & line Angle 
            List<Tuple<float, float>> linesData = new List<Tuple<float, float>>();

            if (data[0] != string.Empty)
            {
                int numberOfLines = Convert.ToInt16(data[0]);

                for (int i = 1, c = 0; i < numberOfLines * 2; i = i + 2, c++)
                {
                    linesData.Add(new Tuple<float, float>((float)Convert.ToDouble(data[i]), (float)Convert.ToDouble(data[i + 1])));
                    //we will use it later to create Bitmap image
                    bitmapWidth += linesData[c].Item1;
                }
            }
            #endregion


            #region "Get the Polygon vertices from the line Length & line Angle"
            //List contains vertices
            List<PointF> points = new List<PointF>();
            //Add the strating point ( in the middle of our bitmapimage)
            points.Add(new PointF(bitmapWidth / 2, bitmapWidth / 2));

            double angle = 0;
            for (int j = 0; j < linesData.Count; j++)
            {
                angle = angle + (Math.PI * linesData[j].Item2) / -180;
                points.Add(new PointF(points[j].X + linesData[j].Item1 * (float)Math.Cos(angle), points[j].Y + linesData[j].Item1 * (float)Math.Sin(angle)));
            }

            //close the shape by drawing a line from last point to start point if it's not already closed
            if (!points.First().Equals(points.Last()))
            {
                points.Add(points.First());
            }
            #endregion


            #region "Draw"
            //Create Bitmap image
            using (var bitmap = new Bitmap(Convert.ToInt16(bitmapWidth), Convert.ToInt16(bitmapWidth)))
            //Draw images
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                //Draw lines for the Original Polygon
                graphics.DrawLines(new Pen(Color.Black, 1.0f), points.ToArray());
                //Give a path for the Original Polygon
                var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OriginalPolygon.png");
                //Save it
                bitmap.Save(path);

                //Start preparing for Triangulation by sending only the vertices
                points.RemoveAt(points.Count - 1);
                Polygon p = new Polygon((points).ToArray());
                List<Triangle> triangles = p.Triangulate();

                //Counter for naming
                int counter = 1;

                //Draw triangles ( you can comment this part )
                foreach (var triangle in triangles)
                {
                    using (var bitmap2 = new Bitmap(Convert.ToInt16(bitmapWidth), Convert.ToInt16(bitmapWidth)))
                    using (var graphics2 = Graphics.FromImage(bitmap2))
                    {
                        graphics2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        graphics2.DrawPolygon(new Pen(Color.Black, 1.0f), triangle.Points.ToArray());
                        path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Triangle" + counter.ToString() + ".png");
                        bitmap2.Save(path);
                        counter++;
                    }
                }

                //Draw Triangulated Polygon
                using (var bitmap2 = new Bitmap(Convert.ToInt16(bitmapWidth), Convert.ToInt16(bitmapWidth)))
                {
                    using (var graphics2 = Graphics.FromImage(bitmap2))
                    {
                        foreach (var triangle in triangles)
                        {
                            graphics2.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                            graphics2.DrawPolygon(new Pen(Color.Black, 1.0f), triangle.Points);
                        }
                    }
                    path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TriangulatedPolygon.png");
                    bitmap2.Save(path);
                }
            }
            
            #endregion
        }
    }
}