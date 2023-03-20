# HINT-VR
HINT-VR is a child-appropriate application for the self-assessment of spatial hearing abilities in VR. The application is described in [1].
The VR peripherals are integrated using the Oculus SDK and audio spatialization is handled using the Unity wrapper [2] for 3D Tune-In Toolkit (3DTI) [3] with far-field
measured non-individual HRTFs (Neumann KU100 dummy head) [4]. 

In the current state, the Hearing in Nosie Test (HINT) [5] is implemented using the original stimuli from the German hearing in noise test featuring a male speaker [6]. 

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
[1] Müller, A., Ramírez, M., Arend, J. M., Rader, T., & Pörschmann, C. (2023). HINT-VR: A child-appropriate application for the self-assessment of spatial hearing abilities in VR. Proceedings of the 49th DAGA.

[2] Reyes-Lecuona, A. & Picinali, L. Unity Wrapper for 3DTI. https://github.com/3DTune-In/3dti_AudioToolkit_UnityWrapper (2022).

[3] Cuevas-Rodríguez, M. et al. 3D Tune-In Toolkit: An open-source library for real-time binaural spatialisation. PLoS ONE 14, (2019).

[4] Bernschütz, B. A Spherical Far Field HRIR HRTF Compilation of the Neumann KU 100. in Proceedings of the 39th DAGA 592–595 (2013)

[5] Nilsson, M., Soli, S. D. & Sullivan, J. A. Development of the Hearing In Noise Test for the measurement of speech reception thresholds in quiet and in noise. The Journal of the Acoustical Society of America 95, 1085–1099 (1994).

[6] Joiko, J., Bohnert, A., Strieth, S., Soli, S. D. & Rader, T. The German hearing in noise test. International Journal of Audiology 60, 927–933 (2021).
