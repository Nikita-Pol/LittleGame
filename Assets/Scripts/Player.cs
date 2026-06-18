using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameManager;

public class Player : MonoBehaviour
{
    public bool _isAlive;
    public int line;
    public List<GameObject> player_tiles;
    public GameObject PlayerTile;
    public AudioSource fx;
    public AudioClip crash;

    void Start()
    {
        _isAlive = false;
        line = 0;
    }

    public void ChangeLine(int l)
    {
        if (l == -1 && line > 0)
            line--;
        else if (l == 1 && line < player_tiles.Count - 1)
            line++;

        PlayerTile.transform.position = player_tiles[line].transform.position;
    }

    void Failing(bool forceFail = false)
    {
        _isAlive = false;
        Time.timeScale = 1f;
        Instance.PointsSave();
        Instance.GetMenu().LoadRecords();
        Instance.PausePanel.SetActive(false);
        if (Instance.WithDefeats())
            Instance.SetGameStatus(false);
        if (!Instance.WithDefeats() && !forceFail)
            Instance.StartGame(Instance.GetMode());
        else
            Instance.GetMenu().MenuPanel.SetActive(true);

        if (!forceFail)
        {
            float volume;
            if (Instance.GetMenu().audiosource.volume == 0)
                volume = 0;
            else
                volume = 1;
            fx.PlayOneShot(crash, volume);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            Failing();
            collision.GetComponentInParent<Obstacle>().SelfDestroy();
        }

    }

    public void Closer()
    {
        Failing(true);
    }

    void Update()
    {
        if (Instance.CheckGame() ) _isAlive = true;
        else _isAlive = false;

        if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            if (line > 0)
                line--;
        }
        else if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            if (line < player_tiles.Count - 1)
                line++;
        }

        PlayerTile.transform.position = player_tiles[line].transform.position;

       /* Debug.Log($"line: {line}, tiles count: {player_tiles.Count}");
        Debug.Log($"target pos: {player_tiles[line].transform.position}");*/
    }
}
