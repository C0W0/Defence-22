using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    
    public HashSet<Monster> allSpawnedMonsters;
    public Dictionary<string, GameObject> monsterPrefabs;

    public GameObject testMonsterPrefab;

    private void Awake()
    {
        Instance = this;
        allSpawnedMonsters = new HashSet<Monster>();
        monsterPrefabs = new Dictionary<string, GameObject>();
        
        monsterPrefabs.Add("distraction1", testMonsterPrefab);
        monsterPrefabs.Add("distraction2", testMonsterPrefab);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Monster SpawnMonster(GameObject prefab, Vector2 position)
    {
        GameObject monsterObj = Instantiate(prefab);
        monsterObj.transform.position = position;

        Monster monster = monsterObj.GetComponent<Monster>(); 
        allSpawnedMonsters.Add(monster);

        return monster;
    }

    public void DeSpawnMonster(Monster monster)
    {
        allSpawnedMonsters.Remove(monster);
        
        monster.DeSpawnInternal();
    }
}
