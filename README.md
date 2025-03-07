# SOLACE (Project Labyrinth)

# Summary
In this game, players will explore a labyrinth and search for the exit, fighting manifestations of Rufio’s inner turmoil along the way. When Rufio comes across one of these manifestations, he will enter a combat scene and will have to defeat the enemies he is faced with. Players will be able to collect different crystals in the overworld which will give Rufio different elemental abilities to use in battle. 

# Controls
The player will use the WASD keys to move Rufio around the map and use the mouse to adjust the camera that follows him. Players can either walk or run through the map, with running being activated by holding shift. Players will press E to interact with different objects and characters in the environment. 

# Core Mechanics
- Turn-Based Combat System
  - Combat System where the user and AI take turns attacking/defending/using abilities and continue until either side’s HP drops to zero. 
  - In this Mechanic, the game will run an instance that loops until the HP property of either the Player or the Enemies drops to zero. Until then, the game will alternate between the Player’s turn where they can attack/defend or use  abilities and the Enemy’s turn where they too can attack/defend or use abilities.  

    - Attack
    - Defend
    - Player : Abilities
    - Enemies : Attack Pattern/Abilities

- Collectable Abilities (Crystals/Shards)
  - The Player around the labyrinth can find crystals that they can pick-up and will hold different skills/abilities they can use in Turn-Based Combat.

  - Player will collect crystals (collectible items), the item is stored in the player’s data. The crystals hold certain skills/abilities which the player can use in combat. The skills/abilities have a set amount of uses and grabbing another  crystal of the same skill/ability will recharge the number of uses, extend it or upgrade it.        

- Inventory System
  - Just a simple Inventory where the player can check what skill shards they hold and what they do. Also what weapon the player is holding and other information.
  - Scriptable object for the inventory system, the inventoryManager will call the SO when an item is needed and save the items the player picks up to this Inventory Scriptable Object

