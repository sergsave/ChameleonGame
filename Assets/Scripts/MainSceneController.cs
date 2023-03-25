using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneController : MonoBehaviour
{
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

    private GameObject _firstTarget;
    private GameObject _secondTarget;

    private int _score = 0;
    private int _resultScore = 0;
    private int _maxScore = 0;

    private enum GameState
    {
        Start,
        Play,
        Loose
    }

    private GameState _gameState = GameState.Start;
    private Dictionary<GameState, List<GameObject>> _state2visibleObjects;

    void Start()
    {
        _state2visibleObjects = new Dictionary<GameState, List<GameObject>> {
            { GameState.Start, new List<GameObject>() { startCanvas.gameObject }},
            { GameState.Play, new List<GameObject>() { playCanvas.gameObject, gameplayFrame }},
            { GameState.Loose, new List<GameObject>() { looseCanvas.gameObject }}
        };

        chameleonController.targetCatchedCallback = () =>
        {
            _score++;
            _maxScore = Mathf.Max(_maxScore, _score);
        };

        chameleonController.missedCallback = () =>
        {
            _gameState = GameState.Loose;

            _resultScore = _score;
            _score = 0;
        };
    }

    void Update()
    {
        //TODO: update optimization?

        UpdateObjectsVisibility();

        chameleonController.isControllable = _gameState == GameState.Play;

        switch (_gameState)
        {
            case GameState.Start:
                UpdateStartUI();

                if (Input.GetMouseButtonDown(0))
                    _gameState = GameState.Play;
                break;
            case GameState.Play:
                UpdatePlayUI();

                CreateTargets();
                break;
            case GameState.Loose:
                UpdateLooseUI();

                if (Input.GetMouseButtonDown(0))
                    _gameState = GameState.Start;

                DestroyTargets();
                break;
            default:
                break;
        }
    }

    private void UpdateObjectsVisibility()
    {
        foreach (var item in _state2visibleObjects)
        {
            foreach (var gameObject in item.Value)
            {
                gameObject.SetActive(_gameState == item.Key);
            }
        }
    }

    private void UpdateLooseUI()
    {
        resultLabel.SetText(_resultScore.ToString());
    }

    private void UpdatePlayUI()
    {
        scoreLabel.SetText(_score.ToString());
    }

    private void UpdateStartUI()
    {
        highScoreLabel.SetText("Best: " + _maxScore.ToString());
    }

    private void CreateTargets()
    {
        if (_firstTarget != null || _secondTarget != null)
            return;

        _firstTarget = CreateTarget(firstTargetPosition);
        _secondTarget = CreateTarget(secondTargetPosition);
    }

    private void DestroyTargets()
    {
        if (_firstTarget == null && _secondTarget == null)
            return;

        if (_firstTarget != null)
            Destroy(_firstTarget);
        if (_secondTarget != null)
            Destroy(_secondTarget);
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
