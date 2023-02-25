Part I: Explanation

First of all, I included comments explain almost every step inside the code for the Triangulation process. You can read the comments inside the code instead of reading the points below and go to the test cases & resources in **Part II** & **Part III**. (note: If there is anything not explained well in **Part I** you can find more mathematically explanation as a resource in **Part III**)

1) The code starting by reading the input from “**polygonLines.txt”**.

1) Convert input from **string** to **float** so we can perform our calculation on the input.
1) From the input (Line Length & Line Angle) we create the Polygon vertices and save it on (**List<PointF> points**) using simple math calculation, the starting point will be the center of bitmap image that we will create it later and then close the Polygon by adding the first vertex as last item on **points** list if the Polygon is not already closed.
1) Start drawing the original Polygon using the function **DrawLines ()** by giving it the **points** list after convert it to array of **PointF** and save it to desktop with name “**OriginalPolygon.png**”. After Triangulation process ends we save the triangles to desktop (note: this is optional step so you can comment the code if you don’t want it), and save the Triangulated Polygon to desktop with name “**TriangulatedPolygon.png**”.
1) Start the **Triangulation** process by creating an instance of **Polygon** and give it the list of Polygon vertices then start the Triangulation by calling **Triangulate** **()** function.
1) After calling **Triangulate ()** function we need to start **Ear Clipping algorithm** to triangulate our polygon. First of all, we need to make sure the polygon clockwise oriented by calling **OrientClockwise ()** function that Calculate the area of our Polygon and find if it's (< 0) that's mean our points move counterclockwise and we have to reverse our points.
1) Now we need to make sure our Polygon is not triangle so in any time we have triangle after the ear clipping process we just add to our list of **List<Triangle> triangles** and end the algorithm. If we don’t have triangle (i.e. more than three vertices) we will call **FindAndRemoveEar ()**.
1) That function will send the first three vertices to function **FindEar ()** and if the function founds an ear will return a non-null value and we will add the ear to our list **List<Triangle> triangles** and remove the convex vertex from the list of **points**.
1) **FindEar ()** will make a for loop looking for an ear sending every iteration three vertices to **CheckEar ()** function and that function will make sure this potential ear:
   a) Does not have concave vertex by calling **GetAngle ()** function.
   b) There is no point inside our potential ear by calling **PointInsidePolygon ()** function.
1) **GetAngle ()** calculate angle based on the rule (***tan (theta) = cross product/dot product***), cross product and dot product for vectors (**p1p0, p1p2**) and **PointInsidePolygon ()** will check if there is point lies inside the polygon.


















Part II: Test Cases

1) Rectangle Move Clockwise (from Candidate Challenge document):

   Input: **4 70 0 40 -90 70 -90 40 -90**

   output:
   ![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.001.png "TriangulatedPolygon")
1) Rectangle Move Counterclockwise (from Candidate Challenge document):

   Input: **4 40 90 70 90 40 90 70 90**

   output:
   ![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.002.png)
1) Equilateral Triangle:

   Input: **3 60 0 60 120 60 120**

   output:
   ![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.003.png)





1) Pentagon:

   Input: **5 60 108 60 -72 60 -72 60 -72 60 -72**

   output:
   ![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.004.png)
1) Trapezoid:

   Input: **4 160 0 50 126.869897646 100 53.1301023542 50 53.1301023542**

   output:
   ![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.005.png)
1) Non-simple shape:


Input: (note:  Just for testing purpose I entered this directly to our list of points not as .txt file!)

1) In Progream.cs we need to create new array of PointF hold our data:
   PointF [] tpoints = new PointF [] {new PointF (66.67f, 200.00f), new PointF (97.33f, 320.47f), new PointF (280.67f, 345.47f), new PointF (362.3f, 148.00f), new PointF (187.67f, 203.53f), new PointF (70.33f, 30.53f), new PointF (66.67f, 200.00f) };
1) In line 83 let the DrawLines take input from the array above:
   graphics.DrawLines (new Pen (Color.Black, 1.0f), points.ToArray());
   --> graphics.DrawLines(new Pen (Color.Black, 1.0f), tpoints);
