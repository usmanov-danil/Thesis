//Copyright (c) 2018 Bruce Greene

//Permission is hereby granted, free of charge, to any person obtaining a copy 
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights to 
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies 
//of the Software, and to permit persons to whom the Software is furnished to do 
//so, subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all 
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR 
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS 
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR 
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER 
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION 
//WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


using HelixToolkit.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Aggregator
{
    public class SurfacePlotVisual3D : HelixViewport3D
    {
        private TruncatedConeVisual3D? marker;
        private BillboardTextVisual3D? coords;
        private double labelOffset, minDistanceSquared;
        private string coordinateFormat;
        private Point3D[,]? points;
        private ModelVisual3D modelContainer = new ModelVisual3D();
        private double[,] ColorValues;
        private Brush SurfaceBrush;

        public SurfacePlotVisual3D()
            : base()
        {
            ZoomExtentsWhenLoaded = true;
            ShowCoordinateSystem = true;
            ShowViewCube = true;
            ShowFrameRate = false;
            ShowTriangleCountInfo = false;

            // Default configuration:
            AxisLabels = "X,Y,Z";
            BoundingBox = new Rect3D(0, 0, 0, 100, 100, 50);
            TickSize = 10;
            MinDistance = 0.1;
            DecimalPlaces = 1;
            Background = Brushes.White;
            AxisBrush = Brushes.Gray;
            MarkerBrush = Brushes.Red;
            Elements = EElements.All;
            //CreateElements();
        }


        // Public fields
        public Point3D[,] Points
        {
            get { return points; }
            set { points = value; }
        }
        public string AxisLabels { get; set; }
        public Rect3D BoundingBox { get; set; }
        public double TickSize { get; set; }
        public double MinDistance { get; set; }
        public int DecimalPlaces { get; set; }
        public SolidColorBrush AxisBrush { get; set; }
        public SolidColorBrush MarkerBrush { get; set; }
        public EElements Elements { get; set; }
        public Color TraceColor { get { return Colors.Black; } }  // (points != null) ? path.Color : Colors.Black;
        public double TraceThickness { get { return 1.0; } } // (points != null) ? path.Thickness : 1;

        [Flags]
        public enum EElements
        {
            /// <summary>Traces only.</summary>
            None = 0x00,
            /// <summary>XYZ axes.</summary>
            Axes = 0x01,
            /// <summary>XY grid.</summary>
            Grid = 0x02,
            /// <summary>XYZ bounding box.</summary>
            BoundingBox = 0x04,
            /// <summary>Marker cone and coordinates.</summary>
            Marker = 0x08,
            /// <summary>Axes, grid, bounding box and marker.</summary>
            All = 0x0F
        };



        // Public methods
        public void CreateElements()
        {
            Children.Clear();
            Children.Add(new DefaultLights());

            string[] labels = AxisLabels.Split(',');
            if (labels.Length < 3)
                labels = new string[] { "X", "Y", "Z" };

            double bbSize = Math.Max(Math.Max(BoundingBox.SizeX, BoundingBox.SizeY), BoundingBox.SizeZ);
            double lineThickness = bbSize / 1000;
            double arrowOffset = lineThickness * 30;
            labelOffset = lineThickness * 50;
            minDistanceSquared = MinDistance * MinDistance;

            if (Elements.HasFlag(EElements.Grid))
            {
                var grid = new GridLinesVisual3D();
                grid.Center = new Point3D(BoundingBox.X + 0.5 * BoundingBox.SizeX, BoundingBox.Y + 0.5 * BoundingBox.SizeY, BoundingBox.Z);
                grid.Length = BoundingBox.SizeX;
                grid.Width = BoundingBox.SizeY;
                grid.MinorDistance = TickSize;
                grid.MajorDistance = bbSize;
                grid.Thickness = lineThickness;
                grid.Fill = AxisBrush;
               // Children.Add(grid);
            }

            if (Elements.HasFlag(EElements.Axes))
            {
                var arrow = new ArrowVisual3D();
                arrow.Point2 = new Point3D((BoundingBox.X + BoundingBox.SizeX) + arrowOffset, 0.0, 0.0);
                arrow.Diameter = lineThickness * 5;
                arrow.Fill = AxisBrush;
                Children.Add(arrow);

                var label = new BillboardTextVisual3D();
                label.Text = labels[0];
                label.FontWeight = FontWeights.Bold;
                label.Foreground = AxisBrush;
                label.Position = new Point3D((BoundingBox.X + BoundingBox.SizeX) + labelOffset, 0.0, 0.0);
                Children.Add(label);

                arrow = new ArrowVisual3D();
                arrow.Point2 = new Point3D(0.0, (BoundingBox.Y + BoundingBox.SizeY) + arrowOffset, 0.0);
                arrow.Diameter = lineThickness * 5;
                arrow.Fill = AxisBrush;
                Children.Add(arrow);

                label = new BillboardTextVisual3D();
                label.Text = labels[1];
                label.FontWeight = FontWeights.Bold;
                label.Foreground = AxisBrush;
                label.Position = new Point3D(0.0, (BoundingBox.Y + BoundingBox.SizeY) + labelOffset, 0.0);
                Children.Add(label);

                if (BoundingBox.SizeZ > 0)
                {
                    arrow = new ArrowVisual3D();
                    arrow.Point2 = new Point3D(0.0, 0.0, (BoundingBox.Z + BoundingBox.SizeZ) + arrowOffset);
                    arrow.Diameter = lineThickness * 5;
                    arrow.Fill = AxisBrush;
                    Children.Add(arrow);

                    label = new BillboardTextVisual3D();
                    label.Text = labels[2];
                    label.FontWeight = FontWeights.Bold;
                    label.Foreground = AxisBrush;
                    label.Position = new Point3D(0.0, 0.0, (BoundingBox.Z + BoundingBox.SizeZ) + labelOffset);
                    Children.Add(label);
                }
            }

            if (Elements.HasFlag(EElements.BoundingBox) && BoundingBox.SizeZ > 0)
            {
                var box = new BoundingBoxWireFrameVisual3D();
                box.BoundingBox = BoundingBox;
                box.Thickness = 1;
                box.Color = AxisBrush.Color;
               // Children.Add(box);
            }

            if (Elements.HasFlag(EElements.Marker))
            {
                marker = new TruncatedConeVisual3D();
                marker.Height = labelOffset;
                marker.BaseRadius = 0.0;
                marker.TopRadius = labelOffset / 5;
                marker.TopCap = true;
                marker.Origin = new Point3D(0.0, 0.0, 0.0);
                marker.Normal = new Vector3D(-1.0, -1.0, 1.0);
                marker.Fill = MarkerBrush;
               // Children.Add(marker);

                coords = new BillboardTextVisual3D();
                coordinateFormat = string.Format("{{0:F{0}}}, {{1:F{0}}}, {{2:F{0}}}", DecimalPlaces, DecimalPlaces, DecimalPlaces);  // "{0:F2}, {1:F2}, {2:F2}"
                coords.Text = string.Format(coordinateFormat, 0.0, 0.0, 0.0);
                coords.Foreground = MarkerBrush;
                coords.Position = new Point3D(-labelOffset, -labelOffset, labelOffset);
               // Children.Add(coords);
            }
            else
            {
                marker = null;
                coords = null;
            }

            if (points != null)
            {
                CreateSurface();
                Children.Add(modelContainer);
            }
        }

        private void CreateSurface()
        {
            // Get relevant constaints from the DataPoints object
            int numberOfRows = points.GetUpperBound(0) + 1;
            int numberOfColumns = points.GetUpperBound(1) + 1;

            double minZ = double.MaxValue;
            double maxZ = double.MinValue;

            double minColorValue = double.MaxValue;
            double maxColorValue = double.MinValue;

            
       
            if (Math.Abs(minColorValue) < Math.Abs(maxColorValue)) { minColorValue = -maxColorValue; }
            else { maxColorValue = -minColorValue; }

            // Set the texture coordinates by either z-value or ColorValue
            var textureCoordinates = new Point[numberOfRows, numberOfColumns];
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    double tc;
                    if (ColorValues != null) { tc = (ColorValues[i, j] - minColorValue) / (maxColorValue - minColorValue); }
                    else { tc = (points[i, j].Z - minZ) / (maxZ - minZ); }
                    textureCoordinates[i, j] = new Point(tc, tc);
                }
            }

            // Build the surface model (i.e. the coloured surface model)
            MeshBuilder surfaceModelBuilder = new MeshBuilder();
            surfaceModelBuilder.AddRectangularMesh(points, textureCoordinates);

            GeometryModel3D surfaceModel = new GeometryModel3D(surfaceModelBuilder.ToMesh(), MaterialHelper.CreateMaterial(BrushHelper.CreateRainbowBrush()));
            surfaceModel.BackMaterial = surfaceModel.Material;
            modelContainer.Content = surfaceModel;


        }

        public void Clear()
        {
            points = null;
            CreateElements();
        }





        public void AddPoints(Point3D[,] points, Color color, double thickness = -1)
        {
            Clear();
            this.points = points;
            UpdateBoundingBox(points);
            
        }

        private void UpdateBoundingBox(Point3D[,] points)
        {
            int numberOfRows = points.GetUpperBound(0) + 1;
            int numberOfColumns = points.GetUpperBound(1) + 1;

            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
            for (int i = 0; i < numberOfRows; i++)
            {
               for (int j = 0; j < numberOfColumns; j++) 
               { 
                minX = Math.Min(minX, points[i, j].X);
                minY = Math.Min(minY, points[i, j].Y);
                minZ = Math.Min(minZ, points[i, j].Z);
                maxX = Math.Max(maxX, points[i, j].X);
                maxY = Math.Max(maxY, points[i, j].Y);
                maxZ = Math.Max(maxZ, points[i, j].Z);
                }
            }
            BoundingBox = new Rect3D(minX, minY, minZ, maxX, maxY, maxZ);
        }
    }
}
