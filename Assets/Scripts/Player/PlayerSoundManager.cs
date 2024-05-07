using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundManager : MonoBehaviour
{
    [SerializeField] AudioClip impactSFX;
    [SerializeField] AudioClip laserSFX;

    public void PlayImpactSFX()
    {
        AudioSource.PlayClipAtPoint(impactSFX, transform.position);
    }

    public void PlayLaserSFX()
    {
        AudioSource.PlayClipAtPoint(laserSFX, transform.position);
    }
}
