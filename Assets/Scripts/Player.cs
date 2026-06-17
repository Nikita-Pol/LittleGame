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

    void Start()
    {
        _isAlive = true;
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

    void Failing()
    {
        _isAlive = false;
        Instance.PointsSave();
        Instance.StartGame(Instance.GetMode());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Obstacle"))
            Failing();

    }

    void Update()
    {
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
