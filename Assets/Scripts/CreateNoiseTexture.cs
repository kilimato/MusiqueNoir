// This script consists of parts taken from official Unity documentation examples:
// https://docs.unity3d.com/ScriptReference/Mathf.PerlinNoise.html
// https://docs.unity3d.com/560/Documentation/ScriptReference/Texture2D.EncodeToPNG.html
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateNoiseTexture : MonoBehaviour
{
    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    // The origin of the sampled area in the plane.
    public float xOrg;
    public float yOrg;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    public Texture m_MainTexture, m_Normal, m_Metal;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
    }

    public void CalcNoise()
    {
        // For each pixel in the texture...
        float y = 0.0F;

        while (y < noiseTex.height)
        {
            float x = 0.0F;
            while (x < noiseTex.width)
            {
                float xCoord = xOrg + x / noiseTex.width * scale;
                float yCoord = yOrg + y / noiseTex.height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord);
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }
            y++;
        }

        // Copy the pixel data to the texture and load it into the GPU.
        noiseTex.SetPixels(pix);
        noiseTex.Apply();
    }

    public void MakePNG ()
    {
        // Encode texture into PNG
        byte[] bytes = noiseTex.EncodeToPNG();
        //Object.Destroy(noiseTex);

        // For testing purposes, also write to a file in the project folder
        File.WriteAllBytes(Application.dataPath + "/../SavedScreen.png", bytes);
    }

    public void CreateTexture()
    {
        CalcNoise();
        MakePNG();
    }
}
