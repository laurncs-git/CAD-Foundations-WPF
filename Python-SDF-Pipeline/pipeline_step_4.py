import numpy as np
from skimage import measure
import trimesh

# ==========================================
# 1. RUN THE SDF MATH (From Step 3)
# ==========================================
# ==========================================
# 1. HIGH RESOLUTION SDF MATH
# ==========================================
print("Calculating High-Res SDF Math...")
grid_size = 80  # <-- 4x Higher Resolution!
x, y, z = np.mgrid[0:grid_size, 0:grid_size, 0:grid_size]

center_x, center_y = 40, 40
distance_from_center = np.sqrt((x - center_x)**2 + (y - center_y)**2)

ring_thickness = np.minimum(32 - distance_from_center, distance_from_center - 16)
ring_height = np.minimum(z - 20, 56 - z)
base_sdf = np.minimum(ring_thickness, ring_height)

cut_radius = 10
distance_from_x_axis = np.sqrt((y - 40)**2 + (z - 38)**2)
cutting_sdf = cut_radius - distance_from_x_axis

final_sdf = np.minimum(base_sdf, -cutting_sdf)

# ==========================================
# 2. MARCHING CUBES (Math -> Triangles)
# ==========================================
print("Converting Math to Triangles...")
# verts = the X,Y,Z coordinates of the corners
# faces = which 3 corners connect to make a triangle
verts, faces, normals, values = measure.marching_cubes(final_sdf, level=0.0)

# ==========================================
# STAGE 4: EXPORT TO STL MESH
# ==========================================
print("Packaging Triangle Mesh...")

# We feed the raw vertices and faces into the trimesh library
mesh = trimesh.Trimesh(vertices=verts, faces=faces)

# We can ask the mesh to check itself for errors!
if mesh.is_watertight:
    print("Mesh Check: Perfect! No holes or cracks. Ready for 3D printing.")
else:
    print("Mesh Check: Warning, there are holes in the geometry.")

# Export the final file to your current folder
filename = "repaired_airplane_part.stl"
mesh.export(filename)

print(f"\nSUCCESS! Look in your VS Code folder for '{filename}'")