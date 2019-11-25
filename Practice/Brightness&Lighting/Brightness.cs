using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brightness : MonoBehaviour
{
    [SerializeField] private float rbgValue = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnGUI()
    {
        /*rbgValue = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 50, y: 90, width: 100, height: 30), 
            value: rbgValue, 
            leftValue: 0f, 
            rightValue: 1.0f);*/

        rbgValue = GUI.HorizontalSlider(new Rect(Screen.width / 2 - 50, y: 90, width: 100, height: 30), rbgValue, 0f, 5f);
        //RenderSettings.ambientLight = new Color(rbgValue, rbgValue, rbgValue, a: 1f);
        RenderSettings.ambientIntensity = rbgValue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
