using System;
using System.Numerics; // The core math library!
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfSketcher
{
    public partial class MainWindow : Window
    {
        private Line _line1;
        private Line _line2;

        private bool _isDrawing = false;
        private int _linesDrawn = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        // ==========================================
        // 1. DRAWING THE LINES (Click and Drag UX)
        // ==========================================
        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_linesDrawn >= 2) return; // Stop if we already have 2 lines

            _isDrawing = true;
            Point startPoint = e.GetPosition(DrawingCanvas);

            // Create a new line starting exactly where the mouse clicked
            Line newLine = new Line
            {
                X1 = startPoint.X,
                Y1 = startPoint.Y,
                X2 = startPoint.X,
                Y2 = startPoint.Y,
                Stroke = _linesDrawn == 0 ? Brushes.Blue : Brushes.Orange,
                StrokeThickness = 4,
                StrokeEndLineCap = PenLineCap.Round,
                StrokeStartLineCap = PenLineCap.Round
            };

            if (_linesDrawn == 0) _line1 = newLine;
            else _line2 = newLine;

            DrawingCanvas.Children.Add(newLine);

            // "Capture" the mouse so dragging outside the canvas doesn't break the app
            DrawingCanvas.CaptureMouse();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDrawing) return;

            // Update the End Point of the line as the user drags their mouse
            Point currentPoint = e.GetPosition(DrawingCanvas);

            if (_linesDrawn == 0)
            {
                _line1.X2 = currentPoint.X;
                _line1.Y2 = currentPoint.Y;
            }
            else if (_linesDrawn == 1)
            {
                _line2.X2 = currentPoint.X;
                _line2.Y2 = currentPoint.Y;
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!_isDrawing) return;

            _isDrawing = false;
            DrawingCanvas.ReleaseMouseCapture();
            _linesDrawn++;

            // Update the UI Instructions
            if (_linesDrawn == 1)
                StatusText.Text = "Step 2: Click and drag to draw the Target Line.";
            else if (_linesDrawn == 2)
                StatusText.Text = "Step 3: Click 'Make Perpendicular' to apply the math constraint!";
        }

        // ==========================================
        // 2. THE CHAPTER 7 MATH (Computational Geometry)
        // ==========================================
        private void MakePerpendicular_Click(object sender, RoutedEventArgs e)
        {
            if (_linesDrawn < 2)
            {
                MessageBox.Show("Please draw both lines first!");
                return;
            }

            // Grab the exact coordinates directly from the UI lines
            Vector2 p1 = new Vector2((float)_line1.X1, (float)_line1.Y1);
            Vector2 p2 = new Vector2((float)_line1.X2, (float)_line1.Y2);
            Vector2 p3 = new Vector2((float)_line2.X1, (float)_line2.Y1);
            Vector2 p4 = new Vector2((float)_line2.X2, (float)_line2.Y2);

            Vector2 v1 = p2 - p1;
            Vector2 v2 = p4 - p3;

            // Step A: Get the pure direction of Line 1
            Vector2 dir1 = Vector2.Normalize(v1);

            // Step B: Calculate the Perpendicular directions (+90 and -90 degrees)
            Vector2 perpDirPos90 = new Vector2(-dir1.Y, dir1.X);
            Vector2 perpDirNeg90 = new Vector2(dir1.Y, -dir1.X);

            // Step C: The Dot Product determines which way is "closer" to the user's drawing
            Vector2 finalPerpDir;
            if (Vector2.Dot(v2, perpDirPos90) > Vector2.Dot(v2, perpDirNeg90))
            {
                finalPerpDir = perpDirPos90;
            }
            else
            {
                finalPerpDir = perpDirNeg90;
            }

            // Step D: Calculate the new End Point for Line 2
            float length2 = v2.Length();
            Vector2 newP4 = p3 + (finalPerpDir * length2);

            // Update the WPF UI with our new calculated math!
            _line2.X2 = newP4.X;
            _line2.Y2 = newP4.Y;
            _line2.Stroke = Brushes.Green;

            StatusText.Text = "Constraint Applied! The green line is mathematically perpendicular.";
        }

        // ==========================================
        // 3. RESET
        // ==========================================
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            DrawingCanvas.Children.Clear();
            _line1 = null;
            _line2 = null;
            _linesDrawn = 0;
            _isDrawing = false;
            StatusText.Text = "Step 1: Click and drag to draw the Reference Line.";
        }
    }
}