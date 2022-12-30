using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager Instance;
    
    public LinkedList<Monster> allMonsters;

    private void Awake()
    {
        Instance = this;
        allMonsters = new LinkedList<Monster>();
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
        allMonsters.AddLast(monster);

        return monster;
    }

    public void DeSpawnMonster(Monster monster)
    {
        allMonsters.Remove(monster);
        
        monster.DeSpawnInternal();
    }
}
