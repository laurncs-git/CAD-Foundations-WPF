import numpy as np
import matplotlib.pyplot as plt
from skimage import measure
from mpl_toolkits.mplot3d.art3d import Poly3DCollection

# ==========================================
# STAGE 2: SMOOTH SURFACE EXTRACTION
# ==========================================
print("Calculating Smooth Mathematical Surface...")

# 1. Setup the 3D space
grid_size = 20
x, y, z = np.mgrid[0:grid_size, 0:grid_size, 0:grid_size]

center_x, center_y = 10, 10
radius_outer = 8
radius_inner = 4

# 2. Build a Continuous Mathematical Field (SDF)
# Instead of Yes/No blocks, every point calculates its exact distance to the ideal shape.
distance_from_center = np.sqrt((x - center_x)**2 + (y - center_y)**2)

# We want values > 0 to be "Inside" and < 0 to be "Outside"
# ring_thickness creates the smooth walls. ring_height creates the flat top and bottom.
ring_thickness = np.minimum(radius_outer - distance_from_center, distance_from_center - radius_inner)
ring_height = np.minimum(z - 5, 14 - z)

# Combine them: The shape exists where BOTH conditions are positive.
sdf_volume = np.minimum(ring_thickness, ring_height)

# 3. The Smoothing Engine (Marching Cubes)
# This algorithm hunts through our SDF volume for EXACTLY where the value hits 0.0.
# It then draws a smooth "skin" of polygons over that exact boundary.
verts, faces, normals, values = measure.marching_cubes(sdf_volume, level=0.0)


# ==========================================
# VISUALIZE THE SMOOTH MESH
# ==========================================
fig = plt.figure(figsize=(8, 8))
ax = fig.add_subplot(111, projection='3d')

# Create the 3D polygon object from the vertices and faces we generated
mesh = Poly3DCollection(verts[faces], alpha=0.8, edgecolor='black', linewidths=0.5)
mesh.set_facecolor('cyan')
ax.add_collection3d(mesh)

# Keep the camera boundaries identical to Stage 1 so we can compare
ax.set_xlim(0, grid_size)
ax.set_ylim(0, grid_size)
ax.set_zlim(0, grid_size)

ax.set_title("Stage 2: Smooth Surface Extraction (Marching Cubes)")
ax.set_xlabel("X-axis")
ax.set_ylabel("Y-axis")
ax.set_zlabel("Z-axis (Slices)")

plt.show()