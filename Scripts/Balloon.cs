using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    private GameController MainControl;
    private Rigidbody body;
    private ParticleSystem popParticles;

    public SpriteRenderer ColoredPiece;

    public float SpeedMult;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        MainControl = FindObjectOfType<GameController>();
        popParticles = GetComponentInChildren<ParticleSystem>();

        Pop(false);

        SpeedMult = Random.Range(0.825f, 0.925f);
    }

    // Update is called once per frame
    void Update()
    {
        if(MainControl.PauseTime <= 0f)
        {
            body.velocity = Vector2.left * SpeedMult * MainControl.GameSpeed;

            if (transform.position.x < -30f)
            {
                Pop(false);
            }
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }

    void Pop(bool WasTouched)
    {
        int ColorRand = Random.Range(0, 5);

        if(ColorRand == 0)
        {
            ColoredPiece.color = ColoredPiece.color = new Color32(255, 45, 70, 255); //red
        }
        else if(ColorRand == 1)
        {
            ColoredPiece.color = new Color32(200, 100, 255, 255); //purple
        }
        else if (ColorRand == 2)
        {
            ColoredPiece.color = new Color32(100, 255, 100, 255); //green
        }
        else if (ColorRand == 3)
        {
            ColoredPiece.color = new Color32(255, 255, 40, 255); //yellow
        }
        else
        {
            ColoredPiece.color = new Color32(120, 255, 255, 255); //cyan
        }

        if (WasTouched) //make noise and add score
        {
            FindObjectOfType<AudioManager>().Play("balloonPop");

            var newParticles = Instantiate(popParticles.gameObject, transform.position, Quaternion.identity);

            newParticles.GetComponent<ParticleSystem>().Play();

            Destroy(newParticles, 1f);

            FindObjectOfType<MainUI>().IncreaseScore(10f);

            SpeedMult = Random.Range(0.825f, 0.925f);
        }

        transform.position = new Vector3(Random.Range(60f, 120f), 1f, Random.Range(-5f, 4f));
    }

    private void OnTriggerEnter(Collider other)
    {
        Pop(true);
    }
}
