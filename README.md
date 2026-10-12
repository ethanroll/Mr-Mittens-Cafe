<h1 align="center"> Delivery Time</h1>

<p align="center">
  <em>(A Unity game where you make and serve drinks to animal npcs!)</em>
</p>

<br>

<p align="center">
  <img src="(insert screenshot or gif link here)" alt="Delivery Time gameplay" width="600">
</p>

<br>

<p align="center">
  <strong>Status:</strong> In progress
  &nbsp;&nbsp;|&nbsp;&nbsp;
  <strong>Engine:</strong> Unity
  &nbsp;&nbsp;|&nbsp;&nbsp;
  <strong>Language:</strong> C#
</p>

<br>
<hr>
<br>

<h2> About</h2>

<p>
  In this game, the player creates drinks by combining different elements using machines that prompt small minigames to add ingredients to the drinks. The player will then serve these orders to the NPCs and gain points corresponding to the given orders, and must do so within the time limit to maximize points earned.
</p>

<br>
<hr>
<br>

<h2> What's Done</h2>

<ul>
  <li>Core game loop: NPC customers queue up, order drinks/food, and get served by the player</li>
  <br>
  <li>Order system (<code>Order</code> / <code>OrderManager</code>)</li>
  <br>
  <li>Item hierarchy: <code>Drink</code> and <code>Food</code> subclasses built on a shared <code>Item</code> base class (cup size, temperature, ice level, milk type, drink type)</li>
  <br>
  <li>Drink-making machines: espresso, milk dispenser, and ice machine, with click-and-hold interaction</li>
  <br>
  <li>Coroutine-based espresso shot timing</li>
  <li>Progress bar system with indication lines (ice machine uses 0.25 / 0.50 / 1.00 markers)</li>
  <br>
  <li>Hotbar inventory</li>
  <br>
  <li>Scoring system using a <code>PointSheet</code> ScriptableObject</li>
  <br>
  <li>NPC queue manager (<code>NPC_QueueManager</code>) with a state machine (<code>NPC_State</code>)</li>
  <br>
  <li>UI managers for in-game menus/displays</li>
</ul>

<br>
<hr>
<br>

<h2> In Progress</h2>

<ul>
  <li>Polishing NPC queue behavior (counter occupancy, advancing after order completion)</li>
  <br>
  <li>Counter "focus" screen with drag-and-drop item slots</li>
  <br>
  <li>Refactoring machine scripts (shared mouse-hold logic into a base class/component)</li>
</ul>

<br>
<hr>
<br>

<h2> Planned</h2>

<ul>
  <li>Order ticket UI panel to show active orders</li>
  <br>
  <li>Level-based configuration with ScriptableObjects and a <code>LevelManager</code></li>
  <br>
  <li>Directional character animations (idle/walk) made in Aseprite</li>
  <br>
  <li>Add Sprites for Player, NPCs, and Environment</li>
</ul>

<br>
<hr>
<br>

<h2> Built With</h2>

<ul>
  <li>Unity 6 (2D), C#</li>
  <li>Unity Input System, TextMeshPro, Cinemachine, Aseprite Importer</li>
  <li>Aseprite for pixel art / animation</li>
  <li>Git + GitHub</li>
</ul>

<br>
<hr>
<br>

<h2> What I Worked On / Learned</h2>

<p>
  (insert 1-3 sentences here about what you built yourself or learned, like "Wrote the player controller from scratch and learned how Unity's physics works") This is my first-ever project on Unity, and on this journey I learned many new things about C#, game logic, and how Unity’s engine actually works. One thing I learned was how to effectively use the Coroutine system for the espresso machine, which set a consecutive timer to prompt the user to start the machine, and ended when detecting user input.
</p>

<br>
<hr>
<br>

<h2> How to Run</h2>

<br>
<hr>
<br>

<h2> Contact</h2>

<p>
  <strong>Ethan</strong><br>
  CS @ Cal Poly Pomona
</p>

<p>
  <a href="https://www.linkedin.com/in/ethan-caoile-641a9632a/?isSelfProfile=true">LinkedIn</a>
  &nbsp;|&nbsp;
  ejcaoile@cpp.edu
</p>
