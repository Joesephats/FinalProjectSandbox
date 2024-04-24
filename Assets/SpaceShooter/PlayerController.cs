using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int speed = 10;
    [SerializeField] float aimSensitivity = 10;

    GameObject targetCursor;
    
    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetCursor = GameObject.FindGameObjectWithTag("Target");
    }

    // Update is called once per frame
    void Update()
    {
        FlyingControls();
        //KeyAimingControls();
        MouseAimingControls();
    }

    private void FixedUpdate()
    {
        
    }

    void ReturnPlayerToBounds()
    {
        /*
        Transform playerTransform = gameObject.transform;

        if (Mathf.Abs(playerTransform.position.x) > 100)
        {
            if (Mathf.Abs(gameObject.transform.position.x) < -100)
            {
                playerTransform.position.x = new Vector3 (-100, gameObject.transform.position.y, gameObject.transform.position.z);
            }
        } */
    }

    void FlyingControls()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");

        Vector3 movementDir = new Vector3 (horizontalInput, verticalInput, 0);
        movementDir = movementDir.normalized;

        rb.AddForce(movementDir * speed, ForceMode.Force);

    }

    void KeyAimingControls()
    {
        Rigidbody targetRb = targetCursor.GetComponent<Rigidbody>();

        float horizontalInput = Input.GetAxisRaw("HorizontalArrow");
        float verticalInput = Input.GetAxisRaw("VerticalArrow");
        Debug.Log(horizontalInput + " / " + verticalInput);


        Vector3 aimDir = new Vector3(horizontalInput, verticalInput, 0);
        aimDir = aimDir.normalized;

        targetRb.AddForce(aimDir * aimSensitivity, ForceMode.Acceleration);
    }

    void MouseAimingControls()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000, LayerMask.GetMask("Aim")))
        {
            targetCursor.transform.position = hit.point;
        }
    }
}
