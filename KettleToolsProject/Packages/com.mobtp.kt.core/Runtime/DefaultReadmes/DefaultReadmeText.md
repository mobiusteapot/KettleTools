# Readme Objects
Readme objects can be displayed in a number of ways, or simply left in the editor for team members to read.

# Readme Components
Readmes can be attached to a Readme component to display on a relevant game object. These will be stripped on build.

# Readme Folders
Readmes can be attached directly to a folder, and will be drawn in the inspector when the folder is selected. These can be used to convey what the purpose of a given folder is.

# Readmes on Scenes
Readmes can be displayed when a scene is selected, or when a scene is open. To do this, press "Create Additional Scene Data" when a scene object is selected.

# Readmes on Canvases
Readmes can be used to generate information on a Canvas using the "Canvas Readme Display" component. Attach this to any child object in a Canvas, and select the "Update Canvas From Readme" option to generate the canvas, or update the text components if your Canvas has changed.

# Editing Workflow
For modifying or locking readmes, the Unity inspector should be put into "Debug Mode". The IsLocked bool value can then be disabled, to unlock the readme again.
![Alt](Packages/com.mobtp.kt.core/Runtime/DefaultReadmes/Images/AccessingDebugMode_1.png)
# 
![Alt](Packages/com.mobtp.kt.core/Runtime/DefaultReadmes/Images/AccessingDebugMode_2.png)

# Internal vs. External
Internal readmes are stored entirely in the Unity object, and can be modified from within the inspector.
External readmes store a path to a text file, which holds a readme formatted like this one. The readmes loosely follow "Markdown" style.

# Images

Images can be dragged into the inspector in an internal readme, or referenced by a local path in an external readme.
Once cached, if the external readme remains unchanged, the path will stay updated.

![Alt](Assets/ScreenshotTest.png)