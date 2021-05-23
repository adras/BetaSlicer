# TODO

* DONE Create Gui Window
* DONE Render Cat.obj from veldrid sourcecode
* DONE Implement camera

## GUI
* Clean up GUI 
	- Remove everything that is not required for the final Slicer
	- Keep everything that might be handy in the future
	- Use F11 to step through every single line to identify what can be removed
* Default model scaling changed from 0.1f to 1 - Therefore the nearplane can be increased again
* Implement ImportedObject layer - see Design
* Add support to have Camera to look at ImportedObject
* Add support to click on imported models
* Add support to remove imported model
* Add list of imported models
* Add support to move imported models
* Add support to scale imported models
* Add support to rotate imported models

## Rendering 
* Clean up everthing that is not used / required
* Verify Skybox - Some sort of background is required, but a sky seems kinda fancy, figure something else out
* Check lighting and shadows
* The models are smoothed by some process, add an option to enable/disable that. 
	- In the end in a slicer you don't want your models to be smoothed, you want to see them how they were designed, every single polygon

## File Formats
* Add support for STL files
* Add support to load files by menu
* Add support to load files by drag & drop

## Camera
* DONE Make camera look at model
* DONE Fix scaling issue with small models and camera - Cat disappears when zooming closer
	Near plane was not small enough, therefore small models were clipped by the depth test. Changed Camera.near property to a smaller value

* Make camera rotate model
* Implement mouse control for camera

