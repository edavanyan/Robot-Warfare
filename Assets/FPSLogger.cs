using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSLogger : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI text;

    private int framesCount = 0;

    private float time = 0;

    private float fps = 0;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 120;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        framesCount++;
        if (time >= 1)
        {
            fps = framesCount;
            framesCount = 0;
            time = 0;
        }
    }

    private void OnGUI()
    {
        text.text = $"fps: {fps}";
    }
}
