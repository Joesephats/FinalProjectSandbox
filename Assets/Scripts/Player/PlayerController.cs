using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] int speed = 10;
    [SerializeField] int maxHp = 3;
    [SerializeField] int laserDamage;
    int hp;
    int darkMatter = 0;
    [SerializeField] float aimSensitivity = 10;

    GameObject targetCursor;

    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject leftLaserPort;
    [SerializeField] GameObject rightLaserPort;

    Rigidbody rb;

    float rollVel = 0;
    float pitchVel = 0;
    [SerializeField] float rollTime = 0.5f;
    [SerializeField] float pitchTime = 0.5f;

    float horizontalInput;
    float verticalInput;
    Vector3 movementDirection;

    [SerializeField] GameObject[] heartsUI;
    int heartsUICounter = 2;
    [SerializeField] TMP_Text darkMatterCounter;


    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("GameOver").SetActive(false);

        rb = GetComponent<Rigidbody>();
        targetCursor = GameObject.FindGameObjectWithTag("Target");
        hp = maxHp;

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        movementDirection = GetFlyingInput();
        //KeyAimingControls();
        MouseAimingControls();
        ShootingControls();
        LevelSkip();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Asteroid")
        {
            GetComponent<PlayerSoundManager>().PlayImpactSFX();
        }
        else if (other.gameObject.tag == "DarkMatter")
        {
            Destroy(other.gameObject);
            darkMatter++;
            darkMatterCounter.text = $"{darkMatter}/10";
        }
        else if (other.gameObject.tag == "WinWall")
        {
            GameManager.Instance.AddDarkMatter(darkMatter);
            GameManager.Instance.NextLevel();
        }
    }

    Vector3 GetFlyingInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        return new Vector3(horizontalInput, verticalInput, 0).normalized;
    }

    void MovePlayer()
    {
        //move player
        rb.AddForce(movementDirection * speed, ForceMode.Force);
        
        //control roll and pitch
        float rollAngle = -25 * horizontalInput;
        float pitchAngle = -15 * verticalInput;

        rollAngle = Mathf.SmoothDampAngle(transform.eulerAngles.z, rollAngle, ref rollVel, rollTime);
        pitchAngle = Mathf.SmoothDampAngle(transform.eulerAngles.x, pitchAngle, ref pitchVel, pitchTime);

        transform.localEulerAngles = new Vector3(pitchAngle, 0, rollAngle);
        
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

        if (Physics.Raycast(ray, out hit, 1100, LayerMask.GetMask("Aim", "Shootable")))
        {
            targetCursor.transform.position = hit.point;
        }
    }

    void ShootingControls()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<PlayerSoundManager>().PlayLaserSFX();

            //instantiate lasers
            GameObject leftLaser = Instantiate(laserPrefab, leftLaserPort.transform.position, Quaternion.identity);
            GameObject rightLaser = Instantiate(laserPrefab, rightLaserPort.transform.position, Quaternion.identity);

            //get direction to cursor
            Vector3 leftLaserDir = (targetCursor.transform.position - leftLaser.transform.position).normalized;
            Vector3 rightLaserDir = (targetCursor.transform.position - rightLaser.transform.position).normalized;

            //initial orient of lasers
            leftLaser.transform.up = leftLaserDir;
            rightLaser.transform.up = rightLaserDir;

            //set laser damage
            leftLaser.GetComponent<LaserManager>().SetDamage(laserDamage);
            rightLaser.GetComponent<LaserManager>().SetDamage(laserDamage);

            //call laser fire method (adds force)
            leftLaser.GetComponent<LaserManager>().Fire(leftLaserDir);
            rightLaser.GetComponent<LaserManager>().Fire(rightLaserDir);
        }
    }

    public void DamagePlayer(int damage)
    {
        hp -= damage;

        heartsUI[heartsUICounter].SetActive(false);
        heartsUICounter--;

        if (hp < 1)
        {
            KillPlayer();
        }

    }

    void KillPlayer()
    {
        GameManager.Instance.SetGameOver();
        Debug.Log("Kill Player");
    }

    public int GetLaserDamage()
    {
        return laserDamage;
    }

    void LevelSkip()
    {
        if (Input.GetKeyDown(KeyCode.N)) { GameManager.Instance.NextLevel(); }
    }
}