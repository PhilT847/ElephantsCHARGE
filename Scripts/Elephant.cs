using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elephant : MonoBehaviour
{
    public SpriteRenderer skinRenderer;
    public SpriteRenderer tuskRenderer;
    public SpriteRenderer hatRenderer;
    public SpriteRenderer eyeRenderer;
    public ParticleSystem dustParticles;
    public ParticleSystem bubbleParticles;
    public float reward;

    private Vector3 velocityRef;
    private Animator bodyAnim;
    private Slider followSlider;

    public int orderInLine;
    public float SpeedMult;

    private GameController control;


    // Start is called before the first frame update
    void Start()
    {
        bodyAnim = GetComponentInChildren<Animator>();
        control = FindObjectOfType<GameController>();

        SpawnElephant();
    }

    // Update is called once per frame
    void Update()
    {
        if (control.PauseTime <= 0f)
        {
            Walk();
        }
        else if (bodyAnim.GetBool("Walking"))
        {
            bodyAnim.SetBool("Walking", false);
            dustParticles.Stop();
        }
    }

    void Walk() //move between z=2 and z=-4; min X is -7
    {
        Vector3 movePosition = Vector3.zero;

        float SlowFactor = 1f;

        if (!bodyAnim.GetBool("Walking"))
        {
            bodyAnim.SetBool("Walking", true);
            dustParticles.Play();
        }

        if (orderInLine == 0)
        {
            movePosition = new Vector3(-3.5f, 1f, followSlider.value);
        }
        else if (orderInLine % 2 == 0) //even numbers are lower, odd are higher
        {
            if (orderInLine > 9)
            {
                movePosition = new Vector3(4.4f - orderInLine, 1f, followSlider.value - (orderInLine * 0.2f));
                SlowFactor = orderInLine - 8;
            }
            else
            {
                movePosition = new Vector3(-4f - orderInLine, 1f, followSlider.value - (orderInLine * .24f));
                SlowFactor = orderInLine;
            }
        }
        else
        {
            if (orderInLine > 8)
            {
                movePosition = new Vector3(3.5f - orderInLine, 1f, followSlider.value + (orderInLine * 0.22f));
                SlowFactor = orderInLine - 8;
            }
            else
            {
                movePosition = new Vector3(-5.2f - orderInLine, 1f, followSlider.value + (orderInLine * .3f));
                SlowFactor = orderInLine;
            }
        }

        transform.position = Vector3.SmoothDamp(transform.position, movePosition, ref velocityRef, (0.1f + (SlowFactor * 0.08f)) * SpeedMult);

        SetSpriteLayers();

        FindObjectOfType<MainUI>().IncreaseScore(reward * Time.deltaTime);

        //transform.position = Vector3.MoveTowards(transform.position, movePosition, Time.deltaTime);
    }

    void SetSpriteLayers()
    {
        //SpriteRenderer[] layeredSprites = GetComponentsInChildren<SpriteRenderer>();

        int AddOrder = (int)(transform.position.z * -1000f);

        skinRenderer.sortingOrder = AddOrder;
        eyeRenderer.sortingOrder = AddOrder + 1;
        hatRenderer.sortingOrder = AddOrder + 1;
        tuskRenderer.sortingOrder = AddOrder + 1;

        /*
        for (int i = 0; i < layeredSprites.Length; i++)
        {
            layeredSprites[i].sortingOrder = AddOrder + 1;
        }
        */
    }

    void SpawnElephant() //create an elephant. pick from colors, hats, tusks, and sizes. also adds a random value to sprite layers so elephants don't blend into each other
    {
        dustParticles.Stop();

        FindObjectOfType<GameController>().ControlledElephants.Add(this);

        orderInLine = FindObjectOfType<GameController>().ControlledElephants.IndexOf(this);

        FindObjectOfType<MainUI>().elephantCounterText.SetText("{0}", FindObjectOfType<GameController>().ControlledElephants.Count - 1);

        followSlider = FindObjectOfType<Slider>();

        if(orderInLine != 0) //extra elephants generate 1 score per 5 seconds. color/hat is 0.25, color+hat is 0.3
        {
            reward = 0.5f;
            SpeedMult = Random.Range(0.9f, 1.1f);

            GenerateNewElephant();
        }
        else //lead elephant generates 1 score per second
        {
            reward = 1f;

            hatRenderer.sprite = FindObjectOfType<HatCollection>().hats[3];
            SpeedMult = 1f;
        }

        bodyAnim.SetFloat("SpeedMult", SpeedMult);
    }

    public void DestroyElephant()
    {
        //if order 0, that's the main guy; remove from list!

        if (orderInLine != 0)
        {
            bodyAnim.SetTrigger("Pop");
            FindObjectOfType<AudioManager>().Play("bubbles");

            Instantiate(bubbleParticles, transform.position, Quaternion.identity);

            List<Elephant> TakeFromList = FindObjectOfType<GameController>().ControlledElephants;

            TakeFromList.Remove(this);
            GetComponent<Collider>().enabled = false;

            Destroy(gameObject, 0.25f);

            FindObjectOfType<MainUI>().elephantCounterText.SetText("{0}", TakeFromList.Count - 1);

            for (int i = 0; i < TakeFromList.Count; i++)
            {
                TakeFromList[i].orderInLine = TakeFromList.IndexOf(TakeFromList[i]);
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("elephantWail");
            FindObjectOfType<AudioManager>().Stop("rumbling"); //add footsteps

            if (FindObjectOfType<MainUI>().Score > PlayerPrefs.GetInt("High Score"))
            {
                PlayerPrefs.SetInt("High Score", (int) FindObjectOfType<MainUI>().Score);

                PlayerPrefs.Save();

                FindObjectOfType<MainUI>().GameOverHighScoreBadge.color = Color.white;
            }
            else
            {
                FindObjectOfType<MainUI>().GameOverHighScoreBadge.color = Color.clear;
            }

            FindObjectOfType<PauseButton>().gameObject.SetActive(false);
            FindObjectOfType<MainUI>().elephantCounterText.transform.parent.parent.gameObject.SetActive(false);

            FindObjectOfType<MainUI>().GameOverScoreText.SetText("Score: {0}", FindObjectOfType<MainUI>().Score);

            control.PauseTime = 999f;
            control.GameOverMenu.SetActive(true);
            //control.RestartGame();
        }
    }

    void GenerateNewElephant()
    {
        int hatChance = Random.Range(0, 10);
        int colorChance = Random.Range(0, 10);
        int scaleChance = Random.Range(0, 5); //0,1,2 is normal, 3 is small, 4 is big. small reduces reward by 50%, big increases by 100%

        if (hatChance > 6) //gain a hat. 30% chance of hat, 3 different hats.
        {
            reward += 0.1f;

            if (hatChance == 7)
            {
                hatRenderer.sprite = FindObjectOfType<HatCollection>().hats[0];
            }
            else if (hatChance == 8)
            {
                hatRenderer.sprite = FindObjectOfType<HatCollection>().hats[1];
            }
            else //crown
            {
                hatRenderer.sprite = FindObjectOfType<HatCollection>().hats[2];
            }
        }

        if (colorChance > 3) //be a color other than gray. 30% chance of gray.
        {
            reward += 0.1f;

            if (colorChance == 4) //red
            {
                skinRenderer.color = new Color32(255, 70, 60, 255);
            }
            else if (colorChance == 5) //blue
            {
                skinRenderer.color = new Color32(100, 200, 255, 255);
            }
            else if (colorChance == 6) //green
            {
                skinRenderer.color = new Color32(140, 255, 160, 255);
            }
            else if (colorChance == 7) //purple
            {
                skinRenderer.color = new Color32(200, 130, 220, 255);
            }
            else if (colorChance == 8) //black
            {
                skinRenderer.color = new Color32(50, 50, 50, 255);
            }
            else //gold
            {
                skinRenderer.color = new Color32(255, 200, 0, 255);
            }
        }

        if (scaleChance > 2)
        {
            if (scaleChance == 3) //small; remove tusk as well
            {
                eyeRenderer.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                transform.localScale = new Vector3(0.85f, 1f, 0.85f);
                tuskRenderer.color = Color.clear;
            }
            else //big
            {
                tuskRenderer.transform.localScale = new Vector3(1.4f, 1.1f, 1f);
                transform.localScale = new Vector3(1.12f, 1f, 1.12f);
            }
        }
    }
}
