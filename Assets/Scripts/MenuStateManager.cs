using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//SINGLETON
public class MenuStateManager : MonoBehaviour {
    static MenuStateManager _Instance = null;
    public static MenuStateManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = FindObjectOfType<MenuStateManager>();
                {
                    if (!_Instance)
                        Debug.LogError("No menu state manager on scene!");
                }
            }
            return _Instance;
        }
    }

    public Canvas gameView;
    Canvas currentMenu = null;
    Dictionary<string, Canvas> menus = null;

    void Start()
    {
        Instance.SetupMenus();
    }

    void SetupMenus()
    {
        if (menus != null)
            return;

        menus = new Dictionary<string, Canvas>();

        List<Canvas> canvasList = new List<Canvas>(FindObjectsOfType<Canvas>());

        foreach (Canvas canvas in canvasList)
        {
            menus.Add(canvas.name, canvas);
        }

        RequestSwitchMenu("MainViewCanvas");
    }

    public void RequestSwitchMenu(string newMenuName)
    {
        Canvas newMenu = null;

        if (menus.TryGetValue(newMenuName, out newMenu))
        {
            foreach (Canvas canvas in menus.Values)
            {
                if (canvas == newMenu)
                {
                    canvas.enabled = true;
                    currentMenu = newMenu;
                }
                else
                    canvas.enabled = false;
            }
                
            return;
        }

        Debug.LogError("Menu " + newMenuName + " not found in menus.");

        return;
    }

    public bool AllowsMouseOnGrid()
    {
        return currentMenu == gameView;
    }
}
