# ReyceMe

Marc Rey
BFH
15.06.2021

## Features

### Checkpoints


### Customization: Underwater
#### Basics

* If camera is underwater then
  * buggy experiences water resistance, which slows the buggy down. Done with RigidBody-Drag.
  * the light is blueish and the sight a bit blurred/distorted. Done with postprocessing rather than a shader. Reason: Changing the existing project to URP would probably have taken to much valuable time. The downside is, the underwater light effects either apply to the entire frame or not at all. It seems not possible, if the frame is partioned by the water level, to only apply the postprocessing to the lower part.
  * buggy exhausts no smoke. Yet the buggy still causes sand to swirl.
  * there is a rescue functionality for the buggy since there are steep depths underwater where it could get stuck. There are 5 spots distributed over the terrain. If the buggy is in need for rescue, press "R" and it will be repositioned to the nearest spot. Also, the buggy's velocity will be set to 0 and the current vertical axis input as well. Since the buggy might be turned over, its rotation (x and z) will be reset.
  * the rescue mode can be abused for demo purpouses. By pressing keys "1" to "5" the buggy gets repositioned to these spots.

#### Bonus

* There are fish :D
  * The model and animation has been imported. 
  * The fish-world consists of "aquariums". Those mean the grid set up by a few points where fish want to swim to ("worms"). The fish has a behaviour where is decided where the fish wants to swim to. From all the worms the fish knows about, it randomly chooses one, changes direction and swims there. The swimming behaviour loops inside a coroutine.
  * If the buggy comes close to the fish, they will swim away (opposite and slightly faster behaviour than with the worms).
* The buggy could fall of the terrain. Thus I placed invisible walls all around the terrain.

#### Further Ideas

* Caustics.
* The fish do not know about the terrain and thus can "burry" in the sand. Optimize.
* Buggy could make waves and foam.
* Buggy flags should wave slower in water.
* Buggy should be abruptly slowed down, when falling into the water.
* The sand dust underwater should be slower.