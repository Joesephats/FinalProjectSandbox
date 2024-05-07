using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLevelSequence : MonoBehaviour
{
    [SerializeField] float levelSpeed = 1;

    Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(Vector3.back * (levelSpeed * 500), ForceMode.VelocityChange);
    }

    // Update is called once per frame
    void Update()
    {
        //rb.AddForce(Vector3.back * levelSpeed * Time.deltaTime);
    }

    public void StopLevel()
    {
        rb.velocity = Vector3.zero;
    }
}
