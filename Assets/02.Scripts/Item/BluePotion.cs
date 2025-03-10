using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BluePotion : Specialty
{
    public float duration = 20f;
    public float multiplier = 3f;

    public override void SpecialUtil()
    {
        CharacterManager.Instance.StartCoroutine(BluPotionSpecialUtil(duration, multiplier));
    }

    public IEnumerator BluPotionSpecialUtil(float wait, float multiplier)
    {
        Vector3 origin = CharacterManager.Instance.Player.transform.localScale;
        CharacterManager.Instance.Player.transform.localScale = origin / multiplier;
        yield return new WaitForSeconds(wait);
        CharacterManager.Instance.Player.transform.localScale = origin;
    }

}