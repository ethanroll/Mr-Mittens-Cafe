# Functional Specification: Mr. Mittens Cafe

## 1. Project Overview: Technical Stack
* **Genre/Style:** 2.5D top-down cozy cafe management / simulation project using 2D sprites within the Unity 3D engine.
* **Architecture:** Component-driven development prioritizing rapid iteration, modularity, and data separation through C# `MonoBehaviour` scripts, C# `Action` events, and `ScriptableObjects` to maximize performance and maintainability.
* **Render & Input Pipeline:** Universal Render Pipeline (URP) with 2D/Y-axis sorting and Unity New Input System.

---

## 2. Feature Specification: NPC Line Queue & Station Flow System

### Goal
Manage customer flow from store entrance to the ordering counter and pick-up counter, maintaining queue order and ensuring single-occupancy for active service positions.

### 1. Technical Requirements
* **NPC State Machine (`NPC_Movement.cs`):**
  * `Spawn`: Instantiate NPC at entrance.
  * `WalkToCounter`: Walk along waypoints toward the queue.
  * `InQueue`: Wait in designated line index (`queueSpotsStart[]`).
  * `WaitAtCounterState`: Interact with player at ordering register.
  * `WalkToPickup`: Transition to the pick-up area after ordering.
  * `WaitForPickup`: Wait for drink fulfillment.
  * `LeaveImpatient`: Exit store early if patience runs out.
* **Queue Manager (`NPC_QueueManager.cs`):**
  * Maintain dynamic lists (`NPC_StartLine`, `NPC_EndLine`).
  * Track spot availability safely with boundary bounds checks.
  * Listen for order completion and departure events to shift line positions forward.

### 2. Logic Flow & Dual-Station Architecture
1. **Ordering Register (Station A):**
   * When `WaitAtCounter` spot is vacant, front NPC advances.
   * `counterOccupied` flag is set to `true`.
   * NPC triggers order dialogue & broadcasts `OnOrderGenerated`.
   * Upon order placement, NPC transitions to `WalkToPickup` (Station B), setting `counterOccupied = false` to allow the next customer in line to order immediately.
2. **Pick-up Counter (Station B):**
   * Customer waits at Station B until player hands over matching item from hotbar.
   * On validation success, customer departs store and frees Station B.

### 3. Edge Cases & Patience System
* **Patience Meter:** Each NPC has a `patienceTimer`. If it reaches 0 while queued or waiting at pickup, the NPC shifts to `LeaveImpatient` state, displays disappointed dialogue, and exits without paying.
* **Queue Re-indexing:** When an NPC departs or leaves early, `LeaveLine()` re-indexes remaining NPCs and triggers movement to their new positions.

---

## 3. Feature Specification: Drink & Food Ordering Logic

### Goal
Create a dynamic, data-driven system for generating unique customer orders and validating finished products against order requirements.

### 1. Technical Requirements
* **Recipe & Order Data (`ScriptableObject` & Data Structures):**
  * `RecipeSO` (ScriptableObject): Defines base drink/food types, sprite icons, base costs, and allowed options.
  * `Drink` / `Food` Data Models:
    * Base Type: Espresso, Americano, Latte, Tea, Pastry, Savory.
    * Modifiers: Cup Size (Small, Medium, Large), Temperature (Hot, Iced), Ice Level (None, Light, Regular, Extra), Milk Type (Whole, Oat, Almond, None), Espresso Shots (0–4), Water.
* **Order Generator (`OrderManager.cs`):**
  * Randomly generates valid recipe combinations.
  * Assigns a unique `GUID` or `OrderID` and customer name to each order.
* **Verification System:**
  * Property-matching validation function comparing customer `Order` vs player's currently active hotbar `Item`.

### 2. Logic Flow
1. **Trigger:** NPC reaches ordering station $\rightarrow$ `GenerateRandomOrder()` creates order instance $\rightarrow$ `OrderEvents.OnOrderGenerated` event is raised.
2. **Fulfillment:** Player approaches customer at Pick-up station holding an item in the active hotbar slot.
3. **Validation:**
   * **Full Match (`true`):** Customer accepts order, pays money/tips, displays happy dialogue, and triggers `OrderEvents.OnOrderFulfilled`.
   * **Mismatch (`false`):** Displays error prompt ("Try again"), item remains in player hotbar, order remains active.

---

## 4. Feature Specification: Player Interaction & Hotbar System

### Goal
Enable fluid 2.5D top-down player movement, proximity-based interaction, and a multi-slot hotbar item management system.

### 1. Technical Requirements
* **Player Controller (`PlayerMovement.cs`):**
  * Top-down physics movement using `Rigidbody2D` and `Time.fixedDeltaTime`.
  * New Input System callback handler (`OnMovement`, `OnInteract`).
* **Interaction Proximity Detector (`InteractionDetector.cs`):**
  * Proximity detection using 2D Trigger Colliders or `Physics2D.OverlapCircle`.
  * Multi-target proximity tracking using `HashSet<IInteractable>` to handle overlapping interactable zones and select the closest object.
* **Hotbar & Item Carrying System (`HotbarManager.cs`, `Inventory.cs`):**
  * 10-slot persistent UI Hotbar (`Keys 1-0`).
  * PickUp/Drop/Clear functions for workstation interactions (Espresso Machine, Milk Dispenser, Ice Machine, Trash Can).
  * Selection state prevents accidental item switching while actively operating a machine (`drinkIsBusy`).

### 2. Rendering & Sprite Sorting
* **Y-Axis Sorting:** Configured via URP 2D Transparency Sort Axis `(X:0, Y:1, Z:0)` or dynamic sorting order `-(transform.position.y * 100)` to ensure player and NPCs correctly sort behind and in front of counters.

---

## 5. Feature Specification: Order Tracking UI Dashboard

### Goal
Provide a visual dashboard for the player to track active orders, monitor customer patience timers, and prioritize workflow.

### 1. Technical Requirements
* **Order Tracking UI (Unity UI / Canvas):**
  * **Order Card Prefab:** UI element displaying Customer Name/ID, Drink Icon, TextMeshPro description of recipe modifiers, and a visual Patience Timer Bar.
  * **Layout Group:** Horizontal/Vertical Layout Group at top of HUD auto-arranging active cards.
* **Order Tracking Manager (`OrderTrackingUI.cs`):**
  * Listens to `OrderEvents.OnOrderGenerated` to instantiate Order Cards.
  * Listens to `OrderEvents.OnOrderFulfilled` / `OnOrderFailed` to animate card removal.

### 2. Visual Cues & UX
* **Color-Coding:** Status borders reflecting patience level (Green = fresh, Yellow = waiting, Red = urgent/impatience warning).
* **Toast Notifications:** Brief floating text toasts (`ToastManager.cs`) confirming actions (e.g., "Item removed", "Cannot switch items").
