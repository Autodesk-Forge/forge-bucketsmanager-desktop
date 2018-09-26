# bucket.manager-csharp-sample.tool

![Platforms](https://img.shields.io/badge/platform-Windows-lightgray.svg)
![.NET](https://img.shields.io/badge/.NET-4.6-blue.svg)
[![CefSharp](http://img.shields.io/badge/CefSharp-57.0.0-blue.svg)](http://opensource.org/licenses/MIT)
[![License](http://img.shields.io/:license-mit-blue.svg)](http://opensource.org/licenses/MIT)

[![Data-Management](https://img.shields.io/badge/Data%20Management-v2-green.svg)](http://developer.autodesk.com/)
[![Model-Derivative](https://img.shields.io/badge/Model%20Derivative-v2-green.svg)](http://developer.autodesk.com/)
[![Viewer](https://img.shields.io/badge/Viewer-v6-green.svg)](http://developer.autodesk.com/)

![Advanced](https://img.shields.io/badge/Level-Advanced-red.svg)


# Demonstration

This sample is a Windows desktop application that lists all buckets for a given Forge Client ID & Secret, allow creating new buckets, upload new files, translate and delete. Allow download of SVF for offline viewing. Simple JavaScript for testing code. It is intended as tool for developers.

## Thumbnail


![thumbnail](/thumbnail.png)

# Setup

## Prerequisites

1. **Visual Studio**: Either Community (Windows) or Code (Windows, MacOS).
2. **CefSharp**: Chromium based browser control for .NET apps
3. **.NET Framework** basic knowledge with C#

## Running locally

For using this sample, you need an Autodesk developer credentials. Visit the [Forge Developer Portal](https://developer.autodesk.com), sign up for an account, then [create an app](https://developer.autodesk.com/myapps/create).

Download the repository, open `bucket.manager.sln` Solution on Visual Studio. The build process should download the required packages (**Autodesk.Forge** and dependencies). Run the project. At the form, enter your Client ID & Secret, click on **Authenticate** button. The app will obtain a 2-legged token and list buckets and files. After translating, files should be Viewable.

# Features

This sample app includes a few features:

## Download SVF

After creating a bucket and uploading an object, translate the file. When finish, open on the Viewer. Click on **Download SVF** button and select a destination folder. The HTML to view the file needs to point to the .svf file, usually a **0.SVF** under a folder with the same name of the viewable.

## Running JavaScript code

The CefSharp control allow **.EvaluateScriptAsync()** for executing JavaScript code, which can be used for quick testing some code. Load a model on the Viewer, then click on **JavaScript** button. Type, paste or open a .js file, then click on **Run** (or `Ctrl+R`) to run, the result will show on the bottom text area and (if applicable) at the DevTools Console). The video demonstrate it:

![JavaScript run demo](js_run.gif)

# Further Reading

Blog:

- [Download SVG](https://forge.autodesk.com/blog/forge-svf-extractor-c-net): details on how to download the SVF for offline viewing.

Samples:

- [Desktop 3-legged sample](https://github.com/Autodesk-Forge/data.management-csharp-desktop.sample)

### Tips & Tricks

The CEF Sharp library should work on `AnyCPU`, but this sample uses only `x64` version. [This issue](https://github.com/cefsharp/CefSharp/issues/1714) entry explains how to adjust it, if needed.

## License

This sample is licensed under the terms of the [MIT License](http://opensource.org/licenses/MIT). Please see the [LICENSE](LICENSE) file for full details.

## Written by

Augusto Goncalves [@augustomaia](https://twitter.com/augustomaia), [Forge Partner Development](http://forge.autodesk.com)