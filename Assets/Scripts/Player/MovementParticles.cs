using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementParticles : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] ParticleSystem particles;

    //run particle variables
    [SerializeField] private float timerMax = 4;
    [SerializeField] private float timerMin = 2;
    [SerializeField] private float timer = 3;
    private bool firstTimeRun = true;
    //jump particle variables
    private bool firstTimeJump = true;
    [SerializeField] int newParticleMax = 30;
    public void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        if (player.isRunning && !player.isJumping)
        {
            RunParticles();

        }
        else if (player.isJumping) 
        {
            JumpParticles();
        }
        else
        {
            firstTimeRun = true;
        }
        if (player.isGrounded && firstTimeJump != true) 
        {
            firstTimeJump = true;
            CreateDust();
        }
    }
    void RunParticles() 
    {
        if (firstTimeRun) 
        {
            particles.Play();
            firstTimeRun = false;
            timer = Random.Range(timerMin, timerMax);

        }
        //set the right sprite to spawn

        if (timer < 0) 
        {
            CreateDust();
            timer = Random.Range(timerMin, timerMax);
        }
        //get a random time between two presets
        timer -= Time.deltaTime;
        //after that set time trigger a batch spawn. 
            //reset the timer
    }
    void JumpParticles() 
    {
        if (firstTimeJump) 
        {
            CreateDust();
            firstTimeJump = false;
        }
    }
    void CreateDust() 
    {
        particles.Play();
    }
}
