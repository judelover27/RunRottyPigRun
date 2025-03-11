using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BluePotion : Specialty
{
    public float duration = 20f;
    public float lerpTime = 3f;
    public float multiplier = 3f;

    public override void SpecialUtil()
    {
        CharacterManager.Instance.StartCoroutine(BluPotionSpecialUtil(duration, multiplier));
    }

    public IEnumerator BluPotionSpecialUtil(float wait, float multiplier)
    {
        Vector3 origin = CharacterManager.Instance.Player.transform.localScale;
        Vector3 target = origin / multiplier;

        float time = 0f;
        while (time < duration)
        {
            CharacterManager.Instance.Player.transform.localScale = Vector3.Lerp(origin, target, (duration - time) > lerpTime ? Mathf.Min(time/lerpTime, 1) : (duration - time)/ lerpTime);
            time += Time.deltaTime;
            yield return null;
        }

        CharacterManager.Instance.Player.transform.localScale = origin;
    }

}