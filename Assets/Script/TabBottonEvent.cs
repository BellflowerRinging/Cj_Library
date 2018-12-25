using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class TabBottonEvent : MonoBehaviour
{

    [Header("Defaule Color")]
    public ColorBlock ColorBlock;
    [Header("Focus Color")]
    public ColorBlock FocusColorBlock;

    public int FocueIndex;

    public TabGround[] TabGrounds;
    // Use this for initialization
    void Start()
    {
        Array.ForEach(TabGrounds, delegate (TabGround s)
         {
             s.button.colors = ColorBlock;
             s.page.SetActive(false);
             if (s.button != null) s.button.onClick.AddListener(delegate ()
              {
                  Array.ForEach(TabGrounds, delegate (TabGround t)
                  {
                      if (!t.Equals(s))
                      {
                          t.page.SetActive(false);
                          t.button.colors = ColorBlock;
                      }
                      else
                      {
                          t.page.SetActive(true);
                          t.button.colors = FocusColorBlock;
                      }
                  });
              });
         });

        if (FocueIndex > TabGrounds.Length) FocueIndex = 0;

        TabGrounds[FocueIndex].button.colors = FocusColorBlock;
        TabGrounds[FocueIndex].page.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitApplication()
    {
        Application.Quit();
    }

}

[System.Serializable]
public struct TabGround
{
    public Button button;
    public GameObject page;

    public override bool Equals(object obj)
    {
        if (!(obj is TabGround))
        {
            return false;
        }

        var ground = (TabGround)obj;
        return EqualityComparer<Button>.Default.Equals(button, ground.button) &&
               EqualityComparer<GameObject>.Default.Equals(page, ground.page);
    }

    public override int GetHashCode()
    {
        var hashCode = 404825487;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<Button>.Default.GetHashCode(button);
        hashCode = hashCode * -1521134295 + EqualityComparer<GameObject>.Default.GetHashCode(page);
        return hashCode;
    }
}
