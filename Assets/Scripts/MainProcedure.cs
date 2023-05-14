using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


// TODO:
// 设置捡起零件和零件到位、进入下一步的事件和委托，以及相关的参数类.

public delegate void ObjectPicked(GameObject sender, int step_number);
public delegate void StepFinished(GameObject sender, int step_number);

// TODO:
// 透明部件指示.

public class MainProcedure : MonoBehaviour
{
    public int currentStep = 0;
    private bool[] queue = new bool[9];  //1个看视频+8个零件（1-axis不算), false表示未完成.
    public GameObject[] stepMainBody = new GameObject[9];
    public GameObject Cone;
    // 指示箭头
    public GameObject Arrow;
    public AudioSource SuccessSound;    //结束音效.
    public Transform axis_transform;
    private bool AllAccomplished = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<queue.Length; ++i)   //初始化queue.
        {
            queue[i] = false;
        }
        Cone = GameObject.Find("HintCone");
        //记得把GameObject全部移过来
        //直接开始强调第一步.
        HighlightObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void HighlightObject()
    {
        //这个只用来带路.
        //对当前步骤的物体强调处理.
        //包括高光、头顶的圆锥、指示箭头.

        //头顶的高光圆锥
        GameObject target = stepMainBody[currentStep];
        Cone.SetActive(true);
        Cone.transform.position = new Vector3(target.transform.position.x, Cone.transform.position.y, target.transform.position.z);
        Cone.GetComponent<HighlightPlus.HighlightTrigger>().Highlight(true);
        //给物体高光.
        target.GetComponent<HighlightPlus.HighlightTrigger>().Highlight(true);
        //指示箭头.
        Arrow.GetComponent<ArrowController>().setTargetPos(target.transform.position);
    }

    public void objectPicked(GameObject sender, int step_number)   //捡到了物体，但注意未必是当前步骤的物体，以用户拿起了什么为准.
    {
        //拿到了物体后，停止强调.但是用户未必按照你希望的顺序.
        Cone.SetActive(false);
        sender.GetComponent<HighlightPlus.HighlightTrigger>().Highlight(false);
        currentStep = step_number;   //以用户拿着的为准.
        //指向中心轴.
        Arrow.GetComponent<ArrowController>().setTargetPos(axis_transform.position);
    }

    public void stepFinished(GameObject sender, int step_number)   //成功安装了一个
    {
        queue[step_number] = true;
        int i;
        for(i=1; i<queue.Length; ++i)  //直接跳过0（放视频），用户可能不看视频直接跑去装.
        {
            if (!queue[i])
                break;
        }
        if(i != queue.Length)
        {
            currentStep = i;
            HighlightObject();  //开始下一步引导.
        }
        else if(!AllAccomplished)
        {
            //成功了，播放成功音效。可能用户之后会扯下来再装回去，但是只播放一遍音效.
            AllAccomplished = true;
            SuccessSound.PlayDelayed(2);
        }
    }
}
