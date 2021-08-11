using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
public class HMDInfoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {       
        if (!XRSettings.isDeviceActive)
        {
            Debug.Log("No headset plugged in");
        }
        else if(XRSettings.isDeviceActive && XRSettings.loadedDeviceName == "MockHMD Display")
        {
            Debug.Log("Using Mock HMD");
            // use this case to enable mouse & keyboard controls of the VR application (especially for camera operation)
        }
        else
        {
            Debug.Log("XR Device Name is: " + XRSettings.loadedDeviceName);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
