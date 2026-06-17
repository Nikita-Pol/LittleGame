using System;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private bool _gameStarted = false;
    private bool _withDefeats = false;

    public double points;
    double maxpoints = 0;
    double tilesfalling;
    public TextMeshProUGUI points_text;
    public TextMeshProUGUI maxpoints_text;
    [SerializeField] private GameObject obstacle;
    [SerializeField] private GameObject spawner_obstacle;
    [Header("Setup")]
    private float fallingtimer = 5f;
    [SerializeField] private float fallingtimerFast = 3f;
    [SerializeField] private float fallingtimerNormal = 8f;
    [SerializeField] private float fallingtimerSlow = 20f;
    float timercounter;
    private float timeStep;
    [SerializeField] private float timeStepFast;
    [SerializeField] private float timeStepNormal;
    [SerializeField] private float timeStepSlow;
    private float fallingtimeradder = 0f;
    [SerializeField] private float fallingtimerFastAdder = 0.02f;
    [SerializeField] private float fallingtimerNormalAdder = 0.05f;
    [SerializeField] private float fallingtimerSlowAdder = 0.1f;
    [SerializeField] private GameMode ModeNow;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        StartGame(ModeNow);
        Debug.Log($"Game started! FallingTimer:{fallingtimer}; TimerCounter:{timercounter}");
    }

    public void plusPoint()
    {
        points++;
        tilesfalling--;
        Time.timeScale += timeStep;
    }

    public void StartGame(GameMode mode)
    {
        _gameStarted = false;
        switch(mode)
        {
            case GameMode.Normal:
                Time.timeScale = 1.0f;
                points = 0;
                fallingtimer = fallingtimerNormal; 
                timeStep = timeStepNormal;
                fallingtimeradder = fallingtimerNormalAdder;
                break;
            case GameMode.Fast:
                Time.timeScale = 1.0f;
                points = 0;
                fallingtimer = fallingtimerFast; 
                timeStep = timeStepFast;
                fallingtimeradder = fallingtimerFastAdder;
                break;
            case GameMode.Slow:
                Time.timeScale = 1.0f;
                points = 0;
                fallingtimer = fallingtimerSlow; 
                timeStep = timeStepSlow;
                fallingtimeradder = fallingtimerSlowAdder;
                break;
        }
        string record = PlayerPrefs.GetString("record_" + mode, "0");
        if (record != "")
            maxpoints = double.Parse(record);
        ModeNow = mode;
        //SpawnTile();
        foreach (Transform child in spawner_obstacle.transform)
            Destroy(child.gameObject);
        _gameStarted = true;
    }

    public enum GameMode
    {
        Normal, Fast, Slow
    }

    void Update()
    {
        if(points_text != null)
            points_text.text = points.ToString();

        if (maxpoints_text != null)
            maxpoints_text.text = $"Record:{maxpoints}";

        if (!_gameStarted)
            return;

        timercounter -= Time.deltaTime;

        if (timercounter <= 0)
            SpawnTile();
    }
    private void OnApplicationQuit()
    {

       PointsSave();
    }

    public void PointsSave()
    {
        if (Instance.GetMaxPoints() < Instance.points)
        {
            PlayerPrefs.SetString("record_" + ModeNow, points.ToString());
            SetMaxPoints(points);
        }
    }

    void SpawnTile()
    {
        timercounter = fallingtimer + (fallingtimeradder * (int)points);
        tilesfalling++;
        Instantiate(obstacle, spawner_obstacle.transform);
    }

    public double GetMaxPoints()
    {
        return maxpoints;
    }
    public void SetMaxPoints(double set)
    {
        maxpoints = set;
    }

    public bool CheckGame()
    {
        return _gameStarted;
    }

    public GameMode GetMode()
    {
        return ModeNow;
    }
}
