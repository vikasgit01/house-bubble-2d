using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragandDropPrac : MonoBehaviour
{

    public GameObject grass;
    public GameObject sand;
    public GameObject water;

    public GameObject grassVisual;
    public GameObject sandVisual;
    public GameObject waterVisual;

    private GameObject _currentItem;
    private Vector3 _mousePoint;

    private void Awake()
    {
        grassVisual.SetActive(false);
        sandVisual.SetActive(false);
        waterVisual.SetActive(false);
    }
    private void Update()
    {
        _mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mousePoint.z = 0;

        this.transform.position = _mousePoint;
       
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _currentItem = grass;
            grassVisual.SetActive(true);
            sandVisual.SetActive(false);
            waterVisual.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _currentItem = sand;
            grassVisual.SetActive(false);
            sandVisual.SetActive(true);
            waterVisual.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _currentItem = water;
            grassVisual.SetActive(false);
            sandVisual.SetActive(false);
            waterVisual.SetActive(true);

        }

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(0))
        {
            _currentItem = null;
            grassVisual.SetActive(false);
            sandVisual.SetActive(false);
            waterVisual.SetActive(false);
        }

        if (_currentItem != null)
        {
            _currentItem.transform.position = _mousePoint;
        }
        else
        {
            return;
        }

        if (Input.GetMouseButtonUp(1))
        {
            Instantiate(_currentItem);
        }


    }

    public void Grass()
    {
        _currentItem = grass;
        grassVisual.SetActive(true);
        sandVisual.SetActive(false);
        waterVisual.SetActive(false);
    }

    public void Sand()
    {
        _currentItem = sand;
        grassVisual.SetActive(false);
        sandVisual.SetActive(true);
        waterVisual.SetActive(false);
    }

    public void Water()
    {
        _currentItem = water;
        grassVisual.SetActive(false);
        sandVisual.SetActive(false);
        waterVisual.SetActive(true);
    }
}
