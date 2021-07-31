# URP Clouds

This is a port to URP of the clouds created by [Sebastian Lague](https://github.com/SebLague)

[Watch video](https://www.youtube.com/watch?v=4QOcCGI6xOU)

![Clouds](https://i.imgur.com/3bXb0EB.jpg)

## Description

In the original project, the main injection point for the effect was in the CloudMaster.cs script, on the method OnRenderImage(). 
This is BRP specific for creating post processing effects. In URP, post processing effects are created by creating renderer features.

Unity has a great [example](https://github.com/Unity-Technologies/UniversalRenderingExamples) of this.
In the examples, there are some scripts called DrawFullscreenFeature and DrawFullscreenPass.
We can use that as a base for a custom pass in which to draw the clouds, all we need is a Material to draw on the screen.
Because the Cloud Master script sets up a new Material with the clouds shader and feeds it's parameters programatically, we need to do the same,
but in our custom pass. In order to do that, a new script needs to be added into the scene, that will take place of the old Cloud Master script.
All this script does is hold the data that Cloud Master held, and assign the parameters to the material through the rendering pass.
All other configurations will be done in the same way as the original project.

## Instructions

This is not a rework by any means, just a port. Most of the things are edited in the same way, but we need to make some extra steps to setup:
- Camera's Background is set to "Uninitialized" as a Default in URP, **change it to "Skybox"**
- Remove Cloud Master scripts, as they are no longer needed
- Add an URP Proxy script into the scene and set the BlueNoise Texture
- Create a new Material with the shader "FX/Clouds", no parameters need to be set in the materials, as it is being updated from the URP Proxy.
- Setup your URP Renderer's Rendering Path to Deferred ([Requires at least Unity 2021.2](https://portal.productboard.com/unity/1-unity-platform-rendering-visual-effects/c/10-deferred-renderer-support)).
- On your URP Renderer, create a new Clouds Feature
- Set the Material in the Clouds Feature to the previously created material.
- Tweak the Clouds URP Proxy, Noise Generator and Weather Map scripts in your scene.

## Tips

- Check the new flag "Update All Parameters In Realtime" under the Clouds URP Proxy to view changes in realtime in the editor. Once you're done,
uncheck it for a performance boost.
  
## Known Issues

Currently URP's rendering system draws the SkyBox after the Opaque geometry while in the Forward Rendering Path.
This has issues, as clouds would draw on top of the geometry, even if they are behind it. To fix this, use the Deferred Rendering Path ([Requires at least Unity 2021.2](https://portal.productboard.com/unity/1-unity-platform-rendering-visual-effects/c/10-deferred-renderer-support)).
If anyone has a fix for Forward rendering, please submit a Merge Request.