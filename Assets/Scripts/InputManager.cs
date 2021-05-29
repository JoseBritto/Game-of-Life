using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager
{
    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                instance = new InputManager();
            return instance;
        }
    }

    private static InputManager instance;

    public Controls InputControls { get; private set; }

    private InputManager()
    {
        InputControls = new Controls();
        InputControls.Enable();

        //For debug stuff 
        SetAsInGame();//inui must be default
        //delete this
    }

    ~InputManager()
    {
        InputControls.Disable();
        InputControls.Dispose();
    }

    public void SetAsInUI()
    {
        InputControls.InGame.Disable();
        InputControls.InUI.Enable();
    }

    public void SetAsInGame()
    {
        InputControls.InUI.Disable();
        InputControls.InGame.Enable();
    }

}
