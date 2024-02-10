# BAKU

<img src="./Assets/Baku.png" width="128">

High-performance [grasshopper]() plugin for simulating large number of particles using GPU acceleration.


https://github.com/sean1832/BAKU/assets/84948038/936c1b54-7e87-435c-8584-fc7941c79c6e



---

_⭐ If you like this project, please consider giving it a star!_

_💡 This project is early in development. If you encounter any issues, please report them in the [issues section]()._

---

### 🌟 Features

- Real-time simulation of large number of particles
  - 10k particles in real-time on a RTX 4090
  - Currently unoptimized, expect better performance in future releases.
- GPU accelerated using [ILGPU]()
  - Up to 100x faster than CPU
  - Supports CUDA and OpenCL
  - Supports CPU fallback
- Customizable boid and particle behaviors
- Particle trails

### 📝 Planned Features

- Attractor and repeller points
- Obstacle avoidance
- Boundary repulsion (currently particles are confined to a box)

### 🖥️ Installation

- Download the latest release from the [releases page]().
- Unzip the file under the Grasshopper Libraries folder.
- Unblock the DLLs by right-clicking on the files, selecting properties, and clicking the `Unblock` button.
- Restart Rhino and Grasshopper.

### 📜 License

This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.

### 🤝 Contribution

Contributions are welcome! Please read the [contribution guidelines](./CONTRIBUTING.md) first.
