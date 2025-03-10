using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Range(0, 1f)]
    public float time;
    public float fullDayLength;
    public float startTime = 0.4f;
    public float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectIntensityMultiplier;
    public AnimationCurve skyBoxExposureMultiplier;

    private void Start()
    {
        timeRate = 1f / fullDayLength;
        time = startTime;

    }

    private void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectIntensityMultiplier.Evaluate(time);
        RenderSettings.skybox.SetFloat("_Exposure", skyBoxExposureMultiplier.Evaluate(time));

        UpdateSunAndMoonSource();
    }

    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
            go.SetActive(false);
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
            go.SetActive(true);
    }

    void UpdateSunAndMoonSource()
    {
        bool isNight = time < 0.25f || time > 0.75f; // 밤 여부 판별

        if (isNight)
        {
            RenderSettings.sun = moon; // 밤에는 달을 Sun Source로 설정
        }
        else
        {
            RenderSettings.sun = sun; // 낮에는 태양을 Sun Source로 설정
        }
    }
}
