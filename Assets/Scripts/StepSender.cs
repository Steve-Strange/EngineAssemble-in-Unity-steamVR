using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class StepSender : MonoBehaviour
{
    // Start is called before the first frame update
    public int step_number;
    private ObjectPicked objpicked;
    private StepFinished stpfinished;
    //���屻����������ɵ��¼�.����֪ͨ���̿�����
    public event ObjectPicked wasPicked
    {
        add
        {
            objpicked += value;
        }
        remove
        {
            objpicked -= value;
        }
    }
    //����װ��λ�õ��¼�������֪ͨ���̿�����
    public event StepFinished wasFinished
    {
        add
        {
            stpfinished += value;
        }
        remove
        {
            stpfinished -= value;
        }
    }

    void Start()
    {
        GameObject procedure = GameObject.Find("Procedure");
        MainProcedure mainProcedure = procedure.GetComponent<MainProcedure>();
        wasPicked += mainProcedure.objectPicked;
        wasFinished += mainProcedure.stepFinished;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void sendPicked()
    {
        //���ڸ�steamVR��OnPicked event���������ľ��Ǵ����Լ����źţ��෢��һ��.
        objpicked(this.gameObject, step_number);   //��ί�д�����������event...
    }

    public void sendFinished()
    {
        stpfinished(this.gameObject, step_number);
    }
}
