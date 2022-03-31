using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public float GameSpeed;
    public List<Elephant> ControlledElephants;

    public float PauseTime;

    public int currentObstacle; //0 (crate), 1 (holes), 2 (cliff)

    public GameObject[] obstacles;

    public GameObject MainMenu;

    public TextMeshProUGUI HighScoreText;

    public GameObject GameOverMenu;

    void Awake()
    {
        if (PlayerPrefs.HasKey("High Score"))
        {
            HighScoreText.SetText("High Score: {0}", PlayerPrefs.GetInt("High Score"));
        }
        else
        {
            PlayerPrefs.SetInt("High Score", 0);
        }

        Debug.Log("Current High: " + PlayerPrefs.GetInt("High Score"));

        ControlledElephants = new List<Elephant>();

        OpenGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseTime <= 0f)
        {
            if(GameSpeed < 40f)
            {
                GameSpeed += 0.2f * Time.deltaTime; //add 1 (5%) every 5s
            }
            else
            {
                GameSpeed = 40f;
            }
        }
        else if (PauseTime > 0f && PauseTime < 10f) //pausing sets time to 10.1f so it doesn't go down; set to 3 when unpausing
        {
            PauseTime -= Time.deltaTime;

            if(PauseTime <= 0f) //return to footsteps once pause ends
            {
                FindObjectOfType<AudioManager>().Play("rumbling"); //add footsteps
            }
        }
    }

    public void OpenGame() //played when opening
    {
        PauseTime = 999f;
        GameSpeed = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void StartGame()
    {
        PauseTime = 0f;

        GameSpeed = 20f;

        GenerateNextObstacle(true);

        FindObjectOfType<AudioManager>().Play("rumbling"); //add footsteps

        /*
        currentObstacle = 0;
        FindObjectOfType<Crate>().GetComponent<MovingObject>().CanMove = true;
        FindObjectOfType<Crate>().transform.position = new Vector3(30f, 1f, -1f);
        */
    }

    public void GenerateNextObstacle(bool ForceCrate)
    {
        int chooseType = Random.Range(0, 4); //0 (crate), 1 (holes), 2-3 (cliffs)

        if (currentObstacle != 0 && chooseType != 0) //reroll when you didn't get a crate last time... double chance after an obstacle
        {
            chooseType = Random.Range(0, 5);
        }

        if(chooseType == 0 || ForceCrate) //crate... forced at the start.
        {
            obstacles[0].GetComponent<MovingObject>().CanMove = true;
            obstacles[1].GetComponent<MovingObject>().CanMove = false;
            obstacles[2].GetComponent<MovingObject>().CanMove = false;

            currentObstacle = 0;

            obstacles[0].transform.position = new Vector3(30f, 1f, Random.Range(4f, -7f));

            /*
            FindObjectOfType<Crate>().GetComponent<MovingObject>().CanMove = true;
            FindObjectOfType<Crate>().transform.position = new Vector3(30f, 1f, Random.Range(4f, -7f));

            StopAllExcept(0);
            */
        }
        else if (chooseType == 1) //holes
        {
            obstacles[0].GetComponent<MovingObject>().CanMove = false;
            obstacles[1].GetComponent<MovingObject>().CanMove = true;
            obstacles[2].GetComponent<MovingObject>().CanMove = false;

            currentObstacle = 1;

            obstacles[1].GetComponent<HoleArea>().GenerateHoles();

            /*
            currentObstacle = 1;
            FindObjectOfType<HoleArea>().GetComponent<MovingObject>().CanMove = true;
            FindObjectOfType<HoleArea>().GenerateHoles();

            StopAllExcept(1);
            */
        }
        else //cliffs (double chance, 2 or 3)
        {
            obstacles[0].GetComponent<MovingObject>().CanMove = false;
            obstacles[1].GetComponent<MovingObject>().CanMove = false;
            obstacles[2].GetComponent<MovingObject>().CanMove = true;

            currentObstacle = 2;

            obstacles[2].GetComponent<CliffArea>().CreateNewPattern();

            /*
            currentObstacle = 2;
            FindObjectOfType<CliffArea>().GetComponent<MovingObject>().CanMove = true;
            FindObjectOfType<CliffArea>().CreateNewPattern();

            StopAllExcept(2);
            */
        }

        //Debug.Log("Generated " + chooseType);
    }
}
