using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SubWave
{
	public GameObject monsterPrefab;
	public int spawnNum;
	public float spawnCd;

	public SubWave(float spawnCd, string monsterName, int spawnNum)
	{
		this.spawnCd = spawnCd;
		monsterPrefab = MonsterManager.Instance.monsterPrefabs[monsterName];
		this.spawnNum = spawnNum;
	}
}

public class Wave
{
	private List<SubWave> _subWaves;
	private int _currSubWaveIndex;
	private float _timeSinceLastSpawn;
	public float waveCd;

	public Wave(float waveCd)
	{
		this.waveCd = waveCd;
		_subWaves = new List<SubWave>();
		_currSubWaveIndex = 0;
		_timeSinceLastSpawn = 0f;
	}

	public void SpawnMonsters(Vector2 position)
	{
		if (_currSubWaveIndex == _subWaves.Count)
		{
			return;
		}
		
		SubWave currSubWave = _subWaves[_currSubWaveIndex];
		_timeSinceLastSpawn += Time.deltaTime;

		if (_timeSinceLastSpawn >= currSubWave.spawnCd)
		{
			MonsterManager.Instance.SpawnMonster(currSubWave.monsterPrefab, position);
			_timeSinceLastSpawn = 0;
			currSubWave.spawnNum--;
			_subWaves[_currSubWaveIndex] = currSubWave;
		}

		if (currSubWave.spawnNum == 0)
		{
			_currSubWaveIndex++;
		}
	}

	public void AddSubWave(float spawnCd, string monsterName, int spawnNum)
	{
		_subWaves.Add(new SubWave(spawnCd, monsterName, spawnNum));
	}
}

public class Spawner : MonoBehaviour
{
	[SerializeField]
	private string spawnerTag;

	private List<Wave> _waves;
	private int _waveIndex;
	private float _timeSinceLastWave;

	private void Awake()
	{
		_waves = new List<Wave>();
		_waveIndex = 0;
		_timeSinceLastWave = 0f;
	}

	// Start is called before the first frame update
	void Start()
	{
		LoadSpawner();
	}

	private void LoadSpawner()
	{
		try
		{
			string path = $"Spawner/{spawnerTag}";
			TextAsset textAsset = Resources.Load<TextAsset>(path);

			string[] lines = textAsset.text.Split('\n');
			for (int i = 1; i < lines.Length-1; i++)
			{
				string[] tokens = lines[i].Split(',');

				float waveCd = float.Parse(tokens[1]);
				Wave wave = new Wave(waveCd);

				string[] subWaves = tokens[2].Split(';');
				foreach (string subWaveInfo in subWaves)
				{
					string[] properties = subWaveInfo.Split('&');
					
					string[] nameAndCount = properties[0].Split(':');
					string monsterName = nameAndCount[0].Trim();
					int monsterCount = int.Parse(nameAndCount[1]);

					float subWaveCd = float.Parse(properties[1].Split(':')[1]);

					wave.AddSubWave(subWaveCd, monsterName, monsterCount);
				}

				_waves.Add(wave);
			}

		}
		catch (Exception e)
		{
			print(e);
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (_waveIndex >= _waves.Count)
		{
			return;
		}

		_timeSinceLastWave += Time.deltaTime;
		
		if (_waveIndex == -1)
		{
			return;
		}
		
		_waves[_waveIndex].SpawnMonsters(transform.position);
		
		if (_waveIndex < _waves.Count-1 && _timeSinceLastWave >= _waves[_waveIndex+1].waveCd)
		{
			_waveIndex++;
			_timeSinceLastWave = 0f;
		}
	}
}
