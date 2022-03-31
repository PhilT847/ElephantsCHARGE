using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleArea : MonoBehaviour
{
    public GameObject[] HoleObjects;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MovingObject>().CanMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < -100f)
        {
            GetComponent<MovingObject>().CanMove = false;
            transform.position = new Vector3(30f, 1f, 0f);
            FindObjectOfType<GameController>().GenerateNextObstacle(false);
        }
    }

    public void GenerateHoles() //activate 3-5 holes and place them from z=-5 to z=6
    {
        transform.position = new Vector3(30f, 1f, 0f);

        int holeCount = Random.Range(3, 6); //3, 4, 5

        for(int i = 0; i < holeCount; i++)
        {
            HoleObjects[i].SetActive(true);
        }
        for(int i = holeCount; i < HoleObjects.Length; i++)
        {
            HoleObjects[i].SetActive(false);
        }

        float xPosition = 0f;

        for(int i = 0; i < HoleObjects.Length; i++)
        {
            HoleObjects[i].transform.localPosition = new Vector3(xPosition, 1f, Random.Range(-5f, 6f));

            xPosition += Random.Range(12f, 20f);
        }
    }
}
