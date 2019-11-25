using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAdjuster : MonoBehaviour
{
    public Light MLight;

    // Range Variables
    public bool BChangeRange = false;
    public float RangeSpeed = 1.0f;
    public float MaxRange = 10.0f;
    public bool BRepeatRange = false;

    // Intensity Variables
    public bool BChangeIntensity = false;
    public float IntensitySpeed = 1.0f;
    public float MaxIntensity = 10.0f;
    public bool BRepeatIntensity = false;

    // Color Variables
    public bool BChangeColors = false;
    public float ColorSpeed = 1.0f;
    public Color StartColor;
    public Color EndColor;
    public bool BRepeatColor = false;

    [SerializeField] private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        MLight = GetComponent<Light>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (BChangeRange)
        {
            if(BRepeatRange)
            {
                MLight.range = Mathf.PingPong(Time.time * RangeSpeed, MaxRange);
            }
            else
            {
                MLight.range = Time.time * RangeSpeed;
                if(MLight.range >= MaxRange)
                {
                    BChangeRange = false;
                }
            }
            
        }
        if (BChangeIntensity)
        {
            if(BRepeatIntensity)
            {
                MLight.intensity = Mathf.PingPong(Time.time * IntensitySpeed, MaxIntensity);
            }
            else
            {
                MLight.intensity = Time.time * IntensitySpeed;
                if(MLight.intensity >= MaxIntensity)
                {
                    BChangeIntensity = false;
                }
            }
            
        }
        if(BChangeColors)
        {
            if (BRepeatColor)
            {
                float t = (Mathf.Sin(Time.time - startTime * ColorSpeed));
                MLight.color = Color.Lerp(StartColor, EndColor, t);
            }
            else
            {
                float t = Time.time - startTime * ColorSpeed;
                MLight.color = Color.Lerp(StartColor, EndColor, t);
            }
            
            
        }
    }
}
