using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ShaderTest : MonoBehaviour
{
    public RenderTexture renderTexture;

    public ComputeShader shader;

    public RawImage rawImage;
   
    // Start is called before the first frame update
    void Start()
    {
        if (renderTexture == null)
        {
           //renderTexture = new RenderTexture(256, 256, 24);
            renderTexture = new RenderTexture(Screen.width,Screen.height, 24);
            renderTexture.enableRandomWrite = true;
            renderTexture.filterMode = FilterMode.Point;
            renderTexture.Create();

        }

        shader.SetTexture(0, "Result", renderTexture);
        shader.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
        rawImage.texture = renderTexture;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
