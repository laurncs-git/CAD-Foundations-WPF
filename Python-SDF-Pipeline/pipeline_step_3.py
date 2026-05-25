import numpy as np
import matplotlib.pyplot as plt
from skimage import measure
from mpl_toolkits.mplot3d.art3d import Poly3DCollection

# ==========================================
# STAGE 3: SDF BOOLEAN CUT
# ==========================================
print("Calculating SDF Math to cut a hole...")

grid_size = 20
x, y, z = np.mgrid[0:grid_size, 0:grid_size, 0:grid_size]

# 1. SHAPE A: The Base Ring (from Step 2)
center_x, center_y = 10, 10
distance_from_center = np.sqrt((x - center_x)**2 + (y - center_y)**2)
ring_thickness = np.minimum(8 - distance_from_center, distance_from_center - 4)
ring_height = np.minimum(z - 5, 14 - z)
base_sdf = np.minimum(ring_thickness, ring_height)

# 2. SHAPE B: The Cutting Cylinder
# We want to drill a hole right through the side of the ring.
# We create a cylinder running along the X-axis, centered at Y=10, Z=9.5
cut_radius = 2.5
distance_from_x_axis = np.sqrt((y - 10)**2 + (z - 9.5)**2)

# If distance is less than radius, it's positive (inside the cutting cylinder)
cutting_sdf = cut_radius - distance_from_x_axis

# 3. THE BOOLEAN MAGIC (Subtraction)
# To subtract, we invert the cutting shape (make it negative) 
# and use np.minimum to combine them. 
# It literally means: "Keep the base shape, UNLESS the cutting shape says 'Empty Space'."
final_sdf = np.minimum(base_sdf, -cutting_sdf)


# ==========================================
# 4. RUN MARCHING CUBES ON THE NEW SHAPE
# ==========================================
# Now we ask the computer to wrap a smooth skin around our new, cut math field.
verts, faces, normals, values = measure.marching_cubes(final_sdf, level=0.0)

# ==========================================
# VISUALIZE THE CUT MESH
# ==========================================
fig = plt.figure(figsize=(8, 8))
ax = fig.add_subplot(111, projection='3d')

mesh = Poly3DCollection(verts[faces], alpha=0.8, edgecolor='black', linewidths=0.5)
mesh.set_facecolor('cyan')
ax.add_collection3d(mesh)

ax.set_xlim(0, grid_size)
ax.set_ylim(0, grid_size)
ax.set_zlim(0, grid_size)

ax.set_title("Stage 3: SDF Boolean Cut (Subtracting a Cylinder)")
ax.set_xlabel("X-axis")
ax.set_ylabel("Y-axis")
ax.set_zlabel("Z-axis")

plt.show()