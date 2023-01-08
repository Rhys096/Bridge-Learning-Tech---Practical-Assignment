using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : MonoBehaviour
{
    // Start is called before the first frame update

    float rotationY = 0;
    float rotationSpeed = 20.0f;
    public bool sphere = false;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, rotationY, 45);
        rotationY += Time.deltaTime * rotationSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.name == "PLAYER")
        {
            GameObject.Find("GAME").GetComponent<GameSetup>().ScorePickupTouched(sphere, GetComponent<Renderer>().material.color, true, transform.position);
        }
        else
        {
            GameObject.Find("GAME").GetComponent<GameSetup>().ScorePickupTouched(sphere, GetComponent<Renderer>().material.color, false, transform.position);
        }
        Destroy(gameObject);
    }

}
