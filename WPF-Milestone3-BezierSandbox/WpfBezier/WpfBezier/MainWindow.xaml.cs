using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfBezier
{
    public partial class MainWindow : Window
    {
        private UIElement _draggedPoint = null;

        public MainWindow()
        {
            InitializeComponent();

            // Set initial starting positions for our 4 control points
            SetPointPosition(Point0, 100, 400); // Start point
            SetPointPosition(Point1, 200, 100); // Handle 1
            SetPointPosition(Point2, 600, 100); // Handle 2
            SetPointPosition(Point3, 700, 400); // End point

            UpdateCurve();
        }

        // ==========================================
        // 1. DRAG & DROP LOGIC
        // ==========================================
        private void Point_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // When user clicks a red dot, remember which one they clicked
            _draggedPoint = sender as UIElement;
            _draggedPoint.CaptureMouse();
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (_draggedPoint == null) return;

            // Move the red dot to the new mouse position
            Point mousePos = e.GetPosition(DrawingCanvas);
            SetPointPosition(_draggedPoint, mousePos.X, mousePos.Y);

            // Recalculate the math immediately as it moves!
            UpdateCurve();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_draggedPoint != null)
            {
                _draggedPoint.ReleaseMouseCapture();
                _draggedPoint = null;
            }
        }

        // Helper to center the Ellipse exactly on the X,Y coordinate
        private void SetPointPosition(UIElement ellipse, double x, double y)
        {
            Canvas.SetLeft(ellipse, x - 8);
            Canvas.SetTop(ellipse, y - 8);
        }

        private Point GetPointPosition(UIElement ellipse)
        {
            return new Point(Canvas.GetLeft(ellipse) + 8, Canvas.GetTop(ellipse) + 8);
        }

        // ==========================================
        // 2. THE CAD MATH (CUBIC BEZIER POLYNOMIAL)
        // ==========================================
        private void UpdateCurve()
        {
            Point p0 = GetPointPosition(Point0);
            Point p1 = GetPointPosition(Point1);
            Point p2 = GetPointPosition(Point2);
            Point p3 = GetPointPosition(Point3);

            // Draw the gray construction lines connecting the points
            ControlPolygon.Points.Clear();
            ControlPolygon.Points.Add(p0);
            ControlPolygon.Points.Add(p1);
            ControlPolygon.Points.Add(p2);
            ControlPolygon.Points.Add(p3);

            // Draw the actual smooth curve
            BezierCurve.Points.Clear();

            // 't' represents the percentage along the curve (0.0 to 1.0)
            // We calculate 50 tiny straight line segments to trick the eye into seeing a smooth curve!
            for (double t = 0; t <= 1.0; t += 0.02)
            {
                // The exact mathematical formula from the textbook:
                double u = 1.0 - t;
                double tt = t * t;
                double uu = u * u;
                double uuu = uu * u;
                double ttt = tt * t;

                double x = (uuu * p0.X) + (3 * uu * t * p1.X) + (3 * u * tt * p2.X) + (ttt * p3.X);
                double y = (uuu * p0.Y) + (3 * uu * t * p1.Y) + (3 * u * tt * p2.Y) + (ttt * p3.Y);

                BezierCurve.Points.Add(new Point(x, y));
            }
        }
    }
}