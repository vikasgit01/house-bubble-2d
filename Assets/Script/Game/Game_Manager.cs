using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using My.Utils;
//This script is attached to GameManager GameObject

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;

    [SerializeField] private GameObject stackofGold;
    [SerializeField] private Text goldCountText;
    [SerializeField] private GameObject trapEffect;
    [SerializeField] private GameObject effectDamage;
    [SerializeField] private Transform crossHair;
    [SerializeField] private Sprite shootCrossHair;

    private int _totalgold;
    private int _levelsCompleted;
    private bool _incremented;

    [HideInInspector] public bool escapeClicked;
    [HideInInspector] public int coinCollected;

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

    }

    private void Start()
    {
        if (FindObjectOfType<Level_01>() != null)
        {
            Debug.Log("SetGameData");
            Data_Manager.instance.SetGameDataToZero();
        }
        else
        {
            Debug.Log("LoadGameData");
           Data_Manager.instance.LoadData();
        }

        _incremented = false;
        SetCrossHairToNull();
        Spawn_Manager.instance.levelCompleted += LevelCompleted;
        Player.Instance.gameOver += GameOver;
        House_01.instance.gameOver += GameOver;
        goldCountText.text = _totalgold.ToString();
    }

    private void Update()
    {
        Crosshair();
        Pause();
    }

    public int TotalLevelsCompleted()
    {
        return _levelsCompleted;
    }

    public int TotalGold()
    {
        return _totalgold;
    }

    public int TotalArrows()
    {
        return BowShoot.instance.Arrows();
    }

    public int TotalBomb()
    {
        return Trap_Manager.instance.Bomb();
    }

    public int TotalTNT1()
    {
        return Trap_Manager.instance.Tnt();
    }

    public int TotalTNT2()
    {
        return Trap_Manager.instance.Tnt2();
    }

    private void Crosshair()
    {
        Vector2 mospos = Utils.GetMouseWorldPosition();
        crossHair.transform.position = mospos;
    }

    public void SetCrossHairToNull()
    {
        Cursor.visible = true;
        crossHair.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = null;
    }

    public void SetCrossHairToShoot()
    {
        Cursor.visible = false;
        crossHair.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = shootCrossHair;
    }

    public void Pause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && escapeClicked == false)
        {
            UI.instance.OpenPausePanel();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && escapeClicked == true)
        {
            Player.Instance.canShoot = true;
            escapeClicked = false;
            UI.instance.ClosePausePanel();
        }
    }


    public void TrapEffect(Transform pos)
    {
        Instantiate(trapEffect, pos.position, pos.rotation);
    }

    public void EffectDamage(Transform pos)
    {
        Instantiate(effectDamage, pos.position, pos.rotation);
    }

    public void CoinSpawner(Transform pos)
    {
        Instantiate(stackofGold, pos.position, Quaternion.identity);
    }

    private void LevelCompleted()
    {
        if (!_incremented)
        {
            IncrementLevelCompleted();
            _incremented = true;
        }

        Data_Manager.instance.SaveData();
        UI.instance.OpenLevelOverPanel();
    }

    public void GameOver()
    {
        UI.instance.OpenGameOverPanel();
    }

    public void LevelCompletedToValue(int level)
    {
        _levelsCompleted = level;
    }

    public void IncrementLevelCompleted()
    {
        _levelsCompleted++;
    }

    public void IncrementGold(int goldAmount)
    {
        _totalgold += goldAmount;
        goldCountText.text = _totalgold.ToString();
    }

    public void DecrementGold(int goldAmount)
    {
        _totalgold -= goldAmount;
        goldCountText.text = _totalgold.ToString();
    }

    public int Gold()
    {
        return _totalgold;
    }
}
