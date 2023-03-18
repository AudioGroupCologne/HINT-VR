# HINT-VR
This project was created to translate auditory test and training procedures into Virtual Reality (VR) using Unity as development platform. Currently VR peripherals are integrated using the Oculus SDK and audio spatialization is handled by the 3D Tune-In Toolkit (3DTI) [1].
Unity wrapper using a custom set of HRTFs created using a Neumann KU100 artificial head [2].

The project is realized in different scenes, fulfilling dedicated functionalities such as offering a login screen, a main menu or the actual test environment. The code-base has been designed following a modular approach. For example, there is a dedicated component in place that manages the virtual audio mixer to change playback levels of target and distractor sources and offers the required API to trigger these changes from a higher level component. Through this method there a very few cross-dependencies between scripts and components, which offers more freedom in changing certain aspects of the program.

Simple tweaks, like the altering of test parameters can be done using the Unity Inspector interface. Currently the default values stored in the project are the ones used during the aforementioned pilot study.

In the current state, the German version of the Hearing in Nosie Test (HINT) [3] is implemented using the original speech stimuli [4]. The implementation has been tested and verified through the conduction of a pilot study, which showed no significant difference in Spatial Release from Masking between
the HINT-VR and a loudspeaker-based reference setup.

## Requirements
- Unity 2020.3.32f1
- Oculus XR Plugin 1.11.2
- OpenXR Plugin 1.3.1
- JSON .NET For Unity 2.0.1

##  Features
- UserSystem
- Data export through JSON files
- Keyboard and mouse controls for testing/debugging
- Multiple feedback systems (traditional and autonomous)
- Adjustable test parameters through usage of the Inspector interface
- Different scenes for Login, MainMenu, Test and Demo/Calibration

## About
Alexander Müller and Christoph Pörschmann
TH Köln - University of Applied Sciences
Institute of Communications Engineering
Department of Acoustics and Audio Signal Processing
Betzdorfer Str. 2, D-50679 Cologne, Germany
https://www.th-koeln.de/akustik

Alexander Müller is now at ImagineOn GmbH
Neusserstr. 27-92, 50670 Cologne, Germany
https://imagineon.de/

## References
[1] Cuevas-Rodríguez, M. et al. 3D Tune-In Toolkit: An open-source library for real-time binaural spatialisation. PLoS ONE 14, (2019).
[2] Bernschütz, B. A Spherical Far Field HRIR HRTF Compilation of the Neumann KU 100. in Proceedings of the 39th DAGA 592–595 (2013)
[3] Nilsson, M., Soli, S. D. & Sullivan, J. A. Development of the Hearing In Noise Test for the measurement of speech reception thresholds in quiet and in noise. The Journal of the Acoustical Society of America 95, 1085–1099 (1994).
[4] Joiko, J., Bohnert, A., Strieth, S., Soli, S. D. & Rader, T. The German hearing in noise test. International Journal of Audiology 60, 927–933 (2021).
