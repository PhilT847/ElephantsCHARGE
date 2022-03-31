using UnityEngine;

public class CliffArea : MonoBehaviour
{
    public GameObject TopPrefab;
    public GameObject BottomPrefab;

    public GameObject[] CliffTops; //4.45 x distance between each cliff
    public GameObject[] CliffBottoms;


    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < CliffTops.Length; i++)
        {
            var newCliffTop = Instantiate(TopPrefab, transform);

            CliffTops[i] = newCliffTop;

            var newCliffBottom = Instantiate(BottomPrefab, transform);

            CliffBottoms[i] = newCliffBottom;
        }

        GetComponent<MovingObject>().CanMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -100f)
        {
            GetComponent<MovingObject>().CanMove = false;
            transform.position = new Vector3(30f, 1f, 0f);
            FindObjectOfType<GameController>().GenerateNextObstacle(false);
        }
    }

    public void CreateNewPattern() //builds a new bridge. Can either be ascending/short, descending/short, smallWaves, or bigWaves
    {
        transform.position = new Vector3(30f, 1f, 0f);

        int randomChoice = Random.Range(0, 2);

        if (randomChoice == 0) //0; shorter bridge with no wave, but can constrict
        {
            int constricts = Random.Range(0, 3); //if 0 (33% chance), the tunnel constricts

            if(constricts == 0) //longer constrictor bridge
            {
                GeneratePattern(10, 16, Random.Range(11f, 9f), Random.Range(0.15f, -0.15f), false, 2f, true);
            }
            else //short bridge
            {
                GeneratePattern(4, 8, Random.Range(11f, 9f), Random.Range(0.4f, -0.4f), false, 2f, false);
            }
        }
        else //1; wave pattern.
        {
            GeneratePattern(15, 20, 10f, Random.Range(-0.6f, -0.8f), true, 2f, false);
        }
    }

    void GeneratePattern(int minLength, int maxLength, float startTopPosition, float zChangePerCliff, bool waves, float distanceNeededToWave, bool constricts)
    {
        SetLength(minLength, maxLength);
        float nextCliffDistanceChange = zChangePerCliff;

        float xPosition = 0f;
        float zPosition = startTopPosition;

        //reduce bridge width if there are less elephants... usual width is 18
        float bridgeWidth = 13f + (FindObjectOfType<GameController>().ControlledElephants.Count * 0.375f);

        if (constricts) //max 21
        {
            bridgeWidth = 17f + (FindObjectOfType<GameController>().ControlledElephants.Count * 0.25f);
        }

        for (int i = 0; i < CliffTops.Length; i++)
        {
            CliffTops[i].transform.localPosition = new Vector3(xPosition, 1f, zPosition);
            CliffBottoms[i].transform.localPosition = new Vector3(xPosition, 1f, zPosition - bridgeWidth);

            xPosition += 4.45f;
            zPosition += zChangePerCliff;

            if (constricts)
            {
                bridgeWidth -= 0.35f;
            }

            if (waves && ((zPosition < startTopPosition - distanceNeededToWave) || (zPosition > startTopPosition + distanceNeededToWave)))
            {
                zChangePerCliff *= -1f;
            }
        }
    }

    void SetLength(int min, int max) //cliff segments are 10-20 segments long. "max" goes up to 20
    {
        int randomNumber = Random.Range(min, max);

        for(int i = 0; i < randomNumber; i++) //set 0 to the maxmimum as active
        {
            CliffTops[i].SetActive(true);

            CliffBottoms[i].SetActive(true);
        }

        for(int i = randomNumber; i < CliffBottoms.Length; i++) //set max to the end as inactive
        {
            CliffTops[i].SetActive(false);

            CliffBottoms[i].SetActive(false);
        }
    }
}
