using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIGroundContorll : MonoBehaviour {

    public GameObject m_LoginGround;
    public GameObject m_RegistGround;
    public GameObject m_RepasswordGround;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ShowLoginGround()
    {
        ShowGround(m_LoginGround);
    }

    public void ShowRegisGround()
    {
        ShowGround(m_RegistGround);
    }

    public void ShowRepasswordGround()
    {
        ShowGround(m_RepasswordGround);
    }

    public void ShowGround(GameObject ground)
    {
        AllHiden();
        ground.SetActive(true);
    }

    public void AllHiden()
    {
        m_LoginGround.SetActive(false);
        m_RegistGround.SetActive(false);
        m_RepasswordGround.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
