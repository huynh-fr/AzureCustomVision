# AzureCustomVision
NAIST project : Hololens Object Detection implementation for Hololens v1

This project was based on the [MR and Azure 310](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310) course.
We take a picture of the real world, analyse it, tag objects with words and display the bounding box and name of the recognized object in 3D space.

## I.	Requirements

•	A development PC

•	Windows 10 Fall Creators Update (or later) with Developer mode enabled

•	The latest Windows 10 SDK

•	[Unity 2017.4 LTS](https://unity3d.com/get-unity/download/archive)

•	[Visual Studio 2017](https://visualstudio.microsoft.com/downloads/)

•	A Microsoft HoloLens with Developer mode enabled

•	Internet access for Azure setup and Custom Vision Service retrieval


## II. Installation

Enable Developer mode in Windows > Settings > For developers.

Install Visual Studio 2017 : check .NET for Desktop and Windows Universal Platform dev.
Log in with an outlook account.

Install Unity 2017.4.1f1 with the link in I.Requirements.
Check Windows Backend for .NET and IL2CPP options.
Log in with a Unity account or create one with Google.

## III. Getting started

The best thing to do would be to complete the [MR and Azure 310](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310) tutorial and then try this project. But you can skip this step and follow the next instructions if you want.

First, complete [Chapter 1](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-1---the-custom-vision-portal), 2 and 3 of the tutorial. For [Chapter 3](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-3---set-up-the-unity-project), step 7.b. check under Capabilities : InternetClient, WebCam, Microphone, SpatialPerception.

Download this project and extract it at your PC's root (C:\Hololens\). Open it in Unity.
[HoloToolkit](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.3.0) and [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126) assets seem to have too many sub-folders, so you will need to import them. Delete in the HoloToolkit folder the files that are doubled in the project.

Import AzureCustomVision.unitypackage like it is done in [Chapter 4](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-4---importing-the-customvisionobjdetection-unity-package). You paste the information you copied in Chapter 2 in the CustomVisionAnalyser and CustomVisionTrainer classes specific variables.

Build the scene "CustomVisionScene" into a C# project (UWP app | Hololens) and open it in Visual Studio as shown in the tutorial [Chapter 13](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-13---build-the-uwp-solution-and-sideload-your-application).

Run the App from VS (Release | x86 | Device or Remote). If there is a bug, just try to run a 2nd time the App.
For the first time, you will be asked the Hololens PIN. With your Hololens : Menu > Settings > Updates & Security > For developers > Pair 
