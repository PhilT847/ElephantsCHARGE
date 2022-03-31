using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Rigidbody body;
    public GameController MainControl;

    public bool CanMove;

    public bool IsGrass;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        MainControl = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanMove && MainControl.PauseTime <= 0f)
        {
            body.velocity = Vector2.left * MainControl.GameSpeed;
        }
        else
        {
            body.velocity = Vector2.zero;
        }

        if(IsGrass && transform.position.x < -20f)
        {
            transform.position = new Vector3(Random.Range(25f, 40f), 1f, Random.Range(-6f, 6f));
        }
    }
}
