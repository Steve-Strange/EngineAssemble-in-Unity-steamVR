using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Vector3 target_pos;
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        //初始target_pos是那个按钮.
        player = GameObject.Find("Player");
        GameObject wall = GameObject.Find("HintCanvas/Wall");
        setTargetPos(wall.transform.position);
        //打开高光.
        GetComponent<HighlightPlus.HighlightTrigger>().Highlight(true);
    }

    // Update is called once per frame
    void Update()
    {
        //float roty = player.transform.rotation.eulerAngles.y;
        //Vector3 dir = new Vector3(0f, 0f, 0f);
        //dir.z = Mathf.Cos(roty);
        //dir.x = Mathf.Sin(roty);  //dir就是人朝向的投影的世界坐标.
        //Vector3 player_floor = player.transform.position;
        //player_floor.y = transform.position.y;  //y轴同步.
        //transform.position = player_floor;
        //transform.Translate(-dir, Space.World);  //这是想放到人跟前.用player的rotation有点问题.
        Vector3 dir = target_pos - player.transform.position;
        float dist = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (dist < 1.5)
        {
            //太近了，隐藏.
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
            GetComponent<MeshRenderer>().enabled = true;
        dir = dir / dist;
        transform.position = player.transform.position + dir;
        transform.LookAt(target_pos);
    }

    public void setTargetPos(Vector3 tarpos)
    {
        target_pos = tarpos;
        target_pos.y = transform.position.y;  //要看向这个即可.y轴不要变
    }
}
