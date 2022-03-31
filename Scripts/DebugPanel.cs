using UnityEngine;
using TMPro;

public class DebugPanel : MonoBehaviour
{
    public TextMeshProUGUI textBox;
    public MovingObject[] objects;

    // Update is called once per frame
    void Update()
    {
        textBox.SetText("Crate: {0} (" + objects[0].CanMove + ")\nHoles: {1} (" + objects[1].CanMove + ")\nCliffs: {2} (" + objects[2].CanMove + ")", objects[0].transform.position.x, objects[1].transform.position.x, objects[2].transform.position.x);
    }
}
