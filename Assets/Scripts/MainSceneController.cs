using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
    // TODO: Do not list all objects. Remake visibility control
    public GameObject gameplayFrame;
    public Canvas startCanvas;
    public Canvas playCanvas;
    public Canvas looseCanvas;
    public ChameleonController chameleonController;
    public GameObject targetPrefab;

    public TMPro.TMP_Text scoreLabel;
    public TMPro.TMP_Text highScoreLabel;
    public TMPro.TMP_Text resultLabel;

    public Vector2 firstTargetPosition;
    public Vector2 secondTargetPosition;

    private int _score = 0;
    private GameObject[] _targets;
    private int _maxScore = 0;

    private enum GameState
    {
        Start,
        Play,
        Loose
    }

    private GameState _gameState = GameState.Start;
    private bool _wasClicked = false;

    void Start()
    {
        _targets = new GameObject[2];
        UpdateScore(0);
        UpdateResult(0);

        chameleonController.targetCatchedCallback = () =>
        {
            UpdateScore(_score + 1);
        };
        chameleonController.missedCallback = () =>
        {
            _gameState = GameState.Loose;
            UpdateResult(_score);
            UpdateScore(0);
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            _wasClicked = false;
        }

        // TODO: reduce copypaste
        switch (_gameState)
        {
            case GameState.Start:
                playCanvas.enabled = false;
                looseCanvas.enabled = false;

                gameplayFrame.SetActive(false);
                chameleonController.isControllable = false;

                startCanvas.enabled = true;

                if (!_wasClicked && Input.GetMouseButtonDown(0))
                {
                    _gameState = GameState.Play;
                    _wasClicked = true;
                }
                break;
            case GameState.Play:
                looseCanvas.enabled = false;
                startCanvas.enabled = false;

                playCanvas.enabled = true;
                gameplayFrame.SetActive(true);

                // TODO: remake control system?
                if (Input.GetMouseButtonUp(0))
                {
                    chameleonController.isControllable = true;
                }

                CreateTargets();

                break;
            case GameState.Loose:
                playCanvas.enabled = false;
                startCanvas.enabled = false;

                gameplayFrame.SetActive(false);
                chameleonController.isControllable = false;

                looseCanvas.enabled = true;

                if (!_wasClicked && Input.GetMouseButtonDown(0))
                {
                    _gameState = GameState.Start;
                    _wasClicked = true;
                }

                DestroyTargets();
                break;
            default:
                break;
        }
    }

    private void UpdateScore(int newScore)
    {
        _score = newScore;
        _maxScore = Mathf.Max(_maxScore, _score);
        scoreLabel.SetText(_score.ToString());
        highScoreLabel.SetText("Best: " + _maxScore.ToString());
    }

    private void UpdateResult(int result)
    {
        resultLabel.SetText(result.ToString());
    }

    private void CreateTargets()
    {
        if (_targets[0] != null || _targets[1] != null)
            return;

        _targets[0] = CreateTarget(firstTargetPosition);
        _targets[1] = CreateTarget(secondTargetPosition);
    }

    private void DestroyTargets()
    {
        if (_targets[0] == null && _targets[1] == null)
            return;

        if (_targets[0] != null)
            Destroy(_targets[0]);
        if (_targets[1] != null)
            Destroy(_targets[1]);
    }

    private GameObject CreateTarget(Vector2 pos)
    {
        // TODO: Full random range
        int multiplier = (Random.Range(0, 2) == 1) ? 1 : -1;
        GameObject target = Instantiate(targetPrefab, new Vector3(multiplier * pos.x, pos.y, 0), Quaternion.identity);
        target.transform.parent = gameplayFrame.transform;
        return target;
    }
}
