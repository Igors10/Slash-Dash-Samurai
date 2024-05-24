using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathParticleScript : MonoBehaviour
{
    public float particleLifeTime;
   
    ParticleSystem deathParticalSystem;
    // Start is called before the first frame update
    void Start()
    {
        
        
       // StartCoroutine(ParticleLifeSpan());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ParticleDirection(Transform target)
    {
        transform.LookAt(target);
        
        transform.rotation = Quaternion.Inverse(transform.rotation);

        

        
    }

   //// IEnumerator ParticleLifeSpan()
   // {
   //    // yield return new WaitForSeconds(particleLifeTime);

   //     //deathParticalSystem.Stop();

   //     //Destroy(gameObject);
   // }

}
