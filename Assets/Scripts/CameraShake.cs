using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    float velocity = 0f;
    float tension = 0.05f;
    float dampening = 0.05f;
    float targetDisplacement = 0f;
    float displacement = 0f;

    Vector3 initial;

    // Start is called before the first frame update
    void Start()
    {
        initial = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //displacement += 3f;
        }

        float delta = targetDisplacement - displacement;
        velocity += (tension * delta) - (dampening * velocity);
        displacement += velocity;

        transform.localPosition = initial + new Vector3(0, displacement, 0);

    }

    public void AddDisplacement(float f)
    {
        displacement += f;
    }
}
