using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IThemeController
{
    private IThemeController() { }

    private static IThemeController instance = null;
    public static IThemeController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new IThemeController();
            }
            return instance;
        }
    }

    public List<ITheme> list_UIComponents = new List<ITheme>();

}
