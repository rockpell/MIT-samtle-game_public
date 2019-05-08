using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClip _menuClick;
    [SerializeField] private AudioClip _menuClickNegative;
    [SerializeField] private AudioClip _doorSound;
    [SerializeField] private AudioClip _clapSound;
    [SerializeField] private AudioClip _laserSound;
    [SerializeField] private AudioClip _enemyLaserSound;
    [SerializeField] private AudioClip _deadSound;
    [SerializeField] private AudioClip _enemyDeadSound;

    private AudioSource _myAudio;

	// Use this for initialization
	void Start () {
        _myAudio = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MenuClickSound() {
        _myAudio.PlayOneShot(_menuClick);
    }

    public void MenuClickNegativeSound() {
        _myAudio.PlayOneShot(_menuClickNegative);
    }

    public void DoorSound()
    {
        _myAudio.PlayOneShot(_doorSound);
    }

    public void ClapSound()
    {
        _myAudio.PlayOneShot(_clapSound);
    }

    public void LaserSound()
    {
        _myAudio.PlayOneShot(_laserSound);
    }

    public void DeadSound()
    {
        _myAudio.PlayOneShot(_deadSound);
    }

    public void EnemyLaserSound()
    {
        _myAudio.PlayOneShot(_enemyLaserSound);
    }

    public void EnemyDeadSound()
    {
        _myAudio.PlayOneShot(_enemyDeadSound);
    }
}
