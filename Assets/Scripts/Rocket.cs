using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rotMul = 100f;
    [SerializeField] float thrustMul = 100f;
    [SerializeField] AudioClip rocketSound, deathSound, winSound;
    [SerializeField] ParticleSystem rocketParticles, winParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;


    bool thrusting;

    enum State { Alive, Dying, Transcending };
    State state;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Alive;
        thrusting = false;

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();


    }

    // Update is called once per frame
    void Update()
    {
        if (state.Equals(State.Alive)) // only move if alive
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
        audioSource.Stop();
        rocketParticles.Stop();
        audioSource.PlayOneShot(winSound);
        winParticles.Play();
        Invoke("LoadNextLevel", 2f);
    }

    private void Die()
    {
        state = State.Dying;
        rigidBody.freezeRotation = false;
        rocketParticles.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(deathSound);
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
                rocketParticles.Play();
                audioSource.PlayOneShot(rocketSound);
                thrusting = true;
            }

        }
        else
        {
            if (thrusting) // if previously thrusting but no longer thrusting, turn off rocket sound and stop thrust particles
            {
                audioSource.Stop();
                rocketParticles.Stop();
                thrusting = false;
            }

        }
    }
}
