using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//This script is attached to SpawnManager GameObject

public class Spawn_Manager : MonoBehaviour
{
    public static Spawn_Manager instance;

    public static event Action<Animator> autoCloseInventory;

    [SerializeField] private List<Transform> spawnPoint = new List<Transform>();
    [SerializeField] private Transform enemy;
    [SerializeField] private Text timerText;
    [SerializeField] private Text waveCounterText;
    [SerializeField] private Text preparationPhaseText;

    [Header("Wave Spawn Settings")]
    [Space(20)]


    [SerializeField] private float preparationPhaseTime = 5.0f;
    [SerializeField] private float timerTickSound = 4.0f;
    [SerializeField] private int noOfEnemyAtSpawn = 5;
    [SerializeField] private Wave[] wave;

    [HideInInspector] public float _currentPhaseTime;

    private int totalNoofWaves;
    private int _nextWave;
    private int _currentEnemyNo;
    private int _enemyKilled;
    private bool _preparationPhase;
    private bool _startSpawn;
    private int _totalnoofEnemy;
    private bool _shown = false;

    public event Action<float> onTimer;
    public event Action waveAnim;
    public event Action levelCompleted;

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
        totalNoofWaves = wave.Length;
        _nextWave = 0;
        waveCounterText.text = "Wave : " + (_nextWave + 1).ToString();
        _startSpawn = true;
        _currentPhaseTime = preparationPhaseTime;
    }

    private void Update()
    {

        #region _preparationPhase 
        if (_currentPhaseTime > 0)
        {
            Game_Manager.instance.SetCrossHairToNull();
            UI.instance.ClockAnimationPlay();
            preparationPhaseText.gameObject.SetActive(true);
            waveCounterText.gameObject.SetActive(false);
            Player.Instance.canShoot = false;
            Inventory.instance.canOpen = true;
            Inventory.instance.canPlaceTrap = true;
            _preparationPhase = true;
            _currentPhaseTime -= Time.deltaTime;
            timerText.text = ((int)_currentPhaseTime).ToString();

            if (_currentPhaseTime <= timerTickSound)
            {
                onTimer?.Invoke(_currentPhaseTime);
            }
        }
        else
        {
            preparationPhaseText.gameObject.SetActive(false);
            waveCounterText.gameObject.SetActive(true);
            _preparationPhase = false;
        }
        #endregion

        #region Spawn min enemy at the start
        if (!_preparationPhase && _startSpawn)
        {
            StartSpawnEnemy();
        }
        #endregion

        #region Spawning Next Wave
        NextWave();
        #endregion

        #region LevelCompleted

        if (_nextWave == totalNoofWaves - 1 && _enemyKilled == wave[_nextWave].noofenemies)
        {
            if (_shown == false)
            {
                StartCoroutine(UI.instance.GoldTooLowMessage("Collect all the gold to complete the level"));
                _shown = true;
            }

            if (Game_Manager.instance.coinCollected == _totalnoofEnemy)
            {
                Debug.Log("LevelCOmpleted");
                levelCompleted?.Invoke();
            }
        }
        #endregion

    }

    private void StartSpawnEnemy()
    {
        Game_Manager.instance.SetCrossHairToShoot();
        UI.instance.ClockAnimationStop();
        Player.Instance.canShoot = true;
        Player.Instance.canMove = true;
        Inventory.instance.canOpen = false;
        Inventory.instance.canPlaceTrap = false;
        UI.instance.SetStoreCountToZero();
        if (Inventory.instance.isOpen)
        {
            autoCloseInventory?.Invoke(Inventory.instance.inventoryAnimator);
            Inventory.instance.isOpen = false;
        }

        int[] randnums = new int[noOfEnemyAtSpawn];
        int rand;

        waveAnim?.Invoke();

        for (int i = 0; i < noOfEnemyAtSpawn; i++)
        {
            rand = UnityEngine.Random.Range(0, spawnPoint.Count);

            while (randnums.Contains(rand))
            {
                rand = UnityEngine.Random.Range(0, spawnPoint.Count);
            }
            randnums[i] = rand;

            Instantiate(enemy, spawnPoint[rand].position, Quaternion.identity);
            _currentEnemyNo++;
        }

        _startSpawn = false;
    }

    public void SpawnEnemy(Wave wave)
    {
        if (_currentEnemyNo < wave.noofenemies)
        {
            int randnum = UnityEngine.Random.Range(0, spawnPoint.Count);
            Instantiate(enemy, spawnPoint[randnum].position, Quaternion.identity);
            _currentEnemyNo += 1;
        }
    }

    private void NextWave()
    {
        if (_nextWave < totalNoofWaves - 1)
        {
            if (wave[_nextWave].noofenemies == _currentEnemyNo && _enemyKilled == _currentEnemyNo)
            {
                StartCoroutine(UI.instance.WaveCompletedAnimation());
                _enemyKilled = 0;
                _currentEnemyNo = 0;
                _currentPhaseTime = preparationPhaseTime;
                _startSpawn = true;
                _nextWave++;
                waveCounterText.text = "Wave : " + (_nextWave + 1).ToString();
            }
        }
    }

    public void EnemiesKilled()
    {
        SpawnEnemy(wave[_nextWave]);
        _enemyKilled++;
        _totalnoofEnemy++;
    }
}


