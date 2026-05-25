using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Microsoft.Win32;

namespace WpfSlicer
{
    public partial class MainWindow : Window
    {
        private MeshGeometry3D _loadedMesh;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "STL Files (*.stl)|*.stl";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // 1. Use Helix to load the mesh data
                    StLReader reader = new StLReader();
                    Model3DGroup modelGroup = reader.Read(openFileDialog.FileName);

                    if (modelGroup.Children.Count > 0 && modelGroup.Children[0] is GeometryModel3D geometryModel)
                    {
                        if (geometryModel.Geometry is MeshGeometry3D mesh)
                        {
                            _loadedMesh = mesh;

                            // Adjust slider max to the actual height of the loaded object
                            ZSlider.Maximum = _loadedMesh.Bounds.Z + _loadedMesh.Bounds.SizeZ;
                            ZSlider.Minimum = _loadedMesh.Bounds.Z;
                            ZSlider.Value = ZSlider.Minimum + (_loadedMesh.Bounds.SizeZ / 2.0); // Start in the middle

                            SliceMesh(ZSlider.Value);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading STL: " + ex.Message);
                }
            }
        }

        private void ZSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (TxtZHeight != null)
                TxtZHeight.Text = $"Current Z: {e.NewValue:F2} mm";

            SliceMesh(e.NewValue);
        }

        // ==========================================
        // THE CAD MATH: SLICING THE 3D MESH
        // ==========================================
        private void SliceMesh(double sliceZ)
        {
            if (_loadedMesh == null) return;

            SliceCanvas.Children.Clear();
            int linesGenerated = 0;

            // Triangles are stored as a single flat list of coordinates (Positions)
            // and a list of instructions on how to connect them (TriangleIndices).
            var positions = _loadedMesh.Positions;
            var indices = _loadedMesh.TriangleIndices;

            // Loop through every single triangle in the mesh (jump by 3)
            for (int i = 0; i < indices.Count; i += 3)
            {
                // Get the 3D corners of the current triangle
                Point3D p0 = positions[indices[i]];
                Point3D p1 = positions[indices[i + 1]];
                Point3D p2 = positions[indices[i + 2]];

                // Get exactly where the plane cuts the triangle (if it does at all)
                List<Point> intersectionPoints = GetIntersections(p0, p1, p2, sliceZ);

                // If the plane cut exactly 2 edges, draw a line between them!
                if (intersectionPoints.Count == 2)
                {
                    Line cutLine = new Line
                    {
                        X1 = intersectionPoints[0].X,
                        Y1 = intersectionPoints[0].Y,
                        X2 = intersectionPoints[1].X,
                        Y2 = intersectionPoints[1].Y,
                        Stroke = Brushes.Cyan,
                        StrokeThickness = 0.5
                    };

                    SliceCanvas.Children.Add(cutLine);
                    linesGenerated++;
                }
            }

            TxtStats.Text = $"Lines Generated: {linesGenerated}";
        }

        // ==========================================
        // HELPER MATH: FINDING THE CUT POINTS
        // ==========================================
        private List<Point> GetIntersections(Point3D p0, Point3D p1, Point3D p2, double sliceZ)
        {
            List<Point> points = new List<Point>();

            // Check all 3 edges of the triangle
            CheckEdge(p0, p1, sliceZ, points);
            CheckEdge(p1, p2, sliceZ, points);
            CheckEdge(p2, p0, sliceZ, points);

            return points;
        }

        private void CheckEdge(Point3D a, Point3D b, double z, List<Point> points)
        {
            // Does this edge cross our Z-plane? (One point above, one point below)
            if ((a.Z > z && b.Z <= z) || (a.Z <= z && b.Z > z))
            {
                // Linear Interpolation (Lerp) to find the exact X,Y coordinate of the cut
                double t = (z - a.Z) / (b.Z - a.Z);
                double x = a.X + t * (b.X - a.X);
                double y = a.Y + t * (b.Y - a.Y);

                points.Add(new Point(x, y));
            }
        }
    }
}