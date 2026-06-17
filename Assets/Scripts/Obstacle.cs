using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private float speed = 1000;
    public List<Tile> tiles = new List<Tile>();



    void Start()
    {
        foreach (var tile in tiles)
        {
            tile._isActive = false;
            tile.Updater();
        }

        List<Tile> shuffled = tiles.OrderBy(_ => Random.value).ToList(); // Сортування списку за рандомним числом
        int activeCount = Random.Range(1, tiles.Count);

        for (int i = 0; i < activeCount; i++)
        {
            shuffled[i]._isActive = true;
            shuffled[i].Updater();
        }
    }
    void Update()
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();
        Vector3 pos = rt.localPosition;
        pos.y -= speed * Time.deltaTime;
        rt.localPosition = pos;

        if(!GameManager.Instance.CheckGame())
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Trigger hit: {collision.gameObject.name}, tag: {collision.tag}");
        if (collision.CompareTag("Destructor"))
        {
            GameManager.Instance.plusPoint();
            Destroy(gameObject);
        }
    }
}
