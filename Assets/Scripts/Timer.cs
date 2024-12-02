using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Timer : MonoBehaviour
{
    [SerializeField] Text timer;
    float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.realtimeSinceStartup;
    }

    // Update is called once per frame
    void Update()
    {
        float elapsedTime = Time.realtimeSinceStartup - startTime;

        timer.text = $"{elapsedTime:0.000}";
    }
}
