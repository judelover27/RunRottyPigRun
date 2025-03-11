using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wine : Specialty
{
    public float duration = 10f;

    public override void SpecialUtil()
    {
        CharacterManager.Instance.StartCoroutine(WineSpecialUtil(duration));
    }

    public IEnumerator WineSpecialUtil(float wait)
    {
        float time = 0f;
        while (time < duration)
        {
            UIManager.Instance.condition.health.curValue = UIManager.Instance.condition.health.maxValue;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
