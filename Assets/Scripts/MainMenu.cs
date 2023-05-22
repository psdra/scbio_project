using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    DataImporter importData;
    private float toggleTimer;
    private bool inRight, inLeft, inDown;

     //gets called by the START button
    public void GameStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ErsteSzene");
    }

    //gets called by the STOP button
    public void GameQuit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }


    public Button[] menuButtons; // eine Liste der Menü-Buttons
    private int currentIndex = 0; // der aktuell ausgewählte Button-Index

    void Start()
    {
        importData = GameObject.FindObjectOfType<DataImporter>();
        // den ersten Button als ausgewählt markieren
        menuButtons[currentIndex].Select();
        toggleTimer = 0.0f;
    }

    void Update()
    {
                //runs timer
        toggleTimer += Time.deltaTime;
        inRight = importData.right;
        inLeft = importData.left;
        inDown = importData.down;

        // mit S- und W-Tasten zwischen den Buttons navigieren
        if (inRight && toggleTimer > 0.2f)
        {
            toggleTimer = 0.0f;
            // wenn wir am Ende des Menüs angekommen sind, zum Anfang springen
            if (currentIndex == menuButtons.Length - 1)
            {
                currentIndex = 0;
            }
            else
            {
                currentIndex++;
            }
            menuButtons[currentIndex].Select();
        }
        else if (inLeft && toggleTimer > 0.2f)
        {
            toggleTimer = 0.0f;
            // wenn wir am Anfang des Menüs angekommen sind, zum Ende springen
            if (currentIndex == 0)
            {
                currentIndex = menuButtons.Length - 1;
            }
            else
            {
                currentIndex--;
            }
            menuButtons[currentIndex].Select();
        }

        // Enter-Taste drücken, um den ausgewählten Button auszulösen
        if (inDown && toggleTimer > 0.2f)
        {
            toggleTimer = 0.0f;
            menuButtons[currentIndex].onClick.Invoke();
        }
    }
}



