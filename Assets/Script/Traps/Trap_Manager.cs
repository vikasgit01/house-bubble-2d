using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using My.Utils;
using TMPro;
using UnityEngine.UI;
using System;

//This script is attached to GameManager GameObject

public class Trap_Manager : MonoBehaviour
{
    public static Trap_Manager instance;

    public static event Action<int> bombCount;
    public static event Action<int> tntCount;
    public static event Action<int> tnt2Count;

    [SerializeField] private Traps_SO[] trapsSo;
    [SerializeField] private Image[] trapButtonSprite;
    [SerializeField] private Transform trapSprite;
    [SerializeField] private Transform trapRangeSprite;
    [SerializeField] private LayerMask cantPlaceTraps;


    [SerializeField] private Image noBombs;
    [SerializeField] private Image noTnts;
    [SerializeField] private Image noTnt2s;

    private int _totalBomb;
    private int _totalTnt;
    private int _totalTnt2;

    private List<TextMeshPro> _textMeshPro = new List<TextMeshPro>();
    private SpriteRenderer _trapVisual;
    private SpriteRenderer _trapRangeVisual;
    private Traps_SO _activeTrap;
    private int _trapRange;
    private Vector3 _mousePoint;

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

        _trapVisual = trapSprite.GetComponent<SpriteRenderer>();
        _trapRangeVisual = trapRangeSprite.GetComponent<SpriteRenderer>();

    }

    private void Start()
    {
        Image[] image = { trapButtonSprite[0], trapButtonSprite[1], trapButtonSprite[2] };
        DeselectTrap(image);

        bombCount?.Invoke(_totalBomb);
        tntCount?.Invoke(_totalTnt);
        tnt2Count?.Invoke(_totalTnt2);

    }

    private void Update()
    {
        #region Transform of visual to mousepoint

        _mousePoint = Utils.GetMouseWorldPosition();
        trapSprite.transform.position = _mousePoint;

        #endregion

        MessageAnimation();
        SelectingTraps();
        PlaceTrap();
        ZeroTrapsEffect();
    }

    private void PlaceTrap()
    {
        if (Input.GetMouseButtonDown(1) && Inventory.instance.canPlaceTrap)
        {
            if (_activeTrap != null)
            {
                if (CanSpwanTrap(_activeTrap, _mousePoint))
                {
                    FindObjectOfType<Audio_Manager>().Play("TrapPlaced");
                    DecrementTrapNumber(_activeTrap);
                    Instantiate(_activeTrap.prefab, _mousePoint, Quaternion.identity);
                }
                else
                {
                    CantPlaceTrapMessage("Can't place Trap");
                }
            }
            else
            {
                CantPlaceTrapMessage("Trap is not selected");
            }
            TrapsAvailability();

        }
    }

    private void DecrementTrapNumber(Traps_SO trap)
    {
        if (trapsSo[0] == trap)
        {
            _totalBomb--;
            bombCount?.Invoke(_totalBomb);
        }
        else if (trapsSo[1] == trap)
        {
            _totalTnt--;
            tntCount?.Invoke(_totalTnt);
        }
        else if (trapsSo[2] == trap)
        {
            _totalTnt2--;
            tnt2Count?.Invoke(_totalTnt2);
        }
    }

    private void TrapsAvailability()
    {

        if (_activeTrap == trapsSo[0])
        {
            
            if (_totalBomb <= 0)
            {
                DeselectingTrap();
                _activeTrap = null;
            }

        }
        else if (_activeTrap == trapsSo[1])
        {
           
            if (_totalTnt <= 0)
            {
                DeselectingTrap();
                _activeTrap = null;
            }
        }
        else if (_activeTrap == trapsSo[2])
        { 
            if (_totalTnt2 <= 0)
            {
                DeselectingTrap();
                _activeTrap = null;
            }
        }

    }
    private void CantPlaceTrapMessage(string message)
    {
        Vector2 _mousePoint = Utils.GetMouseWorldPosition();

        TextMeshPro textMeshPro = Utils.CreateWorldTextMeshPro(this.transform, message, _mousePoint, 5, Color.white, TextAlignmentOptions.Center, 5, "Other");
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
    private Traps_SO SelectingTraps()
    {
        if (Inventory.instance.canPlaceTrap)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                BombSelection();
                return _activeTrap;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TNT1Selection();
                return _activeTrap;
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                TNT2Selection();
                return _activeTrap;
            }
            if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetMouseButtonDown(0)) && _activeTrap != null)
            {
                DeselectingTrap();
                return _activeTrap = null;
            }
        }
        else
        {
            DeselectingTrap();
        }
        return null;
    }
    private void DeselectingTrap()
    {
        _trapVisual.sprite = null;
        _trapRangeVisual.sprite = null;
        Image[] image = { trapButtonSprite[0], trapButtonSprite[1], trapButtonSprite[2] };
        DeselectTrap(image);

    }
    private bool CanSpwanTrap(Traps_SO trapSo, Vector3 position)
    {
        CircleCollider2D trapCircleCollider2D = trapSo.prefab.GetComponent<CircleCollider2D>();

        if (Physics2D.OverlapCircle(position + (Vector3)trapCircleCollider2D.offset, trapCircleCollider2D.radius, cantPlaceTraps) != null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    private void SelectTrap(Image sp)
    {
        Color al = sp.color;
        al.a = 1;
        sp.color = al;
    }
    private void DeselectTrap(Image[] sp)
    {
        foreach (Image i in sp)
        {
            Color al = i.color;
            al.a = 0;
            i.color = al;
        }
    }
    private void ZeroTrapsEffect()
    {
        if (_totalBomb <= 0)
        {
            Color al = noBombs.color;
            al.a = 0.5f;
            noBombs.color = al;
        }
        else
        {
            Color al = noBombs.color;
            al.a = 0f;
            noBombs.color = al;
        }
        if (_totalTnt <= 0)
        {
            Color al = noTnts.color;
            al.a = 0.5f;
            noTnts.color = al;
        }
        else
        {
            Color al = noTnts.color;
            al.a = 0f;
            noTnts.color = al;
        }
        if (_totalTnt2 <= 0)
        {
            Color al = noTnt2s.color;
            al.a = 0.5f;
            noTnt2s.color = al;
        }
        else
        {
            Color al = noTnt2s.color;
            al.a = 0f;
            noTnt2s.color = al;
        }
    }

    public void BombSelection()
    {
        if (_totalBomb > 0)
        {
            _trapVisual.sprite = trapsSo[0].sprite;
            _trapRangeVisual.sprite = trapsSo[0].rangeSprite;
            _trapRange = trapsSo[0].range;
            trapRangeSprite.localScale = new Vector3(_trapRange, _trapRange, _trapRange);
            _activeTrap = trapsSo[0];
            SelectTrap(trapButtonSprite[0]);
            Image[] images = { trapButtonSprite[1], trapButtonSprite[2] };
            DeselectTrap(images);
        }
    }
    public void TNT1Selection()
    {
        if (_totalTnt > 0)
        {
            _trapVisual.sprite = trapsSo[1].sprite;
            _trapRangeVisual.sprite = trapsSo[1].rangeSprite;
            _trapRange = trapsSo[1].range;
            trapRangeSprite.localScale = new Vector3(_trapRange, _trapRange, _trapRange);
            _activeTrap = trapsSo[1];
            SelectTrap(trapButtonSprite[1]);
            Image[] images = { trapButtonSprite[0], trapButtonSprite[2] };
            DeselectTrap(images);
        }

    }
    public void TNT2Selection()
    {
        if (_totalTnt2 > 0)
        {
            _trapVisual.sprite = trapsSo[2].sprite;
            _trapRangeVisual.sprite = trapsSo[2].rangeSprite;
            _trapRange = trapsSo[2].range;
            trapRangeSprite.localScale = new Vector3(_trapRange, _trapRange, _trapRange);
            _activeTrap = trapsSo[2];
            SelectTrap(trapButtonSprite[2]);
            Image[] images = { trapButtonSprite[0], trapButtonSprite[1] };
            DeselectTrap(images);
        }

    }

    public int Bomb()
    {
        return _totalBomb;
    }
    public int Tnt()
    {
        return _totalTnt;
    }
    public int Tnt2()
    {
        return _totalTnt2;
    }
    public void AddBomb(int addedBomb)
    {
        _totalBomb += addedBomb;
        bombCount?.Invoke(_totalBomb);
    }
    public void AddTnt(int addedTnt)
    {
        _totalTnt += addedTnt;
        tntCount?.Invoke(_totalTnt);
    }
    public void AddTnt2(int addedTnt2)
    {
        _totalTnt2 += addedTnt2;
        tnt2Count?.Invoke(_totalTnt2);
    }

    public void RemoveBomb(int removedBomb)
    {
        _totalBomb -= removedBomb;
        bombCount?.Invoke(_totalBomb);
    }
    public void RemoveTnt(int removedTnt)
    {
        _totalTnt -= removedTnt;
        tntCount?.Invoke(_totalTnt);
    }

    public void RemoveTnt2(int removedTnt2)
    {
        _totalTnt2 -= removedTnt2;
        tnt2Count?.Invoke(_totalTnt2);
    }
}
