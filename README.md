# LaserSim
This is an old game I created with Unity, originally for prototyping a physics-based character controller system, which I added a function for the player to grab interactable objects.  
From there, I added an optics system that supports reflection and refraction (using Snell's law) when I was learning the laws of refraction and reflection in physics class.
Additionally, there were plans to create a digital logic system, but I never got to doing that. At the moment, the player can create wires between logic-capable objects, like the laser emitter and receiver, lamps, buttons, and logic gates.

## Controls
Use the mouse to look around, WASD to move, and space to jump.

The player has access to two tools: Grab and Wire.  
To switch tools, the player can use the Q and E buttons, or shift and scroll.
### Grab tool
 - Left click to grab or drop an interactable object
 - Right click with no held object to create a cuboid with random dimensions
 - Right click with a held object to delete it
 - Middle click with a held object to launch it, or while holding shift to freeze it.
 - Scroll with a held object to move it closer or farther from you.
### Wire tool
 - Left click an input or output device to start a wire connection
 - Left click another object of the opposite kind to create a connection, or while holding shift to delete a connection
 - Right click to cancel making a connection
