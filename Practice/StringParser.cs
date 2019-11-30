using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StringParser : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private string testString;

    private void Awake()
    {
        testString = prefab.name;
    }
    // Start is called before the first frame update
    void Start()
    {
        testString.Contains("_Weight");
        
        var testString2 = testString.Substring(testString.IndexOf("_Weight") + 7);
        // int testInt = Int32.Parse(testString2);
        int testInt = 0;

        Int32.TryParse(testString2, out testInt);
        Debug.Log("parsing result is " + testString2 + " and " + testInt);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
