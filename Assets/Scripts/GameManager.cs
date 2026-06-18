using System;
using BreakInfinity;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private Menu menu;

    private bool _gameStarted = false;
    private bool _withDefeats = false;
    private int _defeatsOn = 0; // 0 - off, 1 - on

    public Toggle DefeatsMode;

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
    private string colorID;

    [Header("Pause")]
    private bool _isPaused;
    
    public GameObject PausePanel;
    public GameObject PauseText;


    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        //StartGame(ModeNow);
        _gameStarted = false;
        menu.MenuPanel.SetActive(true);
        PausePanel.SetActive(false);
        _defeatsOn = PlayerPrefs.GetInt("DefeatsMode", 1);
        if (_defeatsOn == 0)
            DefeatsMode.isOn = false;
        else
            DefeatsMode.isOn = true;
    }

    public void plusPoint()
    {
        points++;
        tilesfalling--;
        Time.timeScale += timeStep;
    }

    public void StartGame(GameMode mode)
    {
        Debug.Log($"Game started! FallingTimer:{fallingtimer}; TimerCounter:{timercounter}");
        if (menu.MenuPanel.activeSelf) { menu.MenuPanel.SetActive(false); }

        _withDefeats = DefeatsMode.isOn;

        if (_withDefeats)
            _defeatsOn = 1;
        else
            _defeatsOn = 0;

        PlayerPrefs.SetInt("DefeatsMode", _defeatsOn);
        switch (mode)
        {
            case GameMode.Normal:
                Time.timeScale = 1.0f;
                points = 0;
                fallingtimer = fallingtimerNormal;
                timeStep = timeStepNormal;
                fallingtimeradder = fallingtimerNormalAdder;
                colorID = "#B8FFA6";
                break;
            case GameMode.Fast:
                Time.timeScale = 1.0f;
                points = 0;
                fallingtimer = fallingtimerFast;
                timeStep = timeStepFast;
                fallingtimeradder = fallingtimerFastAdder;
                colorID = "#FF7D7D";
                break;
            case GameMode.Slow:
                Time.timeScale = 1.0f;
                points = 0;
                fallingtimer = fallingtimerSlow;
                timeStep = timeStepSlow;
                fallingtimeradder = fallingtimerSlowAdder;
                colorID = "#C6FCFF";
                break;
        }
        string record = PlayerPrefs.GetString("record_" + mode, "0");
        if (record != "")
            maxpoints = double.Parse(record);
        ModeNow = mode;
        //SpawnTile();
        if (_withDefeats || !_gameStarted)
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
            maxpoints_text.text = $"<color={colorID}>Record:\r\n{maxpoints}";

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

    private double savedTimeScale = 0;

    public void Pause()
    {
        Debug.Log("Pause called!");
        _isPaused = !_isPaused;

        Debug.Log($"Pause status: {_isPaused}");
        if (_isPaused)
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            PausePanel.SetActive(true);
            PauseText.GetComponent<Animator>().enabled = true;
            PauseText.GetComponent<Animator>().SetTrigger("Text animation fade");
        }
        else
        {
            PausePanel.SetActive(false);
            Time.timeScale = (float)savedTimeScale;
        }

    }

    private void OnApplicationPause(bool pause)
    {
        if(pause && _gameStarted)
            Pause();
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
    public void SetGameStatus(bool set)
    {
        _gameStarted = set;
    }
    public bool WithDefeats()
    {
        return _withDefeats;
    }

    public GameMode GetMode()
    {
        return ModeNow;
    }

    public Menu GetMenu()
    {
        return menu;
    }
}
