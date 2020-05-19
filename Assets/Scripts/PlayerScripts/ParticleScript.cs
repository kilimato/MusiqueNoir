// @author Eeva Tolonen
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// handles player's resonator device: emits a ring around the player and a trigger collider that corresponds to ring size
public class ParticleScript : MonoBehaviour
{
    public ParticleSystem particles;
    ParticleSystem.MainModule psMain;

    public float ringMinSize = 1.5f;
    public float ringSize = 1.5f;
    public float ringMaxSize = 5f;
    [SerializeField]
    private float ringStep = 0.5f;

    private bool isResonating = false;
    private int collidersize = 6;

    public CircleCollider2D particleCollider;
    public PlayerController playerController;

    public Canvas mainCanvas;
    public Canvas pauseCanvas;

    // Start is called before the first frame update
    void Start()
    {
        particleCollider.radius = 0.0001f;
    }

    // Update is called once per frame
    void Update()
    {
        psMain = particles.main;

        if (mainCanvas.enabled || pauseCanvas.enabled)
        {
            if (Input.anyKey)
            {
                return;
            }
        }

        if (playerController.isVisible == false)
        {
            CancelInvoke();
            ringSize = 0.0001f;
        }

        CanPlayerResonate();

        UpdateColliderSize();

        UpdateFadingCircle();
    }

    // cancels emitting of ring particles if under minsize
    void UpdateFadingCircle()
    {
        if (ringSize <= ringMinSize && IsInvoking("EmitFadingParticles"))
        {
            CancelInvoke("EmitFadingParticles");
        }
    }

    void UpdateColliderSize()
    {
        if (isResonating || IsInvoking("EmitFadingParticles") && playerController.isVisible)
        {
            particleCollider.radius = ringSize / collidersize;
        }
        else
        {
            particleCollider.radius = 0.0001f;
        }
    }


    // when player holds space, particle rings are emitted, when space is released, emitting shrinks and stops
    private void CanPlayerResonate()
    {
        if (!playerController.isVisible) return;

        while (Input.GetKey(KeyCode.Space) && !IsInvoking("EmitParticles"))
        {
            if (IsInvoking("EmitFadingParticles"))
            {
                CancelInvoke("EmitFadingParticles");
            }
            isResonating = true;
            InvokeRepeating("EmitParticles", 0, 0.4f);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("EmitParticles");
            InvokeRepeating("EmitFadingParticles", 0, 0.4f);
            isResonating = false;
        }
    }

    private void EmitParticles()
    {
        if (ringSize <= ringMinSize)
        {
            ringSize = ringMinSize;
        }

        if (ringSize >= ringMaxSize)
        {
            ringSize = ringMaxSize;
        }
        else
        {
            ringSize += ringStep;
        }

        psMain.startSize = ringSize;
        particles.Emit(1);
    }

    private void EmitFadingParticles()
    {
        if (ringSize >= ringMinSize)
        {
            ringSize -= ringStep * 2;
        }
        psMain.startSize = ringSize;
        particles.Emit(1);
    }

    public bool GetPlayerResonating()
    {
        return isResonating;
    }

    // resets resonator for saving purposes
    public void ResetResonator()
    {
        CancelInvoke();
        isResonating = false;
        particleCollider.radius = 0.0001f;
        ringSize = 0.0001f;
    }
}