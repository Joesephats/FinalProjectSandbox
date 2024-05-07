using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{

    int laserDamage;

    Rigidbody rb;
    Vector3 myDir;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 10f);
    }

    private void Update()
    {
        transform.up = rb.velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            Destroy(gameObject, .5f);
        }
    }

    public void Fire(Vector3 dir)
    {
        rb.AddForce(dir * 500, ForceMode.Impulse);
    }

    public void SetDamage(int damage)
    {
        laserDamage = damage;
    }

    public int GetLaserDamage()
    {
        return laserDamage;
    }
}
