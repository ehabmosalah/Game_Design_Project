using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class System_Manager : MonoBehaviour
{
    public static System_Manager system;
    public Transform Player;
    public AudioClip[] Sounds;

    public int score;
    public int level_Items;

    void Awake()
    {
        system = this;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(AudioClip sound, Vector3 ownerPos)
    {
        GameObject obj = SoundFXPoder.current.GetPooledObject();
        AudioSource audio = obj.GetComponent<AudioSource>();
        obj.transform.position = ownerPos;
        obj.SetActive(true);
        audio.PlayOneShot(sound);
        StartCoroutine(DisableSound(audio));
    }

    IEnumerator DisableSound(AudioSource audio)
    {
        while (audio.isPlaying)
            yield return new WaitForSeconds(0.5f);
        audio.gameObject.SetActive(false);
    }
}
