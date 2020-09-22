# C# z-buffer Project

## Introduction

This project uses C# (and winforms) to present a basic approach to the z-buffer rendering algorithm ([wikipedia](https://en.wikipedia.org/wiki/Z-buffering)) for objects in 3D space.

## Control Set

The below set of combinations of keys and functional keys (shift/ctrl) are used to control the simulation.

Where applicable (in Note column), keys may be modified with the Ctrl key to increase the value of action (ie. `<selected key> + Ctrl` will perfrom the appropriate action with higher value - translation by 10 points instead of 2, etc.)

Objects are always moved with respect to their local axis.

**Screen/Scene Controls**
| Key(s) | Action | Note |
| :---   | :---  | :--- |
| R | Refresh screen. | This action is called by default after every other action |
| R + Ctrl | Reset scene to initial view. | - |
| O | Decreases camera FOV by pi/12 | - |
| P | Increases camera FOV by pi/12 | - |
| 1 - 9 | Select the nth object in scene (numbered by programatic order - added to list) | Maximum of 9 objects are controllable with keys |
| 0 | Select all objects | - |

**Translation Controls**
| Key(s) | Action | Note |
| :---   | :---  | :--- |
| Right arrow | Translates selected objects towards negative X | May be increased with Ctrl |
| Left arrow | Translates selected objects towards positive X | May be increased with Ctrl |
| Up arrow | Translates selected objects towards negative Y | May be increased with Ctrl |
| Down arrow | Translates selected objects towards positive Y | May be increased with Ctrl |
| Z | Translates selected objects towards positive Z | May be increased with Ctrl |
| X | Translates selected objects towards negative Z | May be increased with Ctrl |

**Rotation Controls**
| Key(s) | Action | Note |
| :---   | :--- | :--- |
| A | Rotates selected objects around positive origin Y | May be increased with Ctrl |
| A + Shift | Rotates selected objects around positive local Y | May be increased with Ctrl |
| D | Rotates selected objects around negative origin Y | May be increased with Ctrl |
| D + Shift | Rotates selected objects around negative local Y | May be increased with Ctrl |
| Q | Rotates selected objects around positive origin Z | May be increased with Ctrl |
| Q + Shift | Rotates selected objects around positive local Z | May be increased with Ctrl |
| E | Rotates selected objects around negative origin Z | May be increased with Ctrl |
| E + Shift | Rotates selected objects around negative local Z | May be increased with Ctrl |
| W | Rotates selected objects around negative origin X | May be increased with Ctrl |
| W + Shift | Rotates selected objects around negative local X | May be increased with Ctrl |
| S | Rotates selected objects around positive origin X | May be increased with Ctrl |
| S + Shift | Rotates selected objects around positive local X | May be increased with Ctrl |

**Scale Controls**
| Key(s) | Action | Note |
| :---   | :---  | --- |
| . | Upscales selected objects | May be increased with Ctrl |
| , | Downscales selected objects | May be increased with Ctrl |
