using UnityEngine;


public class HintWindowManager : MonoBehaviour
{
    public HintWindowContorl SetContorl;

    public static HintWindowContorl Contorl;

    void Start()
    {
        Contorl = SetContorl;
    }
}