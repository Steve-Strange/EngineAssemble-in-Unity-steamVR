using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{

    private bool activated = false;
    public GameObject part;
    GameObject oriPart;
    void Start()
    {
        oriPart = GameObject.Find("Turbofan/" + part.name);
    }

    void Update()
    {
        if (activated)
        {
            part.transform.position = oriPart.transform.position - new Vector3(0, 1.1f, 2.58f);
            part.transform.rotation = oriPart.transform.rotation;
            part.GetComponent<Rigidbody>().useGravity = false;
            part.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }
        else
        {
            part.GetComponent<Rigidbody>().useGravity = true;
            part.GetComponent<Rigidbody>().constraints = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        activated = true;
    }

    private void OnTriggerExit(Collider other)
    {
        activated = false;
    }
}
