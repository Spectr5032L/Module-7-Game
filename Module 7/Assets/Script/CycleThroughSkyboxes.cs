using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughSkyboxes : MonoBehaviour
{
    public Material SkyboxHandPaintedBlue;
    public Material SkyboxHandPaintedPurple;
    public Material SkyboxHandPaintedrRed;

    private Material[] materials = new Material[3];
    private int pos = 2;
    void Start()
    {
        materials[0] = SkyboxHandPaintedBlue;
        materials[1] = SkyboxHandPaintedPurple;
        materials[2] = SkyboxHandPaintedrRed;

    }
    public void CycleSkybox()
    {
        RenderSettings.skybox = materials[pos++];
        pos %= 3;
    }
}
