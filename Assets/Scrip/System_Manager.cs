using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class System_Manager : MonoBehaviour
{
    public static System_Manager system;
    public Transform Player;

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
}
