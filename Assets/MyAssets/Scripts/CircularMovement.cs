using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    float timeCnt = 0;
    float dTime = 0;
    float radius;
    float radChange = 10;
    float rot_freq = 0.2F;  // 1 rotation every 5 seconds   
    float radVar;
    float radVarSign = 1;
    float posCnt = 0;

    bool MovementOn = true;
    bool RadialChangeOn = false;

    // Start is called before the first frame update
    void Start()
    {
        radius = 5;
        // cos is 1 @ 0, height is fixed, sin is 0 @ 0 (Initial position)
        transform.position = new Vector3(radius, 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // keep Track of time
        dTime = Time.deltaTime;
        timeCnt += dTime;


        // check for UserActions
        UserActions();

        if (MovementOn)
        {
            ObjectMovement(dTime);
        }      
    }

    void UserActions()
    {

        // Start/Stop movement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (MovementOn)
                MovementOn = false;
            else
                MovementOn = true;

        }

        // decrease Movement speed
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(rot_freq > 0.05F)
            {
                rot_freq -= 0.05F;
                Debug.Log("Rot Freq Dec: " + rot_freq);
            }
            
        }

        // increase Movement speed
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (rot_freq < 5.0F)
            {
                rot_freq += 0.05F;
                Debug.Log("Rot Freq Inc: " + rot_freq);
            }
        }

        // enable/disable change of radius
        if (Input.GetKey(KeyCode.R))
        {
            if (RadialChangeOn)
                RadialChangeOn = false;
            else
                RadialChangeOn = true;
        }

        if (Input.GetKey(KeyCode.Plus))
        {
            if(radChange < 200)
                radChange += 10F;
        }

        if (Input.GetKey(KeyCode.Minus))
        {
            if (radChange > 10)
                radChange -= 10F;
        }

    }

    void ObjectMovement(float dTime)
    {

        posCnt += dTime;

        // delta time is interval in seconds since last frame
        // determine rotational speed in Hz!
        if (RadialChangeOn)
        {
            radVar = radVarSign * radChange * dTime;

            if (radius > 100.0F || radius < 2.0F)
            {
                radVarSign *= -1;
            }
            radius += radVar;
        }


        float x = radius * Mathf.Cos(rot_freq * 2 * Mathf.PI * posCnt);
        float y = 2;
        float z = radius * Mathf.Sin(rot_freq * 2 * Mathf.PI * posCnt);
        transform.position = new Vector3(x, y, z);

    }


}
