# 📐 CAD Foundations & Computational Geometry (.NET & WPF)

This repository contains a series of applications built to explore the core mathematical foundations of CAD (Computer-Aided Design) software, 3D printing slicers, and spatial computing. 

Coming from a background in enterprise .NET/C# application development, I built these projects to bridge the gap between traditional UI engineering and hardcore computational geometry, translating the concepts from textbooks like *Computer Graphics: Principles and Practice* into working engines.

## 🛠️ Technology Stack
* **Languages:** C#, Python
* **Frameworks:** .NET Framework, WPF (Windows Presentation Foundation)
* **Libraries:** `System.Numerics` (Hardware-accelerated math), `HelixToolkit.Wpf`, `NumPy`, `Scikit-Image`

---

## 🚀 The Milestones

### 1. The Parametric Sketcher (2D Constraint Engine)
A WPF application demonstrating the foundation of 2D CAD sketching. It calculates the exact rotation required to snap a user-drawn line to a perfect 90-degree angle relative to a reference line.
* **Math concepts used:** 2D Vectors, Dot Products, Vector Normalization, Rotation Matrices.
* **Why it matters:** This is the foundational mathematics behind the constraint engines in software like SolidWorks and Fusion360.

> **[Insert your screenshot of the green perpendicular lines here]**

### 2. Implicit Geometry & Marching Cubes (Python Pipeline)
A Python-based pipeline that generates a smooth 3D shape from pure mathematical equations and exports it for manufacturing.
* **Math concepts used:** Signed Distance Fields (SDFs), Boolean Math (Subtracting shapes), Marching Cubes Algorithm.
* **Why it matters:** Demonstrates the ability to convert "Implicit" geometry (infinitely smooth math) into "Explicit" geometry (Boundary Representation / B-Reps) that machines can actually read.

> **[Insert your screenshot of the blue 3D python ring with the hole in it]**

### 3. The STL Inspector (3D Data Parsing)
A WPF desktop tool that parses a 3D printable `.STL` file (generated from the Python pipeline), renders it in a 3D viewport, and extracts the raw manufacturing data.
* **Math concepts used:** 3D Bounding Boxes, Mesh Traversal, Coordinate Centering.
* **Why it matters:** Proves the ability to handle massive datasets (e.g., 100,000+ vertices) and calculate the physical constraints required for a 3D printer's build volume.

> **[Insert your screenshot of the yellow ring in the WPF 3D viewport]**

### 4. The Bezier Sandbox (Parametric Curves)
A WPF canvas application that allows a user to interactively manipulate a smooth curve by dragging control points in real-time.
* **Math concepts used:** Cubic Polynomials, Bézier Curves, Linear Interpolation.
* **Why it matters:** Demonstrates an understanding of how industrial design software creates perfectly smooth curves (like airplane wings or car bodies) without relying on blocky triangles.

### 5. The Slicer Engine (Manufacturing Toolpath Generation)
A C# engine that takes a 3D mesh and mathematically slices it at a specific Z-height to generate the 2D blueprints required for 3D printing.
* **Math concepts used:** Triangle-Plane Intersections, Half-Spaces, Linear Interpolation (Lerp).
* **Why it matters:** This is a miniature version of the exact geometric calculations running under the hood of industry-standard slicing software like Ultimaker Cura or PrusaSlicer.

> **[Insert your screenshot of the cyan sliced circles on the black background]**

---

## 🧠 Continuous Learning
These projects were built while studying:
* *Computer Graphics: Principles and Practice* (Hughes, van Dam)
* *Geometry for Programmers* (Kaleniuk)
