﻿
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State {Alive, Dying, Transcending}
    State state = State.Alive;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float linearThrust = 200f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip deathTransition;
    [SerializeField] AudioClip levelTransition;
    [SerializeField] float levelLoadDelay = 2f;

    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathTransitionParticles;
    [SerializeField] ParticleSystem levelTransitionParticles;
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        if(state == State.Alive)
        { 
        RespondToThrustInput();
        RespondToRotateInput();
            
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive)
        {
            return;
        }

       switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;

            default:
                StartDeathSequence();

                break;
        }
            
    }

    private void StartDeathSequence()
    {
        state = State.Dying;
        audioSource.Stop();
        audioSource.PlayOneShot(deathTransition);
        deathTransitionParticles.Play();
        Invoke("LoadFirstLevel", levelLoadDelay);
    }

    private void StartSuccessSequence()
    {
        state = State.Transcending;
        audioSource.Stop();
        audioSource.PlayOneShot(levelTransition);
        levelTransitionParticles.Play();
        Invoke("LoadNextLevel", levelLoadDelay); //parameterise time
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1); //todo allow for more than 2 levels
    }

    private void RespondToThrustInput()
    {
        //float thrustThisFrame = linearThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(Vector3.up * linearThrust * Time.deltaTime);
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
        mainEngineParticles.Play();
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;  //manual control of ration

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A))
        {
            
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }

        rigidBody.freezeRotation = false;  //resume physics control of rotation

    }

   
}
