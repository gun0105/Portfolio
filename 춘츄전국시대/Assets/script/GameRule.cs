using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRule : MonoBehaviour
{
    public GameObject open;

    public void openhelp()
    {
        open.SetActive(true);

    }
    public void closehelp()
    {
        open.SetActive(false);
    }
}
