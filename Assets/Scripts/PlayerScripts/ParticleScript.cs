using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem collisionParticles;

    List<ParticleCollisionEvent> collisionEvents;
    public Gradient particleColorGradient;

    ParticleSystem.MainModule psMain;

    private float ringMinSize = 1.5f;
    public float ringSize = 1.5f;
    public float ringMaxSize = 5f;
    [SerializeField]
    private float ringStep = 0.5f;

    private bool isResonating = false;
    public CircleCollider2D particleCollider;

    public PlayerController playerController;

    /*
     Billboard particle systemin asetuksissa:
     default partikkeli ei ole pallo, vaikka näyttää siltä, vaan neliö, johon on piirretty ympyrä
     billboarding kääntää partikkelin aina kameraa kohti, jolloin näyttää että se olisi pyöreä
         */

    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
        particleCollider.radius = 0.0001f;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isVisible == false)
        {
            CancelInvoke();
            ringSize = 0.0001f;
        }

        CanPlayerResonate();

        psMain = particles.main;

        UpdateColliderSize();

        UpdateFadingCircle();
    }

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
            particleCollider.radius = ringSize / 8;
        }
        else
        {
            particleCollider.radius = 0.0001f;
        }
    }


    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents(particles, other, collisionEvents);
        for (int i = 0; i < collisionEvents.Count; i++)
        {
            Debug.Log("Collision events: " + collisionEvents[i].colliderComponent.gameObject.ToString());
        }
    }

    private void EmitAtLocation(ParticleCollisionEvent collisionEvent)
    {
        // no need to change emit location here in our implementation (if particlesystem is always child of parent)
        // 2 lines below move particles to emit to where the collision took place, and particles emit towards where the collision came from
        // i.e. if we shoot a wall, the particles forming from the impact go where the shot was fired
        //collisionParticles.transform.position = collisionEvent.intersection;
        //collisionParticles.transform.rotation = Quaternion.LookRotation(collisionEvent.normal);
        collisionParticles.Emit(1);
    }


    // when player holds space, waves are created, when space is released, waves stop
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
}