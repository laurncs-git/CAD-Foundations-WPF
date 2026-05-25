import numpy as np
import matplotlib.pyplot as plt

# ==========================================
# STAGE 1: THE VOXEL SCAN
# ==========================================
print("Generating Voxel Scan...")

# 1. Create a 3D grid (20x20x20 voxels)
grid_size = 20
x, y, z = np.indices((grid_size, grid_size, grid_size))

# 2. Define the exact center of our CT Scanner bed
center_x, center_y = 10, 10
radius_outer = 8
radius_inner = 4

# 3. Measure every voxel's distance from the center
distance_from_center = np.sqrt((x - center_x)**2 + (y - center_y)**2)

# 4. The Threshold Rule (Just like we did in 2D!)
# A voxel becomes "Solid" (True) IF:
# - It is inside the outer radius AND
# - It is outside the inner radius AND
# - It is sitting in the middle height of the scanner (z between 5 and 14)
voxels = (distance_from_center <= radius_outer) & \
         (distance_from_center >= radius_inner) & \
         (z >= 5) & (z < 15)

# ==========================================
# VISUALIZE THE RAW DATA
# ==========================================
fig = plt.figure(figsize=(8, 8))
ax = fig.add_subplot(111, projection='3d')

# ax.voxels automatically draws 3D cubes wherever our array is 'True'
ax.voxels(voxels, edgecolor='black', facecolors='cyan', alpha=0.7)

ax.set_title("Stage 1: Raw Voxel Scan (CT Output)")
ax.set_xlabel("X-axis")
ax.set_ylabel("Y-axis")
ax.set_zlabel("Z-axis (Slices)")

plt.show()