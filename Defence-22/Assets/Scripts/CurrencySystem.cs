using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencySystem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currencyText;

    private int _currencyAmount;

    public static CurrencySystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCurrency(int change)
    {
        _currencyAmount += change;
        currencyText.text = _currencyAmount.ToString();
    }
}
