# Slither.io Template - GAME 220
## Core Programming: Loops and Lists

This Unity template is designed for a **3-week, 6-session** course (Feb 4-16) teaching loops and lists through game development.

---

## 🎯 Learning Objectives

Students will learn:
- **Lists** (`List<T>`) for dynamic collections
- **for loops** for counted iteration
- **foreach loops** for collection iteration
- **while loops** for conditional iteration
- **Update vs FixedUpdate** for proper game timing

---

## 📅 Session Breakdown

### **Session 1 (Feb 4): Snake Movement & Input**
**Status:** ✅ Complete and functional

**What Works:**
- Snake head follows mouse cursor smoothly
- Smooth rotation toward movement direction
- Proper use of Update (input) and FixedUpdate (movement)

**Files to Explore:**
- `Player/PlayerSnakeController.cs` - `Update()`, `MoveHead()`

**Key Concepts:**
- steerAction.ReadValue<Vector2>() (Input System) → Camera.ScreenToWorldPoint
- Update() for input (responsive)
- FixedUpdate() for movement (consistent)

---

### **Session 2 (Feb 6): Snake Body Part 1**
**Status:** ✅ Complete and functional

**What Works:**
- Snake has 3 starting body segments
- Segments trail behind the head
- Simple List-based management

**Files to Explore:**
- `Player/PlayerSnakeController.cs` - `CreateStartingBody()`, `AddSegment()`
- `Player/SnakeSegment.cs` - Full file (MoveTowards following)

**Key Concepts:**
- `List<SnakeSegment>` declaration and initialization
- `for` loop to create starting segments
- `List.Add()` to grow the list

---

### **Session 3 (Feb 9): Snake Body Part 2 & Growth**
**Status:** ✅ Complete and functional

**What Works:**
- Each new segment follows the current last segment
- `Grow()` adds any number of segments
- Dynamic segment addition

**Files to Explore:**
- `Player/PlayerSnakeController.cs` - `AddSegment()`, `Grow()`

**Key Concepts:**
- `bodySegments[bodySegments.Count - 1]` to get the last item
- `for` loop in `Grow()` to add several segments
- `List.Count` grows as segments are added

---

### **Session 4 (Feb 11): Food System**
**Status:** ✅ Complete and functional

**What Works:**
- 20 food pellets spawn at start
- Food respawns when collected
- Snake grows on collection
- Score tracking

**Files to Explore:**
- `Food/FoodSpawner.cs` - Full file (List management, spawning loops)
- `Food/FoodPellet.cs` - Full file (OnTriggerEnter2D)
- `Player/PlayerSnakeController.cs` - `Grow()`, `OnTriggerEnter2D()`
- `Managers/ScoreManager.cs` - Full file (UI integration)

**Key Concepts:**
- `List<FoodPellet>` to track active food
- `for` loop for batch spawning
- `while` loop for spawn position validation
- `List.Remove()` when food is collected
- OnTriggerEnter2D for collision detection

---

### **Session 5 (Feb 13): Collision Detection**
**Status:** ✅ Complete and functional

**What Works:**
- Player dies when its head hits an AI snake
- Boundary wrapping (like Pac-Man)
- Game over state
- Restart functionality (press R)

**Files to Explore:**
- `Player/PlayerSnakeController.cs` - `CheckBoundaries()`, `Die()`
- `Utils/BoundsHelper.cs` - Full file (Static utility methods)
- `Managers/GameManager.cs` - `GameOver()`, `RestartGame()`

**Key Concepts:**
- `foreach` loop to move every segment when the head wraps
- OnTriggerEnter2D + CompareTag("AISnake") for death
- Static helper methods
- Boundary wrapping with modulo-like logic

---

### **Session 6 (Feb 16): AI Snakes & Polish**
**Status:** ✅ Complete and functional

**What Works:**
- 3 AI snakes spawn and wander randomly
- AI changes direction periodically
- AI has body segments (reuses player code)
- Player can collide with AI
- AI can eat food and grow

**Files to Explore:**
- `AI/AISnakeController.cs` - Full file (Code reuse, timers, random direction)
- `Managers/GameManager.cs` - `SpawnAISnakes()`, `SpawnSingleAISnake()`

