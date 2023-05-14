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
    //定义被捡起来和完成的事件.用于通知流程控制器
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
    //定义装到位置的事件，用于通知流程控制器
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
        //用于给steamVR的OnPicked event触发，做的就是触发自己的信号，多发了一层.
        objpicked(this.gameObject, step_number);   //用委托触发，而非用event...
    }

    public void sendFinished()
    {
        stpfinished(this.gameObject, step_number);
    }
}
