using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    public static MenuManager Instance;
    

    [SerializeField]
    Menu[] menus;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        OpenMenu("loading");
    }

    public void OpenMenu(String menuName) {
        for (int i = 0; i < menus.Length; i++) {
            if (menus[i].menuName == menuName) {
                menus[i].Open();
            } else if (menus[i].open) {
                CloseMenu(menus[i]);
            }
        }

    }

    public void OpenMenu(Menu menu) {
        for (int i = 0; i < menus.Length; i++) {
            if (menus[i].open) {
                CloseMenu(menus[i]);
            }
        }
        menu.Open();
    }

    public void CloseMenu(Menu menu) {
        menu.Close();
    }

}
