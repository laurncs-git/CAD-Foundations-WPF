using System;
using System.Windows;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Microsoft.Win32;

namespace WpfStlViewer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFile_Click(object sender, RoutedEventArgs e)
        {
            // 1. Open a File Dialog to pick the STL
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "STL Files (*.stl)|*.stl|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    // 2. Helix Toolkit does the heavy lifting of parsing the file
                    StLReader reader = new StLReader();
                    Model3DGroup modelGroup = reader.Read(openFileDialog.FileName);

                    // 3. Put the model into our 3D Viewport
                    ModelContainer.Content = modelGroup;

                    // 4. Zoom the camera out so the whole model fits on the screen
                    Viewport3D.ZoomExtents();

                    // 5. EXTRACT THE MATH!
                    // A Model3DGroup can hold multiple parts. We just grab the first one.
                    if (modelGroup.Children.Count > 0 && modelGroup.Children[0] is GeometryModel3D geometryModel)
                    {
                        if (geometryModel.Geometry is MeshGeometry3D mesh)
                        {
                            UpdateMathUI(mesh);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading STL: " + ex.Message);
                }
            }
        }

        private void UpdateMathUI(MeshGeometry3D mesh)
        {
            // The textbook teaches that a mesh is just a list of Points (Vertices) 
            // and a list of instructions on how to connect them (Triangle Indices).

            // 1. Count the raw data
            int vertexCount = mesh.Positions.Count;
            // Every 3 indices make 1 triangle!
            int triangleCount = mesh.TriangleIndices.Count / 3;

            TxtVertices.Text = $"Vertices: {vertexCount:N0}";
            TxtTriangles.Text = $"Triangles: {triangleCount:N0}";

            // 2. Calculate the Bounding Box (The 3D physical size of the object)
            Rect3D bounds = mesh.Bounds;

            TxtSizeX.Text = $"Size X: {bounds.SizeX:F2} mm";
            TxtSizeY.Text = $"Size Y: {bounds.SizeY:F2} mm";
            TxtSizeZ.Text = $"Size Z: {bounds.SizeZ:F2} mm";

            // 3. Calculate the Exact Center of the object
            // This requires basic 3D Vector Math! 
            // (Min coordinate + Half of the Size = Center)
            double centerX = bounds.X + (bounds.SizeX / 2.0);
            double centerY = bounds.Y + (bounds.SizeY / 2.0);
            double centerZ = bounds.Z + (bounds.SizeZ / 2.0);

            TxtCenterX.Text = $"Center X: {centerX:F2}";
            TxtCenterY.Text = $"Center Y: {centerY:F2}";
            TxtCenterZ.Text = $"Center Z: {centerZ:F2}";
        }
    }
}