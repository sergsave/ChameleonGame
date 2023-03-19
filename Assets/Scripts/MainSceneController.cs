using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    // TODO: Do not list all objects. Remake visibility control
    public GameObject gameplayFrame;
    public Canvas startCanvas;
    public Canvas playCanvas;
    public ChameleonController chameleonController;

    public TMPro.TMP_Text scoreLabel;

    private int _score = 0;

    private enum GameState
    {
        Start,
        Play
    }

    private GameState _gameState = GameState.Start;
    private bool _wasClicked = false;

    void Update()
    {
        if (_gameState == GameState.Start && Input.GetMouseButtonDown(0))
        {
            _gameState = GameState.Play;
            _wasClicked = true;
        }


        switch (_gameState)
        {
            case GameState.Start:
                startCanvas.enabled = true;
                playCanvas.enabled = false;

                gameplayFrame.SetActive(false);
                chameleonController.isControllable = false;
                break;
            case GameState.Play:
                startCanvas.enabled = false;
                playCanvas.enabled = true;

                gameplayFrame.SetActive(true);

                // TODO: remake control system?
                if (_wasClicked && Input.GetMouseButtonUp(0))
                {
                    chameleonController.isControllable = true;
                    _wasClicked = false;
                }

                break;
            default:
                break;
        }
    }
}
