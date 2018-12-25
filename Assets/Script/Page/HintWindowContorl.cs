using UnityEngine;
using UnityEngine.UI;
public class HintWindowContorl : MonoBehaviour
{
    public Text Tittle;
    public Text Content;
    public Button EnterButton;

    void Start()
    {
        EnterButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    public void Show(string content, string tittle = "提示")
    {
        Content.text = content;
        Tittle.text = tittle;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

