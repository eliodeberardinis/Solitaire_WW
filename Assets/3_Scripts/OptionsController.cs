using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

    public GameObject optionScreen;

	// Use this for initialization
	void Start ()
    {
        
	}


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

    public void ResumeGame()
    {
        optionScreen.SetActive(false);
    }

    public void PauseGame()
    {
        if (!Card.isFlippingOn && !GameController.isTranslationOn && !Card.isDragging)
        {
            optionScreen.SetActive(true);
        }
               
    }
}
