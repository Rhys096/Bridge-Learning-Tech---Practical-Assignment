using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3 offset;
    Transform pt;
    void Start()
    {
        pt = GameObject.Find("PLAYER").transform;
        offset = transform.position - pt.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pt.position + offset;
    }
}
