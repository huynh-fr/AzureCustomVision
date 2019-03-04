# AzureCustomVision
NAIST project : Hololens Object Detection implementation

This project was based on the [MR and Azure 310](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310) course.
We take a picture of the real world, analyse it, tag objects with words and display the bounding box and name of the recognized object in 3D space.

## I.	Requirements

•	A development PC

•	Windows 10 Fall Creators Update (or later) with Developer mode enabled

•	The latest Windows 10 SDK

•	Unity 2017.4 LTS

•	Visual Studio 2017

•	A Microsoft HoloLens with Developer mode enabled

•	Internet access for Azure setup and Custom Vision Service retrieval


## II. Installation

## III. Getting started

The best thing to do would be to complete the [MR and Azure 310](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310) tutorial and then try this project. But you can skip this step and follow the next instructions if you want.

Download this project and open it in Unity.
Build the scene "CustomVisionScene" into a C# project (UWP app) and open it in Visual Studio as shown in the tutorial [Chapter 13](https://docs.microsoft.com/en-us/windows/mixed-reality/mr-azure-310#chapter-13---build-the-uwp-solution-and-sideload-your-application).
Run the App from VS (Device or Remote). If there is a bug, just try to run a 2nd time the App.
