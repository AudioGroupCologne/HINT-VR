# unity_vraudio

This project was created to translate auditory test and training procedures into Virtual Reality (VR) using Unity as development platform. Currently VR peripherals are integrated using the Oculus SDK and audio spatialization is handled by the 3D Tune-In Toolkit (3DTI)
Unity wrapper using a custom set of HRTFs created using a Neumann KU100 artificial head.


Built in Unity 2020.3.32f1


## Current state
A port of the German version of the Hearing in Noise Test (HINT / German HINT) is included in this project (HINT-VR). The implementation has been tested and verified through the conduction of a pilot study, which showed no significant difference in Spatial Release from Masking between
the HINT-VR and a loudspeaker-based reference setup.

##  Features

- UserSystem
- Data export through JSON files
- Keyboard and mouse controls for testing/debugging
- Multiple feedback systems (traditional and autonomous)
- Adjustable test parameters through usage of the Inspector interface
- Different scenes for Login, MainMenu, Test and Demo/Calibration

