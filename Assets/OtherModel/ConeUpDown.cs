using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeUpDown : MonoBehaviour
{
    // Start is called before the first frame update
    public float A=0.5f, omega=1f, ori_y=3.5f;  //Asin(omega*t);
    private float startTime;
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        float t = Time.time - startTime;
        float off = A * Mathf.Sin(omega * t);
        Vector3 pos = transform.position;
        pos.y = ori_y + off;
        transform.position = pos;
    }
}