**Key Concepts:**
- Code reuse (AI uses the same List and loop patterns as the player)
- Timers with Time.deltaTime
- Random.insideUnitCircle for random directions
- `for` loop to spawn multiple AI
- `foreach` loop to cleanup AI on game over
- `List.Clear()` to empty a list

---

## 🎮 How to Use This Template

### For Teachers:
1. **Session 1**: Focus on Update vs FixedUpdate concepts. Have students trace the mouse input → movement flow.
2. **Session 2**: Introduce Lists. Have students add segments manually and observe List.Add().
3. **Session 3**: Growth. Have students change `Grow()` amounts and trace how each segment picks its target.
4. **Session 4**: Practice List operations. Have students modify spawn count and observe loops.
5. **Session 5**: Boundaries and game over. Swap wrapping for clamping and trace the GameOver() cleanup.
6. **Session 6**: Emphasize code reuse. AI reuses player patterns - this is good design!

### For Students:
- **Read the script header**, then its section of `LOOPS_AND_LISTS_CONCEPTS.md`. That's where the WHY and HOW are.
- **Experiment!** Change numbers in the Inspector and see what happens.
- **Add features!** Try adding: speed boost, different food types, score multipliers.
- **Debug!** Use the Gizmos in the Scene view: green box = play bounds, where food and AI actually spawn. The cyan box is GameManager's `spawnAreaMin/Max`, which only draws.

---

## 📊 Loop & List Summary

### Lists Used:
1. `List<SnakeSegment> bodySegments` - Player/AI body management
2. `List<FoodPellet> activeFoodPellets` - Food tracking
3. `List<AISnakeController> aiSnakes` - AI management

### Loops Used:
1. **for loops**: Build starting body, grow, spawn food, spawn AI
2. **foreach loops**: Move segments on wrap, cleanup segments and AI, check AI distances
3. **while loops**: Food spawn position retry

---

## 🔧 Scene Setup Instructions

### Required GameObjects in SlitherTemplate_Base:

1. **Main Camera** (already exists)
   - Orthographic mode
   - Size: 10

2. **GameManager** (empty GameObject)
   - Add `GameManager.cs`
   - Set AI Snake Prefab reference
   - Set number of AI snakes: 3

3. **FoodSpawner** (empty GameObject)
   - Add `FoodSpawner.cs`
   - Set Food Prefab reference
   - Initial food count: 20

4. **ScoreManager** (empty GameObject)
   - Add `ScoreManager.cs`
   - Create UI Canvas → Text for score
   - Create UI Canvas → Text for game over

5. **PlayerSnake** (GameObject with SpriteRenderer)
   - Add `PlayerSnakeController.cs`
   - Add CircleCollider2D (Is Trigger: true)
   - Add Rigidbody2D (Body Type: Kinematic)
   - Set Segment Prefab reference
   - Tag: "Player"

### Required Prefabs:

1. **SnakeSegment.prefab**
   - SpriteRenderer (circle, scale 0.8)
   - CircleCollider2D (trigger, radius 0.4)
   - SnakeSegment.cs

2. **FoodPellet.prefab**
   - SpriteRenderer (circle, scale 0.3)
   - CircleCollider2D (trigger, radius 0.15)
   - FoodPellet.cs
   - Tag: "Food"

3. **AISnake.prefab**
   - SpriteRenderer (circle, scale 1)
   - CircleCollider2D (trigger, radius 0.5)
   - Rigidbody2D (kinematic)
   - AISnakeController.cs
   - Set Segment Prefab reference
   - Tag: "AISnake"

### Required Tags:
- Player
- Food
- AISnake

---

## 🎓 Teaching Tips

### Session 1: Update vs FixedUpdate
**Why it matters:** Input needs to be responsive (Update), but movement needs to be consistent for physics (FixedUpdate).

**Demo:** Have students add a Debug.Log to both Update and FixedUpdate. They'll see Update runs at variable rates (60-144fps) while FixedUpdate is consistent (50fps).

### Session 2-3: Lists vs Arrays
**Why Lists:** Arrays have fixed size. Lists can grow/shrink dynamically. Perfect for snake segments!

