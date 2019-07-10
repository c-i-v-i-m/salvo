# How?

## Including Salvo in your Unity project
- Open a new or an existing Unity project.
- Copy the Salvo directory into your project's Assets directory.
- Get the "OctaneRender" plugin: https://unity3d.com/partners/otoy/octanerender.
	- (Import only the plugin. Ignore example scenes)
	
- Add the "OctaneBaker" prefab to your scene.
- Select the OctaneBaker prefab and click the "Setup" button. This will setup the project structure by creating the "Baked Images", "Resources/Materials", and "Resources/Textures" directories in addition to creating a PBR Render Target game object.

- Select the "PBR Render Target", click "Load Octane".

## Creating targets
- Tag every game object for which you want a baked texture by giving it the "PBRInstanceProperties" script component.
- Select PBR Render Target, click "Render"
	- (No need to let the render finish. At this point, the objective is simply to instantiate the PBRInstanceProperties components).
- Select the OctaneBaker instance in your scene, click "Generate PBR Render Targets".


## Rendering
- Select PBR Render Target
- Click "Render"
- Click the gear icon to "Open Octane Interface"
- In the Octane interface, select Script > Batch Rendering
	- Set output folder to the project's "Assets/Baked Images" directory.
	- Set the pattern to "%n.%e".
	- Disable "Save all enabled passes" and "Save denoised main pass if available".

## Creating materials
- Select the OctaneBaker instance in your scene.
- Click "Create materials"


## Applying materials
- Select the OctaneBaker instance in your scene.
- Click "Apply materials"


## Toggle materials
- Select the OctaneBaker instance in your scene.
- Click "Toggle materials"
