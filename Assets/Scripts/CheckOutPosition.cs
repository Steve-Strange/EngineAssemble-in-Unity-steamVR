using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckOutPosition : MonoBehaviour
{
    // Start is called before the first frame update
    public float border, forward, backward, left, right;
    public GameObject player;
    void Start()
    {
        player = GameObject.Find("Player/SteamVRObjects/BodyCollider");
        right = 13.5f;
        left = -13.5f;
        forward = 11.5f;
        backward = -13.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.position.x > right || this.transform.position.x < left 
            || this.transform.position.z > forward || this.transform.position.z < backward)
        {
            this.transform.rotation = player.transform.rotation;
            this.transform.position = player.transform.position;
            transform.Translate(new Vector3(0, 0, 1), Space.Self);
        }
    }
}
