using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//This script to attached to Canvas gameobject

public class Main_Menu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject playMenuPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject controlePanel;
    [SerializeField] private GameObject helpPanel;

    [SerializeField] private Button continueButton;

    [SerializeField] private GameObject[] buttons;

    [SerializeField] private GameObject[] controlePanelPage;
    [SerializeField] private GameObject[] helpPanelPage;

    [SerializeField] private Image[] leftButton;
    [SerializeField] private Image[] rightButton;

   
    private int _controlesCurrentPage;
    private int _helpCurrentPage;


    private void Start()
    {
        Cursor.visible = true;
        mainMenuPanel.SetActive(true);
        settingPanel.SetActive(false);
        controlePanel.SetActive(false);
        helpPanel.SetActive(false);
        _controlesCurrentPage = 0;
    }

    private void Update()
    {
        if (SaveSystem.LoadGame() != null)
        {
            if (SaveSystem.LoadGame().levelsCompleted <= 0 || SaveSystem.LoadGame().levelsCompleted >= 4)
            {
                Color al = continueButton.image.color;
                al.a = 0.5f;
                continueButton.image.color = al;
            }
            else
            {
                Color al = continueButton.image.color;
                al.a = 1f;
                continueButton.image.color = al;
            }
        }
    }

    private void ButtonSound()
    {
        FindObjectOfType<Audio_Manager>().Play("ButtonClick");
    }

    private void ButtonAnimation()
    {
        foreach (GameObject item in buttons)
        {
            item.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void PlayButton()
    {
        ButtonSound();
        ButtonAnimation();
        playMenuPanel.SetActive(true);
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        controlePanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void SettingButton()
    {
        ButtonSound();
        ButtonAnimation();
        settingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        playMenuPanel.SetActive(false);
        controlePanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void ExitButton()
    {
        ButtonSound();
        ButtonAnimation();
        Application.Quit();
    }

    public void ControlButton()
    {
        ButtonSound();
        ButtonAnimation();
        controlePanel.SetActive(true);
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        playMenuPanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void HelpButton()
    {
        ButtonSound();
        ButtonAnimation();
        helpPanel.SetActive(true);
        controlePanel.SetActive(false);
        settingPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        playMenuPanel.SetActive(false);
    }

    public void SettingsBackButton()
    {
        ButtonSound();
        ButtonAnimation();
        mainMenuPanel.SetActive(true);
        controlePanel.SetActive(false);
        settingPanel.SetActive(false);
        helpPanel.SetActive(false);
        playMenuPanel.SetActive(false);
    }

    public void ContinueButton()
    {
        ButtonSound();
        if (SaveSystem.LoadGame().levelsCompleted > 0 && SaveSystem.LoadGame().levelsCompleted < 4)
        {
            Debug.Log("Continue Pressed");
            ButtonAnimation();
            SceneManager.LoadScene(SaveSystem.LoadGame().levelsCompleted + 1);
        }
    }

    public void NewGameButton()
    {
        ButtonSound();
        ButtonAnimation();
        if (Data_Manager.instance.PlayingFirstTime())
        {
            Data_Manager.instance.SetPlayingFirstTime(false);
        }
        SceneManager.LoadScene(1);
    }

    public void PlayBackButton()
    {
        ButtonSound();
        ButtonAnimation();
        mainMenuPanel.SetActive(true);
        playMenuPanel.SetActive(false);
        settingPanel.SetActive(false);
        controlePanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void SmallBackButton()
    {
        ButtonSound();
        ButtonAnimation();
        controlePanelPage[0].SetActive(true);
        _controlesCurrentPage = 0;
        foreach (var item in controlePanelPage)
        {
            if (item != controlePanelPage[0])
            {
                item.SetActive(false);
            }
        }

        helpPanelPage[0].SetActive(true);
        _helpCurrentPage = 0;
        foreach (var item in helpPanelPage)
        {
            if (item != helpPanelPage[0])
            {
                item.SetActive(false);
            }
        }

        settingPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
        controlePanel.SetActive(false);
        helpPanel.SetActive(false);
    }

    public void ControlsPanelNextPage()
    {
        ButtonSound();
        if (_controlesCurrentPage < controlePanelPage.Length - 1)
        {
            _controlesCurrentPage++;
            controlePanelPage[_controlesCurrentPage].SetActive(true);

            foreach (GameObject item in controlePanelPage)
            {
                if (item != controlePanelPage[_controlesCurrentPage])
                {
                    item.SetActive(false);
                }
                /*else
                {
                    Color rb = rightButton[0].color;
                    rb.a = 0.5f;
                    rightButton[0].color = rb;
                    Color lb = leftButton[0].color;
                    lb.a = 1f;
                    leftButton[0].color = lb;
                }*/
            }
        }
    }

    public void ControlsPanelPreviousPage()
    {
        ButtonSound();
        if (_controlesCurrentPage > 0)
        {
            _controlesCurrentPage--;
            controlePanelPage[_controlesCurrentPage].SetActive(true);
            foreach (GameObject item in controlePanelPage)
            {
                if (item != controlePanelPage[_controlesCurrentPage])
                {

                    item.SetActive(false);
                }
                /* else
                 {
                     Color al = leftButton[0].color;
                     al.a = 0.5f;
                     leftButton[0].color = al;
                     Color rb = rightButton[0].color;
                     rb.a = 1f;
                     rightButton[0].color = rb;
                 }*/
            }
        }
    }

    public void HelpPanelNextPage()
    {
        ButtonSound();
        if (_helpCurrentPage < helpPanelPage.Length - 1)
        {
            _helpCurrentPage++;
            helpPanelPage[_helpCurrentPage].SetActive(true);

            foreach (GameObject item in helpPanelPage)
            {
                if (item != helpPanelPage[_helpCurrentPage])
                {

                    item.SetActive(false);
                }
                /*else
                {
                    Color rb = rightButton[1].color;
                    rb.a = 0.5f;
                    rightButton[1].color = rb;
                    Color lb = leftButton[1].color;
                    lb.a = 1f;
                    leftButton[1].color = lb;
                }*/
            }
        }
    }

    public void HelpPanelPreviousPage()
    {
        ButtonSound();
        if (_helpCurrentPage > 0)
        {
            _helpCurrentPage--;
            helpPanelPage[_helpCurrentPage].SetActive(true);

            foreach (GameObject item in helpPanelPage)
            {
                if (item != helpPanelPage[_helpCurrentPage])
                {

                    item.SetActive(false);
                }
                /*else
                {
                    Color lb = leftButton[1].color;
                    lb.a = 0.5f;
                    leftButton[1].color = lb;
                    Color rb = rightButton[1].color;
                    rb.a = 1f;
                    rightButton[1].color = rb;
                }*/
            }
        }
    }

}
