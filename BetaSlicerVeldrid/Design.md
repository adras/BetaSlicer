﻿# Technology

## GUI

For the graphical user interface this guide is being followed: https://mellinoe.wordpress.com/2017/01/18/net-core-game-engine/

The following libraries are used:
Veldrid
ImGui .NET

### Object selection, scaling, rotating etc

A circle aroudn the model to rotate it might be difficult to implement. Therefore stick to textboxes first
TBRotate - x - y - z
TBScale - x - y - z
TBTranslate - x - y - z

Other than slicer, keep the values as they are, don't just apply them and then set the textbox content to zero. It makes sense to be able to look up the values by which a model is transformed.

## STL file format
Seems to be really easy to read. The binary representation is just basic datatypes as binary

## File Types
There seems to be a requirement for different layers, when it comes to rendering, slicing and file format reading.
It needs to be figured out which layers make sense. Especially from a performance viewpoint. It doesn't make sense to convert the model
5 times until it's imported.

## Slicing
Intersting projects for the slicing process:
https://github.com/gradientspace/gsSlicer
https://github.com/Jaszuk/UVDLPSlicerController

Interesting Slicer:
https://www.cotangent.io/open-source 

Uses: gsSlicer, geometry3Sharp, frame3Sharp, Clipper, WildMagic5 and GTEngine

## Gradientspace
This company made gsSlicer also seems to have a lot of mesh/geometry related tools/sources/information

There also seem to be apps which are based on gsLicer:
https://github.com/gradientspace/gsSlicerApps

## GCode
For gcode generation there's also a gradientspace library: https://github.com/gradientspace/gsGCode


## GameController
A game Controller can be initialized like this:

        private Sdl2ControllerTracker _controllerTracker;
            Sdl2Native.SDL_Init(SDLInitFlags.GameController);
            Sdl2ControllerTracker.CreateDefault(out _controllerTracker);

CreateDefault can also be called during the Update process to reconnect it again

# ImportedObject
Currently an .obj file is read as textured mesh, which is added to the scene. The textured mesh basically consists of a lot of vectors.

Add a min/max algorithm to get the min and max dimensions while the file is being read. From there on the center can be calculated.

Add a new Class which holds the Textured mesh along with this extra information

NOTE: A TexturedMesh already has a bounding box, so no min/max algorithm required

This can be used to have the camera to look at the model by:
Vector3 cameraPos = Camera.position;
Vector3 modelCenter = ImportedObject.center;

Vector3 lookDir = cameraPos - modelCenter; // Maybe normalized
Vector3 newCameraPos = cameraPos - (lookDir.Normalized) * ImportedObject.AllAxisSize.length