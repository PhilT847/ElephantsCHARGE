using UnityEngine;

public class TreeFrame : MonoBehaviour
{
    private Rigidbody body;
    public GameController MainControl;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody>();
        MainControl = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (MainControl.PauseTime <= 0f)
        {
            body.velocity = Vector2.left * 1.1f * MainControl.GameSpeed;

            if (transform.position.x < -30f)
            {
                transform.position = new Vector3(Random.Range(29.75f,30.25f), transform.position.y, transform.position.z);
            }
        }
        else
        {
            body.velocity = Vector2.zero;
        }
    }
}
