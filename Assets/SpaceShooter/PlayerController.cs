using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int speed = 10;
    [SerializeField] float aimSensitivity = 10;

    GameObject targetCursor;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject leftLaserPort;
    [SerializeField] GameObject rightLaserPort;

    Rigidbody rb;

    [SerializeField] GameObject level;

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
        ShootingControls();
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
        Vector3 levelRotation = new Vector3(0, 0, -horizontalInput);

        //level movement
        level.GetComponent<Rigidbody>().AddTorque(levelRotation * speed * Time.deltaTime, ForceMode.Force);


        //ship movement
        //float verticalInput = Input.GetAxisRaw("Vertical");

        //Vector3 movementDir = new Vector3 (horizontalInput, verticalInput, 0);
        //movementDir = movementDir.normalized;

        //rb.AddForce(movementDir * speed, ForceMode.Force);

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

        if (Physics.Raycast(ray, out hit, 1100, LayerMask.GetMask("Aim")))
        {
            targetCursor.transform.position = hit.point;
        }
    }

    void ShootingControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //instantiate lasers
            GameObject leftLaser = Instantiate(laserPrefab, leftLaserPort.transform.position, Quaternion.identity);
            GameObject rightLaser = Instantiate(laserPrefab, rightLaserPort.transform.position, Quaternion.identity);

            //get direction to cursor
            Vector3 leftLaserDir = (targetCursor.transform.position - leftLaser.transform.position).normalized;
            Vector3 rightLaserDir = (targetCursor.transform.position - rightLaser.transform.position).normalized;

            //initial orient of lasers
            leftLaser.transform.up = leftLaserDir;
            rightLaser.transform.up = rightLaserDir;

            //call laser fire method (adds force)
            leftLaser.GetComponent<LaserManager>().Fire(leftLaserDir);
            rightLaser.GetComponent<LaserManager>().Fire(rightLaserDir);
        }
    }
}