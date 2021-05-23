# TODO

* DONE Create Gui Window
* DONE Render Cat.obj from veldrid sourcecode
* DONE Implement camera

## Camera
* DONE Make camera look at model
* DONE Fix scaling issue with small models and camera - Cat disappears when zooming closer
	Near plane was not small enough, therefore small models were clipped by the depth test. Changed Camera.near property to a smaller value

* Make camera rotate model
* Implement mouse control for camera

