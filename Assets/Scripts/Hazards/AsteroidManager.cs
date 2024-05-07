using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    [SerializeField] int hp = 1;
    [SerializeField] bool indestructable = false;

    [SerializeField] GameObject explosion;
    [SerializeField] AudioClip explosionSFX;
    AudioSource myAudioSource;

    Vector3 impact;

    // Start is called before the first frame update
    void Start()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hp < 1)
        {
            DestroyAsteroid(impact);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Asteroid Collision");
        if (other.gameObject.tag == "Player")
        {
            other.GetComponentInParent<PlayerController>().DamagePlayer(1);
            impact = other.ClosestPoint(transform.position);
            hp -= 1;
            //DestroyAsteroid(other.ClosestPoint(transform.position));
        }

        if (other.gameObject.tag == "Laser")
        {
            if (!indestructable)
            {
                impact = other.ClosestPoint(transform.position);
                hp -= other.GetComponent<LaserManager>().GetLaserDamage();
                //DestroyAsteroid(other.ClosestPoint(transform.position));
            }
        }

        if (other.gameObject.tag == "DeathWall")
        {
            Destroy(gameObject);
        }
    }

    void DestroyAsteroid(Vector3 impactLocation)
    {
        Instantiate(explosion, impactLocation, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSFX, impactLocation);
        Destroy(gameObject);
    }
}
