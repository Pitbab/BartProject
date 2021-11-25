using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private Slider ManaSliderRight;
    [SerializeField] private Slider ManaSliderLeft;
    [SerializeField] private Slider StaminaSlider;
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject RedIndicatorMana;
    [SerializeField] private GameObject SelectedPower;
    [SerializeField] private List<Sprite> PowerImages;
    [SerializeField] private Text Tutorial;
    [SerializeField] private Text Indicator;
    [SerializeField] private Text TimerText;
    [SerializeField] private GameObject PauseMenu;

    [SerializeField] private Text TimerMin;
    [SerializeField] private Text TimerSec;
    [SerializeField] private Text TimerFrac;

    private Image PowerSelected;
    private Telekinesis Mana;
    private int index;
    private float Timer = 0;
    private bool InMenu = false;
    private const float NormalTimeScale = 1.0f;
    private const float MenuTimeScale = 0.0f;

    private void Start()
    {

        PlayerManager.Instance.RegisterPlayerUi(this);
        PowerSelected = SelectedPower.GetComponent<Image>();
        Mana = Player.GetComponent<Telekinesis>();

        PlayerManager.Instance.OnStaminaChanged += OnStaminaChanged;
        PlayerManager.Instance.OnHealthChanged += OnHealthChanged;
        Timer = 0;
        TimerText.text = "0 : 00";
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnHealthChanged(float health)
    {
        HealthSlider.value = health;
    }

    private void OnStaminaChanged(float stamina)
    {
        StaminaSlider.value = stamina;
    }
    
    private void Update()
    {
        
        UpdateUiValues();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ChangeMenuState();
        }

    }

    private void UpdateUiValues()
    {
        UpdateTimer();
        
        //set sliders values
        ManaSliderRight.value = Mana.CurrentMana;
        ManaSliderLeft.value = Mana.CurrentMana;
        index = Mana.ModeIndex;
        
        //use the index of powers being used in the telekinesis script
        PowerSelected.sprite = PowerImages[index];
        
        //exclamation mark for incoming bullets
        Indicator.enabled = Mana.CheckForBullets();
    }

    public void ChangeMenuState()
    {
        //activate menu mode or deactivate it
        InMenu = !InMenu;
        PauseMenu.SetActive(!PauseMenu.activeInHierarchy);
        ChangeCursorMode();
        ChangeTimeScale();
        PlayerManager.Instance.ChangeMenuState();
    }

    private void ChangeCursorMode()
    {
        if(InMenu)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void ChangeTimeScale()
    {
        if (Time.timeScale == NormalTimeScale)
        {
            Time.timeScale = MenuTimeScale;
        }
        else
        {
            Time.timeScale = NormalTimeScale;
        }
    }

    private void UpdateTimer()
    {
        Timer += Time.deltaTime;
        
        int intTime = (int)Timer;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        float fraction = Timer * 1000;
        fraction = (fraction % 1000);

        TimerMin.text = String.Format("{0:00}", minutes);
        TimerSec.text = String.Format("{0:00}", seconds);
        TimerFrac.text = String.Format("{0:000}", fraction);
    }

    public void NotEnoughMana()
    {
        StartCoroutine(IndicatorMana());
    }

    private IEnumerator IndicatorMana()
    {
        //create red flash when mana is completely used 
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            RedIndicatorMana.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            RedIndicatorMana.SetActive(false);
        }
    }

    public void BackToMain()
    {
        Time.timeScale = NormalTimeScale;
        PlayerManager.Instance.ChangeMenuState();
        SceneManager.LoadScene("MainMenu 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public string GetFinalTime()
    {
        string result =  $"{TimerMin.text} : {TimerSec.text} : {TimerFrac.text}";
        return result;
    }

}
