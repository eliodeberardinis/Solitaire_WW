using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsController : MonoBehaviour {

    GameObject winningScreen;

	// Use this for initialization
	void Start ()
    {
        winningScreen = GameObject.FindGameObjectWithTag("WinScreen");
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
}
