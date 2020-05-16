using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotMul = 100f;
    [SerializeField] float thrustMul = 100f;

    Rigidbody rigidBody;
    AudioSource[] audioSources;
    AudioSource rocketSound, deathSound;
    ParticleSystem rocketThrust;


    bool thrusting;

    enum State { Alive, Dying, Transcending };
    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Alive;
        thrusting = false;

        rigidBody = GetComponent<Rigidbody>();
        rocketThrust = GetComponent<ParticleSystem>();
        audioSources = GetComponents<AudioSource>();
        rocketSound = audioSources[0];
        deathSound = audioSources[1];

    }

    // Update is called once per frame
    void Update()
    {
        if (!state.Equals(State.Dying)) // only move if alive
        {
            Thrust();
            Rotate();
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!state.Equals(State.Alive)) return; // exit if not alive, ignore collision

        switch (collision.gameObject.tag)
        {
            case "Dead":
                Die();
                
                break;

            case "Finish":
                Transcend();
                break;



        }
    }

    private void Transcend()
    {
        state = State.Transcending;
        Invoke("LoadNextLevel", 1f);
    }

    private void Die()
    {
        state = State.Dying;
        rigidBody.freezeRotation = false;
        rocketThrust.Stop();
        rocketSound.Stop();
        deathSound.Play();
        Invoke("LoadFirstLevel", 2f);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Rotate()
    {

        rigidBody.freezeRotation = true;
        float rotThisFrame = Time.deltaTime * rotMul;

        if (Input.GetKey(KeyCode.A)) // rotate left but cannot rotate right at the same time, so use if-elseif
        {
            /*rigidBody.AddRelativeTorque(Vector3.forward * 2);*/
            gameObject.transform.Rotate(Vector3.forward * rotThisFrame);



        }
        else if (Input.GetKey(KeyCode.D))
        {
            /*rigidBody.AddRelativeTorque(Vector3.back * 2);*/
            gameObject.transform.Rotate(Vector3.back * rotThisFrame);
        }
        rigidBody.freezeRotation = false;
    }

    private void Thrust()
    {
        float thrustThisFrame = Time.deltaTime * thrustMul;
        if (Input.GetKey(KeyCode.W)) // can thrust while rotating
        {

            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!thrusting) // if not already thrusting, play rocket sound and starting thrust particles
            {
                rocketThrust.Play();
                rocketSound.Play();
                thrusting = true;
            }


        }
        else
        {
            if (thrusting) // if previously thrusting but no longer thrusting, turn off rocket sound and stop thrust particles
            {
                rocketSound.Stop();
                rocketThrust.Stop();
                thrusting = false;
            }

        }
    }
}
