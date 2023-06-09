using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject menuUI;
    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject MenuUI
    {
        get
        {
            return menuUI;
        }

        set
        {
            menuUI = value;
        }
    }



    public void EnableMenuUI(bool enable)
    {
        menuUI.SetActive(enable);
    }


}
