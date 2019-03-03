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

## III. Getting started

The best thing to do would be to complete the [MR and Azure 310](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310) tutorial and then try this project. But you can skip this step and follow the next instructions if you want.

Download this project and extract it at your PC's root (C:\Hololens\). Open it in Unity.
[HoloToolkit](https://github.com/Microsoft/MixedRealityToolkit-Unity/releases/tag/2017.4.3.0) and [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126) assets seem to have too many sub-folders, so you will need to import them. Delete in the HoloToolkit folder the files that are doubled in the project.
Build the scene "CustomVisionScene" into a C# project (UWP app | Hololens) and open it in Visual Studio as shown in the tutorial [Chapter 13](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-13---build-the-uwp-solution-and-sideload-your-application).
Run the App from VS (Release | x86 | Device or Remote). If there is a bug, just try to run a 2nd time the App.
