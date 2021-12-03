using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField] private Text TextFinalTime;
    [SerializeField] private Text BestTime;
    [SerializeField] private AudioClip ButtonSound;

    private AudioSource Source;
    private void Start()
    {
        Source = GetComponent<AudioSource>();
        
        string finalTime = PlayerManager.Instance.GetFinalTime;
        string prevScene = Transition.Instance.PreviousScene;
        TextFinalTime.text = finalTime;
        
        if (PlayerPrefs.HasKey($"PB_{prevScene}"))
        {
            if (ConvertTime(PlayerPrefs.GetString($"PB_{prevScene}")) > ConvertTime(finalTime))
            {
                PlayerPrefs.SetString($"PB_{prevScene}", finalTime);
                BestTime.text = finalTime;
            }
            else
            {
                BestTime.text = PlayerPrefs.GetString($"PB_{prevScene}");
            }

        }
        else
        {
            PlayerPrefs.SetString($"{prevScene}", finalTime);
            BestTime.text = finalTime;
        }
        
        PlayerPrefs.Save();
        
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void BackTOMain()
    {
        Source.PlayOneShot(ButtonSound);
        Transition.Instance.ChangeScene("MainMenu 1");
    }
    
    private float ConvertTime(string time)
    {
        string temp = time;

        string[] array = temp.Split(':');

        int min = int.Parse(array[0]);
        
        int sec = int.Parse(array[1]);

        int frac = int.Parse(array[2]);

        float resultInSec = min * 60 + sec + frac / 1000;
        
        Debug.Log(min);
        Debug.Log(sec);
        Debug.Log(frac);

        return resultInSec;
        


    }
}
