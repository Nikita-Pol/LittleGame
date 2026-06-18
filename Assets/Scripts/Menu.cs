using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static GameManager;

public class Menu : MonoBehaviour
{
    public GameObject MenuPanel;

    public TextMeshProUGUI fasttext;
    public TextMeshProUGUI normaltext;
    public TextMeshProUGUI slowtext;

    public Sprite UnmutedIcon;
    public Sprite MutedIcon;
    private bool _isMuted = false;
    public Image Sounds;
    public AudioSource audiosource;

    double fastrecord;
    double normalrecord;
    double slowrecord;

    private void Start()
    {
        LoadRecords();
        audiosource.volume = PlayerPrefs.GetFloat("Volume", 0.25f);
        if (audiosource.volume > 0)
            _isMuted = false;
        else 
            _isMuted = true;
    }


    public void LoadRecords()
    {
        string record = PlayerPrefs.GetString("record_" + GameMode.Fast, "0");
        if (record != "")
            fastrecord = double.Parse(record);
        record = PlayerPrefs.GetString("record_" + GameMode.Normal, "0");
        if (record != "")
            normalrecord = double.Parse(record);
        record = PlayerPrefs.GetString("record_" + GameMode.Slow, "0");
        if (record != "")
            slowrecord = double.Parse(record);
    }

    private void Update()
    {
        fasttext.text = $"Fast Mode\r\n<size=16>\r\n<color=#FF7D7D>Record:{fastrecord}";
        normaltext.text = $"Normal Mode\r\n<size=16>\r\n<color=#B8FFA6>Record:{normalrecord}";
        slowtext.text = $"Slow Mode\r\n<size=16>\r\n<color=#C6FCFF>Record:{slowrecord}";

        if (_isMuted)
        {
            Sounds.sprite = MutedIcon;
            audiosource.volume = 0;
            PlayerPrefs.SetFloat("Volume", audiosource.volume);
        }
        else
        {
            Sounds.sprite = UnmutedIcon;
            audiosource.volume = 0.25f;
            PlayerPrefs.SetFloat("Volume", audiosource.volume);
        }
    }

    public void Muted()
    {
        _isMuted = !_isMuted;
    }
}
