using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashGrabber : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -22f)
        {
            transform.position = new Vector3(25f, 1f, Random.Range(4f, -6f));
        }
    }

    private void OnCollisionEnter(Collision collision) //elephant hit the crate. reposition it to show up later
    {
        Debug.Log("sold an elephant");
    }
}
