using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Goblet : Specialty
{
    public float duration = 10f;
    public float multiplier = 5f;

    public override void SpecialUtil()
    {
        CharacterManager.Instance.StartCoroutine(GobletSpecialUtil(duration, multiplier));
    }

    public IEnumerator GobletSpecialUtil(float wait, float multiplier)
    {
        float origin = CharacterManager.Instance.Player.controller.jumpPower;
        CharacterManager.Instance.Player.controller.jumpPower = origin * multiplier;
        yield return new WaitForSeconds(wait);
        CharacterManager.Instance.Player.controller.jumpPower = origin;
    }

}
