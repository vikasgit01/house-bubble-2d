using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using My.Utils;
using UnityEngine.SceneManagement;

//This script is on GameManager
public class UI : MonoBehaviour
{
    public static UI instance;


    //Player Health Bar
    public Slider healthBar;
    public Gradient healthBarGradient;
    public Image fillImage;

    [SerializeField] private GameObject panelBG;
    [SerializeField] private GameObject timerAnimationText;
    [SerializeField] private Animator waveAnimator;

    [SerializeField] private TextMeshProUGUI bombCountText;
    [SerializeField] private TextMeshProUGUI tntCountText;
    [SerializeField] private TextMeshProUGUI tnt2CountText;
    [SerializeField] private TextMeshProUGUI arrowCountText;

    [SerializeField] private Text storeBombText;
    [SerializeField] private Text storeTntText;
    [SerializeField] private Text storeTnt2Text;
    [SerializeField] private Text storeArrowsText;
    [SerializeField] private Text storeHealthText;

    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] tips;

    [SerializeField] private Animator levelOverPanelAnimator;
    [SerializeField] private Animator gameOverPanelAnimator;
    [SerializeField] private Animator clock;

    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject waveCompleted;
    [SerializeField] private GameObject goldMessage;

    private List<TextMeshPro> _textMeshPro = new List<TextMeshPro>();
    private int _storeBombCost = 30;
    private int _storeTntCost = 50;
    private int _storeTnt2Cost = 100;
    private int _storeArrowCost = 2;
    private int _storeHealthCost = 5;

    private int _storeBombCount;
    private int _storeTntCount;
    private int _storeTnt2Count;
    private int _storeArrowsCount;
    private int _storeHealthCount;

    private float _timer;
    private int _timerAnimationId;
    private int _waveCountAnimationId;
    private int _inventoryAnimaitonId;
    private int _clockAnimationId;
    private int _pausePanelAnimationId;
    private int _levelOverPanelAnimationId;
    private int _gameOverPanelAnimationId;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        Inventory.inventoryOpen += InventoryOpen;
        Inventory.inventoryClose += InventoryClose;
        Trap_Manager.bombCount += BombCountText;
        Trap_Manager.tntCount += TntCountText;
        Trap_Manager.tnt2Count += Tnt2CountText;
        BowShoot.arrowCount += ArrowCountText;
        Spawn_Manager.autoCloseInventory += InventoryClose;

    }

    private void Start()
    {
        Spawn_Manager.instance.onTimer += TimerAnimation;
        Spawn_Manager.instance.waveAnim += WaveCountAnimation;

        timerAnimationText.SetActive(false);
        _timerAnimationId = Animator.StringToHash("Timer");
        _waveCountAnimationId = Animator.StringToHash("Wave");
        _inventoryAnimaitonId = Animator.StringToHash("ShopOpen");
        _clockAnimationId = Animator.StringToHash("StartClock");
        _pausePanelAnimationId = Animator.StringToHash("Pause");
        _levelOverPanelAnimationId = Animator.StringToHash("Level");
        _gameOverPanelAnimationId = Animator.StringToHash("Game");

        panelBG.SetActive(false);
    }

    private void Update()
    {
        if (_timer <= 0)
        {
            timerAnimationText.SetActive(false);
        }

        MessageAnimation();
    }

    #region Player Health Bar
    public void SetMaxHealth(float health)
    {

        if (healthBar != null && fillImage != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;

            fillImage.color = healthBarGradient.Evaluate(1f);
        }
        else
        {
            return;
        }

    }

    public void SetHealthBar(float health)
    {
        if (healthBar != null && fillImage != null)
        {
            healthBar.value = health;
            fillImage.color = healthBarGradient.Evaluate(healthBar.normalizedValue);
        }
        else
        {
            return;
        }
    }
    #endregion


    private void TimerAnimation(float count)
    {
        _timer = count;
        timerAnimationText.SetActive(true);
        timerAnimationText.GetComponent<Text>().text = ((int)count).ToString();
        timerAnimationText.GetComponent<Animator>().SetTrigger(_timerAnimationId);
    }

    private void WaveCountAnimation()
    {
        waveAnimator.SetTrigger(_waveCountAnimationId);
    }

    private void InventoryOpen(Animator anim)
    {
        FindObjectOfType<Audio_Manager>().Play("InventoryOpen");
        anim.SetBool(_inventoryAnimaitonId, true);
    }

    private void InventoryClose(Animator anim)
    {
        FindObjectOfType<Audio_Manager>().Play("InventoryClose");
        anim.SetBool(_inventoryAnimaitonId, false);
    }

    private void BombCountText(int count)
    {
        bombCountText.text = count.ToString();
    }

    private void TntCountText(int count)
    {
        tntCountText.text = count.ToString();
    }

    private void Tnt2CountText(int count)
    {
        tnt2CountText.text = count.ToString();
    }

    private void ArrowCountText(int count)
    {
        arrowCountText.text = count.ToString();
    }

    private void BombStoreText()
    {
        storeBombText.text = _storeBombCount.ToString();
    }

    private void TntStoreText()
    {
        storeTntText.text = _storeTntCount.ToString();

    }

    private void Tnt2StoreText()
    {
        storeTnt2Text.text = _storeTnt2Count.ToString();

    }

    private void ArrowStoreText()
    {
        storeArrowsText.text = _storeArrowsCount.ToString();

    }

    private void HealthStoreText()
    {
        storeHealthText.text = _storeHealthCount.ToString();

    }

    public void SetStoreCountToZero()
    {
        _storeBombCount = 0;
        BombStoreText();
        _storeTntCount = 0;
        TntStoreText();
        _storeTnt2Count = 0;
        Tnt2StoreText();
        _storeArrowsCount = 0;
        ArrowStoreText();
        _storeHealthCount = 0;
        HealthStoreText();
    }

    public void ButtonSound()
    {
        FindObjectOfType<Audio_Manager>().Play("ButtonClick");
    }

    public void OpenLevelOverPanel()
    {
        StartCoroutine(levelOver());
    }

    IEnumerator levelOver()
    {
        yield return new WaitForSeconds(1.0f);
        Game_Manager.instance.SetCrossHairToNull();
        Player.Instance.canMove = false;
        panelBG.SetActive(true);
        levelOverPanelAnimator.SetBool(_levelOverPanelAnimationId, true);
    }

    public void OpenGameOverPanel()
    {
        int rand = Random.Range(0, tips.Length);

        tips[rand].SetActive(true);

        Player.Instance.canMove = false;
        panelBG.SetActive(true);
        gameOverPanelAnimator.SetBool(_gameOverPanelAnimationId, true);
        Game_Manager.instance.SetCrossHairToNull();
    }

    public void OpenPausePanel()
    {
        Game_Manager.instance.SetCrossHairToNull();
        panelBG.SetActive(true);
        pausePanel.gameObject.SetActive(true);
        Game_Manager.instance.escapeClicked = true;
        Time.timeScale = 0;
        Player.Instance.canMove = false;
    }

    public void ClosePausePanel()
    {
        Game_Manager.instance.SetCrossHairToShoot();
        panelBG.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        ButtonAnimation();
        Player.Instance.canMove = true;
        Time.timeScale = 1;
        Game_Manager.instance.escapeClicked = false;
    }

    private void ButtonAnimation()
    {
        foreach (GameObject item in buttons)
        {
            item.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    public void ResumeButton()
    {
        ButtonSound();
        Player.Instance.canShoot = true;
        Game_Manager.instance.escapeClicked = false;
        ClosePausePanel();
    }

    public void MenuButton()
    {
        ButtonAnimation();
        ButtonSound();
        foreach (var item in tips)
        {
            item.SetActive(false);
        }
        SceneManager.LoadScene(0);
    }

    public void ReplayButton()
    {
        ButtonAnimation();
        ButtonSound();

        foreach (var item in tips)
        {
            item.SetActive(false);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextButton()
    {
        ButtonAnimation();
        ButtonSound();
        SceneManager.LoadScene(SaveSystem.LoadGame().levelsCompleted + 1);
    }

    public void NextButtonForLevel01()
    {
        ButtonAnimation();
        ButtonSound();
        SceneManager.LoadScene(2);
    }
    public void NextButtonForLevel02()
    {
        ButtonAnimation();
        ButtonSound();
        SceneManager.LoadScene(3);
    }
    public void NextButtonForLevel03()
    {
        ButtonAnimation();
        ButtonSound();
        SceneManager.LoadScene(4);
    }
    public void NextButtonForLevel04()
    {
        ButtonAnimation();
        ButtonSound();
        SceneManager.LoadScene(0);
    }

    public void AddBombsButton()
    {
        ButtonSound();
        if (Game_Manager.instance.Gold() >= _storeBombCost)
        {
            _storeBombCount++;
            Game_Manager.instance.DecrementGold(_storeBombCost);
            Trap_Manager.instance.AddBomb(1);
            Message("-" + _storeBombCost);
            BombStoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage(" your gold is too low"));
        }
    }

    public void AddTNTsButton()
    {
        ButtonSound();
        if (Game_Manager.instance.Gold() >= _storeTntCost)
        {
            _storeTntCount++;
            Game_Manager.instance.DecrementGold(_storeTntCost);
            Trap_Manager.instance.AddTnt(1);
            Message("-" + _storeTntCost);
            TntStoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage(" your gold is too low"));
        }
    }

    public void AddTNT2sButton()
    {
        ButtonSound();
        if (Game_Manager.instance.Gold() >= _storeTnt2Cost)
        {
            _storeTnt2Count++;
            Game_Manager.instance.DecrementGold(_storeTnt2Cost);
            Trap_Manager.instance.AddTnt2(1);
            Message("-" + _storeTnt2Cost);
            Tnt2StoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage(" your gold is too low"));
        }
    }

    public void AddArrowsButton()
    {
        ButtonSound();
        if (Game_Manager.instance.Gold() >= _storeArrowCost)
        {
            if (BowShoot.instance.Arrows() < 30)
            {
                _storeArrowsCount++;
                Game_Manager.instance.DecrementGold(_storeArrowCost);
                BowShoot.instance.AddArrows(1);
                Message("-" + _storeArrowCost);
                ArrowStoreText();
            }
            else
            {
                StartCoroutine(GoldTooLowMessage("you have reach max arrow limit"));
            }
        }
        else
        {
            StartCoroutine(GoldTooLowMessage(" your gold is too low"));
        }
    }

    public void AddHealthButton()
    {
        ButtonSound();
        if (Game_Manager.instance.Gold() >= _storeHealthCost)
        {
            if (Player.Instance.playerHealth < 100)
            {
                _storeHealthCount++;
                Player.Instance.playerHealth += 10;
                SetHealthBar(Player.Instance.playerHealth);
                Game_Manager.instance.DecrementGold(_storeHealthCost);
                Message("-" + _storeHealthCost);
                HealthStoreText();
            }
            else
            {
                StartCoroutine(GoldTooLowMessage("your health is Full"));
            }
        }
        else
        {
            StartCoroutine(GoldTooLowMessage(" your gold is too low"));
        }
    }

    public void SubstractBombsButton()
    {
        ButtonSound();
        if (_storeBombCount > 0 && Trap_Manager.instance.Bomb() > 0)
        {
            _storeBombCount--;
            Game_Manager.instance.IncrementGold(_storeBombCost);
            Trap_Manager.instance.RemoveBomb(1);
            Message("+" + _storeBombCost);
            BombStoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage("you don't have anything to sell"));
        }
    }

    public void SubstractTNTsButton()
    {
        ButtonSound();
        if (_storeTntCount > 0 && Trap_Manager.instance.Tnt() > 0)
        {
            _storeTntCount--;
            Game_Manager.instance.IncrementGold(_storeTntCost);
            Trap_Manager.instance.RemoveTnt(1);
            Message("+" + _storeTntCost);
            TntStoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage("you don't have anything to sell"));
        }
    }

    public void SubstractTNT2sButton()
    {
        ButtonSound();
        if (_storeTnt2Count > 0 && Trap_Manager.instance.Tnt2() > 0)
        {
            _storeTnt2Count--;
            Game_Manager.instance.IncrementGold(_storeTnt2Cost);
            Trap_Manager.instance.RemoveTnt2(1);
            Message("+" + _storeTnt2Cost);
            Tnt2StoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage("you don't have anything to sell"));
        }
    }

    public void SubstractArrowsButton()
    {
        ButtonSound();
        if (_storeArrowsCount > 0 && BowShoot.instance.Arrows() > 0)
        {
            _storeArrowsCount--;
            Game_Manager.instance.IncrementGold(_storeArrowCost);
            BowShoot.instance.RemoveArrows(1);
            Message("+" + _storeArrowCost);
            ArrowStoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage("you don't have anything to sell"));
        }

    }

    public void SubstractHealthButton()
    {
        ButtonSound();
        if (_storeHealthCount > 0)
        {
            _storeHealthCount--;
            Player.Instance.playerHealth -= 10;
            SetHealthBar(Player.Instance.playerHealth);
            Game_Manager.instance.IncrementGold(_storeHealthCost);
            Message("+" + _storeHealthCost);
            HealthStoreText();
        }
        else
        {
            StartCoroutine(GoldTooLowMessage("you don't have anything to sell"));
        }
    }

    public IEnumerator GoldTooLowMessage(string message)
    {
        goldMessage.GetComponent<Text>().text = message;
        goldMessage.SetActive(true);
        yield return new WaitForSeconds(2f);
        goldMessage.SetActive(false);
    }


    public void ClockAnimationPlay()
    {
        clock.SetBool(_clockAnimationId, true);
    }

    public void ClockAnimationStop()
    {
        clock.SetBool(_clockAnimationId, false);
    }

    public IEnumerator WaveCompletedAnimation()
    {
        waveCompleted.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        waveCompleted.SetActive(false);
    }

    public void Message(string message)
    {
        TextMeshPro textMeshPro = Utils.CreateWorldTextMeshPro(this.transform, message, Player.Instance.transform.position, 5, Color.yellow, TextAlignmentOptions.Center, 5, "Other");
        _textMeshPro.Add(textMeshPro);
    }

    IEnumerator TextMesh(TextMeshPro t)
    {
        yield return new WaitForSeconds(0.8f);
        _textMeshPro.Remove(t);
    }

    private void MessageAnimation()
    {
        if (_textMeshPro.Count != 0)
        {
            foreach (TextMeshPro t in _textMeshPro)
            {
                t.gameObject.transform.Translate(Vector3.up * 2f * Time.deltaTime);
                StartCoroutine(TextMesh(t));
                Destroy(t.gameObject, 1f);

            }
        }
    }
}
