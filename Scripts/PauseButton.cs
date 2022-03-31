using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
    public GameObject PausePanel;

    public Sprite pauseSprite;
    public Sprite playSprite;

    public Slider restartSlider;

    private bool Activated;

    public Slider peanut;

    private void Update()
    {
        if(!Activated && FindObjectOfType<GameController>().PauseTime > 0f)
        {
            if (!restartSlider.gameObject.activeSelf)
            {
                restartSlider.gameObject.SetActive(true);
                peanut.interactable = false;
                GetComponent<Button>().interactable = false;
            }

            restartSlider.value = FindObjectOfType<GameController>().PauseTime;
        }
        else if (!peanut.interactable)
        {
            GetComponent<Button>().interactable = true;
            peanut.interactable = true;
            restartSlider.gameObject.SetActive(false);
        }
    }

    public void TogglePause()
    {
        FindObjectOfType<AudioManager>().Stop("rumbling"); //remove footsteps even when unpausing; it'll come back when the timer's up.
        peanut.interactable = false; //disable sliding

        if (!Activated)
        {
            GetComponent<Image>().sprite = playSprite;
            FindObjectOfType<GameController>().PauseTime = 999f;
            PausePanel.SetActive(true);

        }
        else
        {
            GetComponent<Image>().sprite = pauseSprite;
            FindObjectOfType<GameController>().PauseTime = 1.5f;
            PausePanel.SetActive(false);
        }

        Activated = !Activated;
    }
}
