# Windows 10 IoT Core Dashboard
The Windows 10 IoT Core Dashboard is the Universal Windows Platform (UWP) version of the web based management dashboard that's built into Windows 10 IoT Core.

## Windows IoT Core Version Requirements
**The current release requires Windows 10 IoT Core build version 10.0.10531.**  There have been a number of changes to the web API that powers the web based management dashboard that's built into Windows 10 IoT Core.

## Cross-Platform Testing
This project has only been tested on a desktop, a Surface 3 Pro, a Raspberry PI 2 B, and a Windows 10 Mobile emulator.  It doesn't appear to run very well on the emulator yet and due to a bug in the current release of Windows 10 IoT Core no dialogs can be displayed using the Windows.UI.Popups.MessageDialog class so functionality is significantly reduced.

## Current Features
* Displays general Windows IoT Core device information (model, name, OS version, etc.).
* Displays general device performance metrics (CPU usage, total, in use, and available memory, network in/out).
* Ability to reboot and shutdown the device.
* Displays installed and running applications.
* Displays available devices (i.e. device manager enumeration).
* Displays available networking configurations (IP configurations, Wifi adapters, and available Wifi networks).
* Displays running processes with the ability to terminate any process.
* Run any command.

## Near Future Features
* Start and uninstall apps.
* Bluetooth paired and available devices.
* Change device name and password.

## Features Not Currently Planned
* Install new App packages.
* Performance tracing.
* Realtime ETW Tracing.
* Kernel and process dumps.
* Windows Error Report tracking.

The built-in web based management dashboard can be used directly to accomplish these tasks.
