# BAKU

<img src="./Assets/Baku.png" width="128">

High-performance [Grasshopper 3D](https://en.wikipedia.org/wiki/Grasshopper_3D) plugin for simulating large number of particles using GPU acceleration.

https://github.com/sean1832/BAKU/assets/84948038/42143e28-08ef-4de8-9c80-186beeea28a2

---

_⭐ If you like this project, please consider giving it a star!_

_💡 This project is early in development. If you encounter any issues, please report them in the [issues section](https://github.com/sean1832/BAKU/issues)._

---

### 📘 About

This project started as a personal experiment to learn about GPU programming and high-performance computing. The goal is to create a high-performance particle simulation plugin for Grasshopper 3D using GPU acceleration. The project is named after the Japanese mythical creature [Baku](<https://en.wikipedia.org/wiki/Baku_(spirit)>) which is said to consume nightmares.

### 🌟 Features

- Real-time simulation of large number of particles with [Boids algorithm](https://en.wikipedia.org/wiki/Boids)
  - 10k particles in real-time on a RTX 4090
  - Currently unoptimized (looping all the boids positions), expect better performance in future releases.
- GPU accelerated using [ILGPU](https://github.com/m4rs-mt/ILGPU)
  - Up to 100x faster than CPU
  - Supports CUDA and OpenCL
  - Supports CPU fallback
- Customizable boid and particle behaviors
- Particle trails

### 📝 Planned Features

- [ ] Spatial Hashing algorithm for performance improvement.
- [ ] Attractor and repeller points
- [ ] Obstacle avoidance
- [x] ~~Boundary repulsion (currently particles are confined to a box)~~
  - Boundary repulsion is implemented since [v0.1.0](https://github.com/sean1832/BAKU/releases/tag/0.1.0).

### 🖥️ Installation

- Download the latest release from the [releases page](https://github.com/sean1832/baku/releases/latest).
- Unzip the file under the Grasshopper Libraries folder.
- Unblock the DLLs by right-clicking on the files, selecting properties, and clicking the `Unblock` button.
- Restart Rhino and Grasshopper.

### 📜 License

This project is licensed under the Apache License 2.0 - see the [LICENSE](./LICENSE) file for details.

### 🤝 Contribution

Contributions are welcome! Please follow standard contribution guidelines.

### 🔗 References

- [Coding Adventure: Boids](https://www.youtube.com/watch?v=bqtqltqcQhw&ab_channel=SebastianLague)
