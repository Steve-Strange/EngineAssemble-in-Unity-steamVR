using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour
{
    GameObject target;

    void Start()
    {
        target = GameObject.Find("Player");
    }

    void Update()
    {
        transform.LookAt(target.transform);
    }
}