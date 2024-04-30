using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserManager : MonoBehaviour
{

    Rigidbody rb;
    Vector3 myDir;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.up = rb.velocity;
    }

    public void Fire(Vector3 dir)
    {
        rb.AddForce(dir * 500, ForceMode.Impulse);
    }
}
