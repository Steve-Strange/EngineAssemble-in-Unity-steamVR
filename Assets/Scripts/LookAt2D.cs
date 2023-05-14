using UnityEngine;
using System.Collections;

public class LookAt2D : MonoBehaviour
{
    GameObject target;

    void Start()
    {
        target = GameObject.Find("Player");
    }

    void Update()
    {
        transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));
    }
}