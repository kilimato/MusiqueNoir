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

    public float ringSize = 0.5f;
    public float ringMaxSize = 5f;
    [SerializeField]
    private float ringStep = 0.5f;

    private bool isResonating = false;
    public CircleCollider2D particleCollider;

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
        CanPlayerResonate();
        psMain = particles.main;

        UpdateColliderSize();
    }

    void UpdateColliderSize()
    {
        if (isResonating)
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
        while (Input.GetKey(KeyCode.Space) && !IsInvoking("EmitParticles"))
        {
            isResonating = true;
            InvokeRepeating("EmitParticles", 0, 0.5f);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("EmitParticles");
            isResonating = false;
            ringSize = 0;
        }
    }


    private void EmitParticles()
    {
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
}
