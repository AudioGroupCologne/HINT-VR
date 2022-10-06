using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircMovement : MonoBehaviour
{
    bool MovementOn = false;
    Vector3 center;
    float radius;
    float rotFreq = 0.2F;
    float posCnt = 0;

    // Update is called once per frame
    void Update()
    {
        if (MovementOn)
        {
            ObjectMovement(Time.deltaTime);
        }
    }

    public void ToggleMovement(bool enable)
    {
        MovementOn = enable;
    }

    public void SetMovementParameters(Vector3 _center, float _radius = 5, float _rotFreq = 0.2f)
    {
        radius = _radius;
        rotFreq = _rotFreq;
        center = _center;
    }

    void ObjectMovement(float dTime)
    {
        posCnt += dTime;
        float x = radius * Mathf.Cos(rotFreq * 2 * Mathf.PI * posCnt);
        float y = 2;
        float z = radius * Mathf.Sin(rotFreq * 2 * Mathf.PI * posCnt);
        transform.position = new Vector3(x, y, z) + center;
    }
}
