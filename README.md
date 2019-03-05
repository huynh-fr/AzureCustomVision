# AzureCustomVision
NAIST project : Hololens Object Detection implementation

This project was based on the [MR and Azure 310](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310) course.
We take a picture of the real world, analyse it, tag objects with words and display the bounding box and name of the recognized object in 3D space.


## I.	Requirements

•	A development PC

•	Windows 10 Fall Creators Update (or later) with Developer mode enabled

•	The latest Windows 10 SDK

•	[Unity 2017.4 LTS](https://unity3d.com/unity/qa/lts-releases?version=2017.4&page=3)

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

### 1) Custom Vision Portal

First, complete [Chapter 1](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-1---the-custom-vision-portal) and 2 of the tutorial.

### 2) Unity

Download this project and extract it at your PC's root (C:\Hololens\). Open Assets\Scenes\CustomVisionScene.unity in Unity.
Check that your project is set like in [Chapter 3](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-3---set-up-the-unity-project). Also, for step 7.b. check under Capabilities : InternetClient, WebCam, Microphone, SpatialPerception.
Paste the information you copied in Chapter 2 in the CustomVisionAnalyser and CustomVisionTrainer classes specific variables.

NOTE : you can also create your project from scratch using the Chapter 3 and then import this project AzureCustomVision.unitypackage like in Chapter 4.

Build the scene "CustomVisionScene" into a C# project (UWP app | Hololens) as shown in the tutorial [Chapter 13](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-13---build-the-uwp-solution-and-sideload-your-application).

### 3) Visual Studio

Open and run the solution in Visual Studio from the folder you just created (Release | x86 | Device or Remote). If there is a bug, just try to run a 2nd time the App.
For the first time, you will be asked the Hololens PIN. With your Hololens : Menu > Settings > Updates & Security > For developers > Pair 

NOTE : for the next Unity Builds, you can just select the folder you just created. The files should be re-written. However, if there is a bug, delete that folder and Build in Unity like you did the first time.
