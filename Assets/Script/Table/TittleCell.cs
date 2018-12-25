using UnityEngine;
using UnityEngine.UI;

public class TittleCell : MonoBehaviour
{
    public Text Text;

    public string Value
    {
        get { return Text.text; }

        set { Text.text = value; }
    }

    void Awake()
    {
        Text = GetComponent<Text>();
    }
}