1) In Lines (77,100,112) change the Bitmap width and height to something big enough:
   new Bitmap(Convert.ToInt16(bitmapWidth), Convert.ToInt16(bitmapWidth))
   à new Bitmap (1000, 1000)
1) In Polygon.cs function Triangulate let:
   ` `Points = new PointF [] {new PointF (66.67f, 200.00f), new PointF (97.33f, 320.47f), new PointF (280.67f, 345.47f), new PointF (362.3f, 148.00f), new PointF (187.67f, 203.53f), new PointF (70.33f, 30.53f)~~, new PointF (66.67f, 200.00f)~~ };(note: **Without the last point as we said above in Part I we need only vertices)** 





   output:
   ![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.006.png)
1) Non-simple shape (looks like [Figure 2] from Candidate Challenge document):


Input: (note:  Just for testing purpose I entered this directly to our list of points not as .txt file!)

1) In Progream.cs we need to create new array of PointF hold our data:
   PointF[] tpoints = new PointF[] { new PointF(83.67f, 97.00f), new PointF(24.50f, 108.24f),new PointF(89.33f, 250.47f),new PointF(16.18f, 322.07f),new PointF(182.03f, 305.67f),new PointF(243.40f, 233.88f),new PointF(300.13f, 271.29f),new PointF(367.58f, 241.10f),new PointF(370.50f, 68.74f),new PointF(316.33f, 119.00f),new PointF(268.67f, 70.53f),new PointF(162.50f, 97.03f),new PointF(104.33f, 145.53f),new PointF(83.67f, 97.00f)};
1) In line 83 let the DrawLines take input from the array above:
   graphics.DrawLines (new Pen (Color.Black, 1.0f), points.ToArray());
   --> graphics.DrawLines(new Pen(Color.Black, 1.0f), tpoints);
1) In Lines 77,100,112 change the Bitmap width and height to something big enough:
   new Bitmap(Convert.ToInt16(bitmapWidth), Convert.ToInt16(bitmapWidth))
   à new Bitmap (1000, 1000)
1) In Polygon.cs function Triangulate let:

` `Points = new PointF[] { new PointF(83.67f, 97.00f), new PointF(24.50f, 108.24f),new PointF(89.33f, 250.47f),new PointF(16.18f, 322.07f),new PointF(182.03f, 305.67f),new PointF(243.40f, 233.88f),new PointF(300.13f, 271.29f),new PointF(367.58f, 241.10f),new PointF(370.50f, 68.74f),new PointF(316.33f, 119.00f),new PointF(268.67f, 70.53f),new PointF(162.50f, 97.03f),new PointF(104.33f, 145.53f)~~,new~~ ~~PointF(83.67f, 97.00f)~~}; (note: **Without the last point as we said above in Part I we need only vertices)**











output:
![](Aspose.Words.e166b7a8-3e5f-4d05-b24c-d21f2819f3ae.007.png)

Part III: Resources

\1) Calculate [area of Irregular Polygons](https://www.mathsisfun.com/geometry/area-irregular-polygons.html) using vertices (**Part I** point#6)

\2) [Ear Cutting for Simple Polygons](http://www-cgrl.cs.mcgill.ca/%7Egodfried/teaching/cg-projects/97/Ian/cutting_ears.html) by Ian Garton

\3) [Triangulation by Ear Clipping](https://www.geometrictools.com/Documentation/TriangulationByEarClipping.pdf) by David Eberly

\4) [Online tool for Concave Polygon](http://www.mathopenref.com/polygonconcave.html)

\5) [Finding angle using cross product and dot product](http://mathhelpforum.com/calculus/100162-finding-angle-using-cross-product-dot-product.html) (**Part I** point#10)

\6) [C# Helper](http://csharphelper.com/blog/) by Rod Stephens

\7) How to find a [point inside a polygon](http://stackoverflow.com/questions/4243042/c-sharp-point-in-polygon) (**Part I** point#10)

\8) Amazing tool to [draw polygon online](https://betravis.github.io/shape-tools/polygon-drawing/) (**Part II** Case#6 & Case#7)
