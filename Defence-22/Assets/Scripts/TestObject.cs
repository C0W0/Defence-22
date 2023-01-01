using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
	// I'm using the test file as a spawner now
	[SerializeField]
	private GameObject testMonsterPrefab;

	[SerializeField]
	private int spawnNum;

	[SerializeField]
	private bool shouldSpawn;

	private int _spawnCounter;
	private float _timeSinceLastSpawn;

	void Awake()
	{
		_spawnCounter = 0;
		_timeSinceLastSpawn = -5f;
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	public void Update()
	{
		if (!shouldSpawn)
		{
			return;
		}
		
		if (_timeSinceLastSpawn > 0.5f && _spawnCounter < spawnNum)
		{
			Monster monster = MonsterManager.Instance.SpawnMonster(testMonsterPrefab, transform.position);
			monster.gameObject.name = $"Monster {_spawnCounter}";

			_timeSinceLastSpawn = 0f;
			_spawnCounter++;
		}
		else
		{
			_timeSinceLastSpawn += Time.deltaTime;
		}
	}
}
