using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

    public GameObject optionScreen;

    //Resets the game. Called by UI button in the options on click
    public void RestartGame()
    {
        GameController.ListIndex = 0;
        GameController.score = 0;
        GameController.moves = 0;
        GameController.isTranslationOn = false;
        Card.isDragging = false;
        Card.isFlippingOn = false;
        SceneManager.LoadScene("1_GamePlay");
    }

    //Resume the game. Called by UI button on click in options
    public void ResumeGame()
    {
        optionScreen.SetActive(false);
    }

    //Resume the game. Called by UI button on click in main screen
    public void PauseGame()
    {
        if (!Card.isFlippingOn && !GameController.isTranslationOn && !Card.isDragging)
        {
            optionScreen.SetActive(true);
        }            
    }
}