**Demo:** Show what happens if you try to add a segment to an array (can't!). Then show List.Add() working perfectly.

### Session 3: The Follow Chain
**Why the last item:** A new segment always follows `bodySegments[bodySegments.Count - 1]`, so the body forms a chain.

**Demo:** Draw on whiteboard: HEAD <- Seg0 <- Seg1 <- Seg2. Add Seg3 and show it points at Seg2.

### Session 4: While Loops
**When to use:** When you don't know how many iterations you need, but you know a condition to stop.

**Example:** Spawning food - keep trying random positions WHILE they're too close to snakes.

### Session 5: Wrapping and Cleanup
**Why foreach on wrap:** When the head teleports, every segment must move by the same offset or the body stretches across the screen.

**Cleanup:** GameOver() needs both `Destroy()` and `List.Clear()` (see LOOPS_AND_LISTS_CONCEPTS.md section 7).

### Session 6: Code Reuse
**Design pattern:** Player and AI both need a List and the same segment management. The patterns carry over.

**Demo:** Show how AISnakeController reuses the exact same patterns. Change mouse input → random direction. Everything else is the same!

---

## 🚀 Extension Ideas

Once students complete all 6 sessions, challenge them with:

### Easy:
- Different food colors worth different points
- Speed boost power-ups
- Snake speed increases as it grows
- Add sound effects

### Medium:
- Minimap showing full play area
- Different AI personalities (aggressive, defensive, food-focused)
- Trail effects behind snake
- Camera follows player smoothly

### Hard:
- AI pathfinding to food
- Local multiplayer (2 players, split controls)
- Obstacles that kill snakes
- Boss AI with special abilities
- Re-add a `Queue<Vector3>` position history so segments follow the head's exact path (Enqueue/Dequeue, `while` loop to cap its size)
- Self-collision: a `for` loop over `bodySegments` starting at index 3 (the first few are always touching the head), with `break` on the first hit

### Advanced:
- Online multiplayer (requires networking knowledge)
- Procedural map generation
- Replay system
- Leaderboards with name entry

---

## 📝 Common Issues & Solutions

### Issue: Segments bunch up
**Solution:** Increase `segmentSpacing` on the snake (it overrides SnakeSegment's `followDistance`) or raise SnakeSegment `moveSpeed`

### Issue: Food spawns inside snake
**Solution:** `FoodSpawner.IsPositionValid()` checks distance - increase `minDistanceFromSnakes`

### Issue: AI gets stuck in corners
**Solution:** Add boundary avoidance in `AISnakeController.ChangeDirection()`

### Issue: Performance drops with long snakes
**Solution:** Limit max segments or use object pooling (advanced)

---

## 📖 Key Vocabulary

- **Update()**: Runs every frame at variable rate for input
- **FixedUpdate()**: Runs at fixed 50Hz rate for physics
- **List<T>**: Dynamic collection that can grow/shrink
- **for loop**: Counted iteration (known number of iterations)
- **foreach loop**: Iterate all items in a collection
- **while loop**: Conditional iteration (unknown iterations)
- **MoveTowards**: Move toward a point at a constant speed
- **OnTriggerEnter2D**: Unity collision event
- **CompareTag**: Fast way to check GameObject tags

---

## ✅ Verification Checklist

After implementation, verify:

- [ ] Session 1: Snake head follows mouse smoothly
- [ ] Session 2: 3 segments spawn and trail behind
- [ ] Session 3: Snake grows and new segments join the end of the chain
- [ ] Session 4: Food spawns, respawns, snake grows, score updates
- [ ] Session 5: Hitting an AI kills the player, boundaries wrap
- [ ] Session 6: 3 AI snakes wander, player can collide with them
- [ ] Every script header names its `LOOPS_AND_LISTS_CONCEPTS.md` section
- [ ] Gizmos show the play bounds (green) and GameManager's spawn area (cyan) in the editor
- [ ] Press R restarts the game

---

## 🎯 Assessment Ideas

### Formative (during sessions):
- Can students explain why Update vs FixedUpdate matters?
- Can students add a segment to the list manually?
- Can students explain why `Count - 1` is the last index?
- Can students explain what foreach does?

### Summative (end of 3 weeks):
- Add a new food type that's worth 50 points (tests List iteration, collision)
- Make AI avoid boundaries before hitting them (tests conditionals, timers)
- Add a speed boost power-up (tests OnTriggerEnter2D, temporary effects)
- Create a second AI type with different behavior (tests code reuse)

---

**Built with ❤️ for GAME 220 students**
**Happy coding!** 🐍
