using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveScript : MonoBehaviour
{

    private Vector3 myScale;
    private float myCameraSize;


    public float maxScale = 2f;
    public Camera connectedCamera;
    public float scaleMultiplier = 3f;
    public float cameraSizeMultiplier = 0.5f;

    void Start()
    {
        myScale = transform.localScale;
        myCameraSize = connectedCamera.orthographicSize;
    }


    void Update()
    {
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime * scaleMultiplier;

        connectedCamera.orthographicSize += Time.deltaTime * cameraSizeMultiplier;
        //connectedCamera.rect = new Rect(m_ViewPositionX, m_ViewPositionY, m_ViewWidth, m_ViewHeight);

        if (transform.localScale.x > maxScale)
        {
            transform.localScale = myScale;
            connectedCamera.orthographicSize = myCameraSize;
        }

    }
}