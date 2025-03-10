using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaCondition : Condition
{


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        SetRecovering();
    }


    protected void SetRecovering()
    {
        if (curValue <= 0)
        {
            recovering = true;
        }

        if (recovering)
        {
            if (curValue > recoveringValue)
            {
                recovering = false;
            }
        }
    }
}
