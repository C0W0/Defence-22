using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModifyTooltip : MonoBehaviour
{
    public static ModifyTooltip Instance;

    public Button sellBtn;

    private Camera _uiCamera;

    private void Awake()
    {
        Instance = this;
        _uiCamera = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
		
    }

    // Update is called once per frame
    void Update()
    {
		
    }

    public void ShowTooltip(BuildingPlaceable caller)
    {
        gameObject.SetActive(true);
        transform.position = _uiCamera.WorldToScreenPoint(caller.transform.position);

        sellBtn.onClick.AddListener(() =>
        {
            BuildingSystem.Instance.RemoveBuilding(caller.transform.position);
            Destroy(caller.gameObject);
            int sellCost = caller.cost * 2 / 3;
            CurrencySystem.Instance.UpdateCurrency(sellCost);
            HideTooltip();
        });
    }

    public void HideTooltip()
    {
        sellBtn.onClick.RemoveAllListeners();
		
        gameObject.SetActive(false);
    }
}