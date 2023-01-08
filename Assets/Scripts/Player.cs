using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float maxSpeed = 5.0f;
    Transform camTransform;
    Rigidbody rb;
    

    // Start is called before the first frame update
    void Start()
    {
        camTransform = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float vert = Input.GetAxis("Vertical");
        float hor = Input.GetAxis("Horizontal");

        Vector3 motion = camTransform.forward * vert + camTransform.right * hor;
        motion.y = 0;

        Vector3 curVel = rb.velocity;
        curVel.y = 0;

        float delta = maxSpeed - curVel.magnitude;

        if (Mathf.Abs(vert) > 0 || Mathf.Abs(hor) > 0)
        {
            rb.AddForce(motion.normalized * delta, ForceMode.Force);
        }

        //transform.Rotate()

        // cc.Move(-Vector3.up * Time.deltaTime);

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position + Vector3.up * 2.005f, Vector3.up, 0.5f))
        {
            GameObject.Find("GAME").GetComponent<GameSetup>().EndGame();
        }
    }
}
