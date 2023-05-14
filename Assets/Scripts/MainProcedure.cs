using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


// TODO:
// ���ü�������������λ��������һ�����¼���ί�У��Լ���صĲ�����.

public delegate void ObjectPicked(GameObject sender, int step_number);
public delegate void StepFinished(GameObject sender, int step_number);

// TODO:
// ͸������ָʾ.

public class MainProcedure : MonoBehaviour
{
    public int currentStep = 0;
    private bool[] queue = new bool[9];  //1������Ƶ+8�������1-axis����), false��ʾδ���.
    public GameObject[] stepMainBody = new GameObject[9];
    public GameObject Cone;
    // ָʾ��ͷ
    public GameObject Arrow;
    public AudioSource SuccessSound;    //������Ч.
    public Transform axis_transform;
    private bool AllAccomplished = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i<queue.Length; ++i)   //��ʼ��queue.
        {
            queue[i] = false;
        }
        Cone = GameObject.Find("HintCone");
        //�ǵð�GameObjectȫ���ƹ���
        //ֱ�ӿ�ʼǿ����һ��.
        HighlightObject();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void HighlightObject()
    {
        //���ֻ������·.
        //�Ե�ǰ���������ǿ������.
        //�����߹⡢ͷ����Բ׶��ָʾ��ͷ.

        //ͷ���ĸ߹�Բ׶
        GameObject target = stepMainBody[currentStep];
        Cone.SetActive(true);
        Cone.transform.position = new Vector3(target.transform.position.x, Cone.transform.position.y, target.transform.position.z);
        Cone.GetComponent<HighlightPlus.HighlightTrigger>().Highlight(true);
        //������߹�.
        target.GetComponent<HighlightPlus.HighlightTrigger>().Highlight(true);
        //ָʾ��ͷ.
        Arrow.GetComponent<ArrowController>().setTargetPos(target.transform.position);
    }

    public void objectPicked(GameObject sender, int step_number)   //�������壬��ע��δ���ǵ�ǰ��������壬���û�������ʲôΪ׼.
    {
        //�õ��������ֹͣǿ��.�����û�δ�ذ�����ϣ����˳��.
        Cone.SetActive(false);
        sender.GetComponent<HighlightPlus.HighlightTrigger>().Highlight(false);
        currentStep = step_number;   //���û����ŵ�Ϊ׼.
        //ָ��������.
        Arrow.GetComponent<ArrowController>().setTargetPos(axis_transform.position);
    }

    public void stepFinished(GameObject sender, int step_number)   //�ɹ���װ��һ��
    {
        queue[step_number] = true;
        int i;
        for(i=1; i<queue.Length; ++i)  //ֱ������0������Ƶ�����û����ܲ�����Ƶֱ����ȥװ.
        {
            if (!queue[i])
                break;
        }
        if(i != queue.Length)
        {
            currentStep = i;
            HighlightObject();  //��ʼ��һ������.
        }
        else if(!AllAccomplished)
        {
            //�ɹ��ˣ����ųɹ���Ч�������û�֮��ᳶ������װ��ȥ������ֻ����һ����Ч.
            AllAccomplished = true;
            SuccessSound.PlayDelayed(2);
        }
    }
}
