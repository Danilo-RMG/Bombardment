using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float DelayE = 5f;
    public GameObject Explosion;
    public GameObject WoodBreak;
    public float BlastRadius = 5f;
    public int BlastDamage = 10;
    void Start()
    {
        StartCoroutine(Explode());
    }
   private IEnumerator Explode()
    {
     yield return new WaitForSeconds(DelayE);
     BombExplode();
    }
    private void BombExplode()
     {
      Instantiate(Explosion, transform.position, Explosion.transform.rotation);
       Collider[] colliders = Physics.OverlapSphere(transform.position, BlastRadius);
        foreach(Collider collider in colliders)
         {
          GameObject hitObject = collider.gameObject;
           if(hitObject.CompareTag("Platform"))
            {
             Life lifeScript = hitObject.GetComponent<Life>();
              if(lifeScript != null)
               {
                float distance = (hitObject.transform.position - transform.position).magnitude;
                float distanceRate = Mathf.Clamp(distance / BlastRadius, 0, 1);
                float damageRate = 1f - Mathf.Pow(distanceRate, 4);
                int damage = (int) Mathf.Ceil(damageRate * BlastDamage);
                 lifeScript.Health -= damage;
                  if(lifeScript.Health <= 0)
                   {
                    Instantiate(WoodBreak, hitObject.transform.position, WoodBreak.transform.rotation);
                    Destroy(hitObject);
                   }
               }
            }
         }
      Destroy(gameObject);
     }
}
