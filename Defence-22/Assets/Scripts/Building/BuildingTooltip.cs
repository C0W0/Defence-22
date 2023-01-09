using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTooltip : MonoBehaviour
{
	public static BuildingTooltip Instance;

	public Button placeBtn, cancelBtn;

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

		placeBtn.onClick.AddListener(caller.Place);
		cancelBtn.onClick.AddListener(() =>
		{
			Destroy(caller.gameObject);
			HideTooltip();
		});
	}

	public void HideTooltip()
	{
		placeBtn.onClick.RemoveAllListeners();
		cancelBtn.onClick.RemoveAllListeners();
		
		gameObject.SetActive(false);
	}
}
