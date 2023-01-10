using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthSystem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI healthText;

    private int _healthAmount;

    public static HealthSystem Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateHealth(int change)
    {
        _healthAmount += change;
        healthText.text = _healthAmount.ToString();
    }
}
