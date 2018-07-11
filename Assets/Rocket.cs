
using UnityEngine;
using UnityEngine.SceneManagement;


public class Rocket : MonoBehaviour {

    Rigidbody rigidBody;
    AudioSource audioSource;
    enum State { Alive, Dying, Transcending }
    State state = State.Alive;

    [SerializeField] float rcsThrust = 200f;
    [SerializeField] float linearThrust = 200f;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        Thrust();
        Rotate();
    }

    void OnCollisionEnter(Collision collision)
    {
       switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                Invoke("LoadNextScene", 1f);
                break;

            default:
                print("dead");
                SceneManager.LoadScene(0);
                break;
        }
            
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(1); //todo allow for more than 2 levels
    }

    private void Thrust()
    {
        //float thrustThisFrame = linearThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.Space)) //can thrust while rotating
        {
            rigidBody.AddRelativeForce(Vector3.up * linearThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void Rotate()
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
