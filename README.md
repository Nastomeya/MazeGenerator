# Maze Generator

A procedural 3D dungeon maze generator built in **Unity (2022.3.62f2 / URP)** using a **Recursive Backtracking** algorithm with support for both **Square** and **Hexagonal** tile grids, bitwise wall/object formation management, circle-packed props, and interactive step-by-step or full-maze generation.

## Features

- **Dual Grid Topologies**
  - **Hexagonal Maze (`HexagonMaze`)**: 6-neighbor staggered hex grid (`HexagonNeighbours` / `HexagonFormation`).
  - **Square Maze (`SquareMaze`)**: 4-neighbor orthogonal grid (`SquareNeighbours` / `SquareFormation`).
- **Interactive & Step-by-Step Generation**
  - Generate an entire maze at once (**Create Entire Maze**) or step through the algorithm tile by tile (**Create Next Single Tile**) via `MazeManager` and `BaseMaze`.
  - Visualizes traversal with a moving position marker, optional fog/reveal mask (`MazeMask`), and per-tile step/index counters (`TrackText`, `RevealObjectController`).
  - `StraightPathController` limits consecutive straight moves to produce more varied, winding corridors.
- **Bitwise Wall & Modular Object Sets**
  - Each tile tracks open and closed edges with bitmasks (`Bitwise`) and automatically deduplicates shared walls/objects between adjacent visited tiles (`OverlappingObjectsReduction`).
  - `MazeObjectSettings` ScriptableObjects configure modular `IMazeObjects` layers per tile (brick walls, rock walls, torches, creatures, etc.) that can be toggled at runtime or in the Inspector.
- **Procedural Circle-Packing & Prop Placement**
  - `BaseCirclePacking`, `SquareCirclePackingWall`, and `HexagonCirclePackingObject` scatter non-overlapping props (such as rocks) along active wall regions using `CirclePackingSetting` and an object pool (`JasonPoolSystem.Pool`).
  - `HexagonTorch` and `HexagonCreatureSet` place torches and creatures along active wall segments using `ObjectsSelectorSetting`.
  - `StoneFloor` adds subtle random floor rotation variation (`FloorRotation`), while `WallsDetector` / `PillarPositioner` manage corner pillars.
- **Touch & Camera Controls**
  - Orbit, pan, and pinch-to-zoom camera controls powered by `com.jctool.inputhandler`, with `HintsSwitcher` adapting on-screen control hints to the active input mode.
- **Unity Editor Tooling**
  - **Custom Maze Inspectors (`SquareMazeEditor`, `HexagonMazeEditor`)**: Toggle individual `IMazeObjects` layers, switch between **Display Index Num** and **Display Squence**, and step through **Next Tile**, **Loop Through All Tiles**, or **Reset** in Play Mode.
  - **Single-Tile Debugger (`SingleTileInDebugMode` / `SingleTileInDebugModeEditor`)**: Test individual tile object sets and cycle through or manually input bitwise formation values.
  - **Reveal & Wall Tools (`RevealObjectControllerEditor`, `WallPositionerEditor`)**: Toggle tile text/markers and test radial wall/pillar placement.

## Requirements

- **Unity**: `2022.3.62f2` (or compatible Unity 2022.3 LTS)
- **Render Pipeline**: Universal Render Pipeline (`com.unity.render-pipelines.universal` `14.0.12`)

### Key Packages & Dependencies

Defined in [`Packages/manifest.json`](Packages/manifest.json):

- [UniTask](https://github.com/Cysharp/UniTask) (`com.cysharp.unitask`)
- [NaughtyAttributes](Assets/NaughtyAttributes) (included in `Assets/NaughtyAttributes`)
- JCTool UPM packages:
  - `com.jctool.debugextensions`
  - `com.jctool.easysingleton`
  - `com.jctool.extensions`
  - `com.jctool.inputhandler`
  - `com.jctool.names`
- Unity packages: `TextMeshPro` (`3.0.9`), `AI Navigation` (`1.1.6`), `UGUI`, `Timeline`

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/Nastomeya/MazeGenerator.git
   ```
2. **Open in Unity**
   - Open the project folder in Unity Hub using Unity **`2022.3.62f2`**.
   - Unity will automatically resolve the Git-based UPM dependencies in `Packages/manifest.json`.
3. **Open a Scene**
   - [`Assets/Scenes/Maze Scene - Mobile.unity`](Assets/Scenes/Maze%20Scene%20-%20Mobile.unity) *(enabled in Build Settings)* — includes the runtime UI with **Width** (`2–10`) and **Height** (`1–10`) sliders, **Create Next Single Tile**, **Create Entire Maze**, and touch/mouse camera controls.
   - [`Assets/Scenes/Maze Scene.unity`](Assets/Scenes/Maze%20Scene.unity) — editor/desktop scene.
4. **Press Play**
   - Adjust the **Width** and **Height** sliders and click **Create Next Single Tile** to watch the recursive backtracker carve the maze step by step, or **Create Entire Maze** to complete it immediately.

## Project Structure

```text
Assets/
├── 3D Models/           # Character/creature models and animations (e.g., Cyclops)
├── Animation/           # Animator controllers and animation assets
├── Materials/           # Shared materials
├── NaughtyAttributes/   # Inspector attribute extensions
├── Prefabs/
│   ├── Maze/            # Tile, wall, pillar, torch, creature, and circle-packing prefabs
│   └── Particles/       # Torch fire and effect particle prefabs
├── Scenes/
│   ├── Maze Scene - Mobile.unity
│   └── Maze Scene.unity
├── ScriptableData/      # MazeObjectSettings, CirclePackingSetting, FloorRotation, selectors
├── Scripts/
│   ├── AnimatorState/   # Animator state behaviors
│   ├── Edit/            # Custom Inspectors and editor utilities
│   ├── Maze/
│   │   ├── CirclePacking/  # Circle-packing, wall, torch, and creature placement
│   │   └── ...             # BaseMaze, SquareMaze, HexagonMaze, Neighbours, Tiles
│   ├── PoolSystem_Jason/   # Generic MonoBehaviour object pooling
│   ├── Scriptables/        # ScriptableObject definitions
│   ├── UIs/                # UI helpers (slider value display, etc.)
│   └── Utilities/          # Bitwise helpers, math utilities, JSON/list helpers
├── Settings/            # URP pipeline assets (Low, Medium, High)
└── Textures/            # UI sprites, icons, and effect textures
```

## How It Works

1. **Grid Initialization (`BaseMaze.InitialMaze`)**: Spawns `Width × Height` instances of `TileWithMazeObjectSets`, initializes neighbor topology (`SquareNeighbours` or `HexagonNeighbours`), and selects a random starting tile.
2. **Recursive Backtracking (`BaseMaze.BacktrackingPath`)**:
   - Picks a random valid unvisited neighbor (modulated by `StraightPathController` to limit long straight runs).
   - Pushes the current tile onto a `Stack<TileWithMazeObjectSets>`, removes the shared wall bit from both tiles' formations (`UpdateRemovalObjectFormation`), and deduplicates overlapping adjacent walls (`OverlappingObjectsReduction`).
   - Yields until **Create Next Single Tile** is triggered (or runs continuously when **Create Entire Maze** is active).
   - When a dead end is reached, pops from the stack and backtracks until all tiles are visited.
3. **Object Layer Updates (`MazeObjsController` & `BaseCirclePacking`)**: Each tile updates its active `IMazeObjects` components (walls, pillars, circle-packed rocks, torches, creatures) whenever its bitwise formation changes.

## Credits

- **Developer**: Joseph Chiang ([Nastomeya](https://github.com/Nastomeya))
