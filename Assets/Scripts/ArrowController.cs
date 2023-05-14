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
        //��ʼtarget_pos���Ǹ���ť.
        player = GameObject.Find("Player");
        GameObject wall = GameObject.Find("HintCanvas/Wall");
        setTargetPos(wall.transform.position);
        //�򿪸߹�.
        GetComponent<HighlightPlus.HighlightTrigger>().Highlight(true);
    }

    // Update is called once per frame
    void Update()
    {
        //float roty = player.transform.rotation.eulerAngles.y;
        //Vector3 dir = new Vector3(0f, 0f, 0f);
        //dir.z = Mathf.Cos(roty);
        //dir.x = Mathf.Sin(roty);  //dir�����˳����ͶӰ����������.
        //Vector3 player_floor = player.transform.position;
        //player_floor.y = transform.position.y;  //y��ͬ��.
        //transform.position = player_floor;
        //transform.Translate(-dir, Space.World);  //������ŵ��˸�ǰ.��player��rotation�е�����.
        Vector3 dir = target_pos - player.transform.position;
        float dist = dir.x * dir.x + dir.y * dir.y + dir.z * dir.z;
        if (dist < 1.5)
        {
            //̫���ˣ�����.
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
        target_pos.y = transform.position.y;  //Ҫ�����������.y�᲻Ҫ��
    }
}
