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
    public class ScatterPlotVisual3D : HelixViewport3D
    {
        private TruncatedConeVisual3D? marker;
        private BillboardTextVisual3D? coords;
        private double labelOffset, minDistanceSquared;
        private string coordinateFormat;
        private List<LinesVisual3D> trace;
        private LinesVisual3D path;
        private Point3D point0;
        private Vector3D delta0;


        public ScatterPlotVisual3D()
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
        public string AxisLabels { get; set; }
        public Rect3D BoundingBox { get; set; }
        public double TickSize { get; set; }
        public double MinDistance { get; set; }
        public int DecimalPlaces { get; set; }
        public SolidColorBrush AxisBrush { get; set; }
        public SolidColorBrush MarkerBrush { get; set; }
        public EElements Elements { get; set; }
        public Color TraceColor { get { return (path != null) ? path.Color : Colors.Black; } }
        public double TraceThickness { get { return (path != null) ? path.Thickness : 1; } }

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
                //Children.Add(grid);

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
                //Children.Add(box);
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
              //  Children.Add(marker);

                coords = new BillboardTextVisual3D();
                coordinateFormat = string.Format("{{0:F{0}}}, {{1:F{0}}}, {{2:F{0}}}", DecimalPlaces, DecimalPlaces, DecimalPlaces);  // "{0:F2}, {1:F2}, {2:F2}"
                coords.Text = string.Format(coordinateFormat, 0.0, 0.0, 0.0);
                coords.Foreground = MarkerBrush;
                coords.Position = new Point3D(-labelOffset, -labelOffset, labelOffset);
                //Children.Add(coords);
            }
            else
            {
                marker = null;
                coords = null;
            }

            if (trace != null)
            {
                foreach (LinesVisual3D p in trace)
                    Children.Add(p);
                path = trace[trace.Count - 1];
            }
        }


        public void Clear()
        {
            trace = null;
            path = null;
            CreateElements();
        }


        public void NewTrace(Point3D point, Color color, double thickness = 1)
        {
            path = new LinesVisual3D();
            path.Color = color;
            path.Thickness = thickness;
            trace = new List<LinesVisual3D>();
            trace.Add(path);
            Children.Add(path);
            point0 = point;
            delta0 = new Vector3D();

            if (marker != null)
            {
                marker.Origin = point;
                coords.Position = new Point3D(point.X - labelOffset, point.Y - labelOffset, point.Z + labelOffset);
                coords.Text = string.Format(coordinateFormat, point.X, point.Y, point.Z);
            }
        }

        public void AddPoint(Point3D point, Color color, double thickness = -1)
        {
            if (trace == null)
            {
                NewTrace(point, color, (thickness > 0) ? thickness : 1);
                return;
            }

            if ((point - point0).LengthSquared < minDistanceSquared) return;  // less than min distance from last point

            if (path.Color != color || (thickness > 0 && path.Thickness != thickness))
            {
                if (thickness <= 0)
                    thickness = path.Thickness;

                path = new LinesVisual3D();
                path.Color = color;
                path.Thickness = thickness;
                trace.Add(path);
                Children.Add(path);
            }

            // If line segments AB and BC have the same direction (small cross product) then remove point B.
            bool sameDir = false;
            var delta = new Vector3D(point.X - point0.X, point.Y - point0.Y, point.Z - point0.Z);
            delta.Normalize();  // use unit vectors (magnitude 1) for the cross product calculations
            if (path.Points.Count > 0)
            {
                double xp2 = Vector3D.CrossProduct(delta, delta0).LengthSquared;
                sameDir = (xp2 < 0.0005);  // approx 0.001 seems to be a reasonable threshold from logging xp2 values
                //if (!sameDir) Title = string.Format("xp2={0:F6}", xp2);
            }

            if (sameDir)  // extend the current line segment
            {
                path.Points[path.Points.Count - 1] = point;
                point0 = point;
                delta0 += delta;
            }
            else  // add a new line segment
            {
                path.Points.Add(point0);
                path.Points.Add(point);
                point0 = point;
                delta0 = delta;
            }

            if (marker != null)
            {
                marker.Origin = point;
                coords.Position = new Point3D(point.X - labelOffset, point.Y - labelOffset, point.Z + labelOffset);
                coords.Text = string.Format(coordinateFormat, point.X, point.Y, point.Z);
            }
        }

        public void AddPoints(Point3D[] points, Color color, double thickness = -1)
        {
            Clear();
            for (int i = 0; i < points.Length; i++)
                AddPoint(points[i], color, thickness);
            UpdateBoundingBox(points);


        }

        private void UpdateBoundingBox(Point3D[] points) {
            double minX = double.MaxValue;
            double maxX = double.MinValue;
            double minY = double.MaxValue;
            double maxY = double.MinValue;
            double minZ = double.MaxValue;
            double maxZ = double.MinValue;
            for (int i=0; i < points.Length; i++)
            {
                minX = Math.Min(minX, points[i].X);
                minY = Math.Min(minY, points[i].Y);
                minZ = Math.Min(minZ, points[i].Z);
                maxX = Math.Max(maxX, points[i].X);
                maxY = Math.Max(maxY, points[i].Y);
                maxZ = Math.Max(maxZ, points[i].Z);
            }
            BoundingBox = new Rect3D(minX, minY, minZ, maxX, maxY, maxZ);
        }
    }
}
