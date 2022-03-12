using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameLimiter : MonoBehaviour
{
    public int MaxFrames = 20;

    private void OnEnable()
    {
        Application.targetFrameRate = MaxFrames;
    }
}
