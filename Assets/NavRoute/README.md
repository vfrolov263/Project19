# NavRoute — Navigation System for Unity

**NavRoute** is a navigation extension package for Unity that builds on top of the **AI Navigation (NavMesh)** system. It provides graph-based route planning, weighted Point of Interest (POI) selection, and advanced pathfinding algorithms for NPC movement.

---

## ✨ Features

- **Graph-based route planning** — Build complex route networks between POIs
- **8 pathfinding algorithms** — BFS, DFS, Dijkstra, A*, and bidirectional variants
- **Weighted POI selection** — Control NPC behavior with priorities
- **Editor-friendly visualization** — See your routes and points in the Scene view
- **Easy integration** — Simple `RouteWalker` component for NPCs
- **Demo scene included** — Get started in minutes

---

## 📦 Requirements

- Unity **2021.3** or newer
- **AI Navigation** package (installed via Package Manager)

---

## 🚀 Quick Start

1. Install **AI Navigation** via `Window → Package Manager`
2. Import `NavRoute` into your project
3. Add the `RouteService` prefab to your scene
4. Add `Poi` and `Route` objects to build your graph
5. Add `RouteWalker` to your NPC and assign references
6. Call `StartWalk()` to begin navigation

For detailed instructions, see the **[Manual.pdf](Documentation/Manual.pdf)**.

---

## 📁 Package Structure

NavRoute/
├── Runtime/ # Core scripts (Algorithms, Core, Movement, Navigation, Utils)
├── Prefabs/ # Ready-to-use prefabs (RouteService, Poi, Waypoint)
├── Demo/ # Demo scene with example setup
└── Documentation/ # Full manual and support files

---

## 📖 Documentation

- **Full Manual:** [Documentation/Manual.pdf](Documentation/Manual.pdf)
- **Support:** [Documentation/Support.md](Documentation/Support.md)
- **Changelog:** [CHANGELOG.md](CHANGELOG.md)

---

## 🤝 Support

If you find this package useful, you can support its development.  
See [Support.md](Documentation/Support.md) for details.

---

## 📄 License

This package is provided as-is. See [LICENSE.md](LICENSE.md) for details.

---

*Made with ❤️ for the Unity community*