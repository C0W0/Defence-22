using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopBuildingBtn : MonoBehaviour
{
	[SerializeField]
	private GameObject buildingPrefab;
	[SerializeField]
	private TextMeshProUGUI nameText;

	void Awake()
	{
		Tower tower = buildingPrefab.GetComponent<Tower>();
		GetComponent<Image>().sprite = tower.icon;
		nameText.text = tower.towerName;
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
    public void OnBtnClick()
    {
        if (CurrencySystem.Instance.currencyAmount >= buildingPrefab.GetComponent<Tower>().cost)
        {
            ShopSystem.Instance.CreateBuildingDrag(buildingPrefab);
        }
    }
}
