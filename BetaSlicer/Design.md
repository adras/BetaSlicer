# Technology

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

