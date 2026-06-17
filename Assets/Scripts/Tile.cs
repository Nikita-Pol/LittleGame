using UnityEngine;

[System.Serializable]
public class Tile
{
    public bool _isActive;
    //public int _index;
    public GameObject TileObject;

    public void Updater()
    {
        if(_isActive)
            TileObject.SetActive(true);
        else
            TileObject.SetActive(false);
    }
}
