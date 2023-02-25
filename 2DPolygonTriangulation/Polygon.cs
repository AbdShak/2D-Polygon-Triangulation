using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2DPolygonTriangulation
{
    public class Polygon
    {
        public PointF[] Points;
        public Polygon()
        {}
        public Polygon(PointF[] points)
        { Points = points;}
        public List<Triangle> Triangulate()
        {
            PointF[] tPoints = new PointF[Points.Length];
            tPoints = Points;
            Polygon polygon = new Polygon(tPoints);
            // Make sure the polygon clockwise oriented so we can later find concave vertex & convex vertex otherwise reverse the points
            polygon.OrientClockwise();
            // List contains our triangles 
            List<Triangle> triangles = new List<Triangle>();
            // If the polygon is not a triangle Remove an ear from the polygon.
            while (polygon.Points.Count() > 3)
            {
                polygon.FindAndRemoveEar(triangles);
            }
            // If the polygon is triangle add it to our list
            triangles.Add(new Triangle(polygon.Points[0], polygon.Points[1], polygon.Points[2]));
            return triangles;
        }

        private void OrientClockwise()
        {
            // Add first point to the end of the list. so we can make calutlation for the last point relativity to the first one.
            PointF[] pts = new PointF[Points.Length + 1];
            Points.CopyTo(pts, 0);
            pts[Points.Length] = Points[0];
            // Calculate the area of our Polygon and find if it's (< 0 ) that's mean our points move counterclockwise and we have to reverse our points (more info on how we caluclate the area https://www.mathsisfun.com/geometry/area-irregular-polygons.html )
            float areaOfPolygon = 0;
            for (int i = 0; i < Points.Length; i++)
            {
                areaOfPolygon += (pts[i + 1].X - pts[i].X) * (pts[i + 1].Y + pts[i].Y) / 2;
            }

            if (areaOfPolygon > 0)
                // Orient the polygon clockwise
                Points.Reverse();
        }

        private void FindAndRemoveEar(List<Triangle> triangles)
        {
            // Check for an ear. Triangle vertices p0,p1,p2
            int[] triangle = new int[3] { 0, 0, 0 };
            triangle = FindEar(triangle[0], triangle[1], triangle[2]);
            // Add an new ear to the list
            if (triangle != null)
            {
                triangles.Add(new Triangle(Points[triangle[0]], Points[triangle[1]], Points[triangle[2]]));
                // Remove the ear(point) from the polygon.
                List<PointF> p = Points.ToList();
                p.RemoveAt(triangle[1]);
                Points = p.ToArray();
            }
        }

        private int[] FindEar( int p0,  int p1,  int p2)
        {
            for (p0 = 0; p0 < Points.Length; p0++)
            {
                p1 = (p0 + 1) % Points.Length;//if p0 was the last point or last - 1 point we wil take the last or first point
                p2 = (p1 + 1) % Points.Length;//if p0 was the last point or last - 1 point we wil take the first or second point
                // Send three points and check if it's ear or not if it's ear return our three points to save it 
                if (CheckEar(Points, p0, p1, p2))
                    return new int[3] { p0, p1, p2 };
            }
            return null;
        }

        private static bool CheckEar(PointF[] points, int p0, int p1, int p2)
        {
            // Check if p1 is concave vertex (i.e) Angle is bigger than 180 or (Pi 3.14)
            float angle = GetAngle(points[p0].X, points[p0].Y, points[p1].X, points[p1].Y, points[p2].X, points[p2].Y);
            if (angle > 180 || angle < -180)
            {
                // not ear
                return false;
            }
            Triangle triangle = new Triangle(points[p0], points[p1], points[p2]);
            // Make sure there is no point inside our ear
            for (int i = 0; i < points.Length; i++)
            {
                if ((i != p0) && (i != p1) && (i != p2))
                {
                    if (triangle.PointInsidePolygon(points[i].X, points[i].Y))
                    {
                        // not ear
                        return false;
                    }
                }
            }
            //  an ear.
            return true;
        }

        public static float GetAngle(float p0x, float p0y, float p1x, float p1y, float p2x, float p2y)
        {
            
            float p1p0x = p0x - p1x;
            float p1p0y = p0y - p1y;
            float p1p2x = p2x - p1x;
            float p1p2y = p2y - p1y;

            // another way to find dot product
            //float[] p1p0 = new float[] { p1p0x, p1p0y };
            //float[] p2p1 = new float[] { p1p2x, p1p2y };
            //float tem3333p = p1p0.Zip(p2p1, (x, y) => x * y).Sum(); ;

            // tan (theta) = cross product/dot product 
            float temp = (p1p0x * p1p2y - p1p0y * p1p2x) / (p1p0x * p1p2x + p1p0y * p1p2y);
            // Get theta
            double radians = Math.Atan(temp);
            // Convert from radians to degrees.
            double angle = radians * (180 / Math.PI);

            return (float)angle;
        }

        public bool PointInsidePolygon(float X, float Y)
        {
            bool result = false;
            int j = Points.Count() - 1;
            for (int i = 0; i < Points.Count(); i++)
            {
                if (Points[i].Y < X && Points[j].Y >= Y || Points[j].Y < Y && Points[i].Y >= Y)
                {
                    if (Points[i].X + (Y - Points[i].Y) / (Points[j].Y - Points[i].Y) * (Points[j].X - Points[i].X) < X)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }


    }
}
