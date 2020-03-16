using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    public ParticleSystem particles;
    public ParticleSystem collisionParticles;

    List<ParticleCollisionEvent> collisionEvents;
    public Gradient particleColorGradient;

    //public ParticleDecalPool particlePool;

    ParticleSystem.SizeOverLifetimeModule psSizeOverLifetime;
    ParticleSystem.MainModule psMain;

    //public ParticleSystemCurveMode mode;
    /*
     MIHIN JÄIN:
     Miten saa rinkulan kokoa muutettua koodista sen lifetimen aikana? Curve? Miten päästä käsiksi, oikea moduuli on jo löydetty,
     vrt. miten mainModule toimii updatesta käsin jos se auttaisi
         */

    public float ringSize = 0.5f;
    public float ringMaxSize = 5f;
    private float ringStep = 0.5f;
    /*
     Nytt och nyttigt:
     Billboard particle systemin asetuksissa:
     default partikkeli ei ole pallo, vaikka näyttää siltä, vaan neliö, johon on piirretty ympyrä
     billboarding kääntää partikkelin aina kameraa kohti, jolloin näyttää että se olisi pyöreä

     jäit kohtaan 10/11 tutoriaalissa
     Our implementation:
     values we need to modify:
     startlife -> area grows -> make it dependant on how long key is pressed, min and max startlives
     particlePool -> needed to get collisions with objects


         */

    // Start is called before the first frame update
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        CanPlayerResonate();
        psMain = particles.main;
        //psMain.startColor = particleColorGradient.Evaluate(Random.Range(0f, 1f));

        psSizeOverLifetime = particles.sizeOverLifetime;

        // tekee noin kahden vakion väliltä randomilla, miten saa curven?
        //psSizeOverLifetime.size = new ParticleSystem.MinMaxCurve(2f,5f);
    }

    
    /*private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        ParticlePhysicsExtensions.GetCollisionEvents(particles, other, collisionEvents);

        for (int i = 0; i < collisionEvents.Count; i++)
        {
            Debug.Log("Collision events: " + collisionEvents[i].colliderComponent.gameObject.ToString());
            // when collision happens, we take every collision event and emit particle with wanted values to collision location
            particlePool.ParticleHit(collisionEvents[i], particleColorGradient);

            EmitAtLocation(collisionEvents[i]);
            Debug.Log("Will emit at location: " + collisionEvents[i].colliderComponent.transform.position.ToString());
        }
        /*
        if(!IsInvoking("EmitAtLocation"))
        { 
            InvokeRepeating("EmitAtLocation", 0, 1);
        }
        
    }*/

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
            InvokeRepeating("EmitParticles", 0, 0.5f);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("EmitParticles");
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
