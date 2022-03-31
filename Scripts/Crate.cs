using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject elephantSpawn;
    public ParticleSystem smokeParticles;

    private float nextElephantTimer;
    
    // Update is called once per frame
    void Update()
    {
        nextElephantTimer -= Time.deltaTime;

        if (nextElephantTimer < 0f)
        {
            nextElephantTimer = 0f;
        }

        if(transform.position.x < -40f)
        {
            GetComponent<MovingObject>().CanMove = false;
            transform.position = new Vector3(30f, 1f, 0f);
            FindObjectOfType<GameController>().GenerateNextObstacle(false);
        }

        /*
        if(FindObjectOfType<GameController>().ControlledElephants.Count > 16) //at maximum, move crate too high to be touched
        {
            transform.position = new Vector3(transform.position.x, 1f, 20f);
        }
        else if (transform.position.z == 20f) //return crate when population is low enough
        {
            transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        if (nextElephantTimer == 0f)
        {
            FindObjectOfType<AudioManager>().Play("crate");

            nextElephantTimer = 0.1f;

            Instantiate(elephantSpawn, transform.position, Quaternion.identity);

            var newParticles = Instantiate(smokeParticles.gameObject, transform.position, Quaternion.identity);

            newParticles.GetComponent<ParticleSystem>().Play();

            Destroy(newParticles, 1f);

            transform.position = new Vector3(30f, 1f, 0f);

            FindObjectOfType<GameController>().GenerateNextObstacle(false);
        }
    }
}
