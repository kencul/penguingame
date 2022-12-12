using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [SerializeField] GameObject[] GroundPrefabs = new GameObject[9];
    [SerializeField] GameObject LevelParent;
    [SerializeField] GameObject GoalTile;


    //Length of one side of board, must be odd
    [SerializeField] int BoardWidth = 5;
    private int BoardRadius;

    //Food Variables
    private int MaxFoodCount = 5;
    [SerializeField] GameObject FoodPrefab;
    [SerializeField] GameObject FoodParent;

    private void Awake()
    {
        //Singleton instance
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void GenerateLevel()
    {
        BoardRadius = (BoardWidth - 1) / 2;
        bool GoalPlaced = false;

        for (int x = -BoardRadius; x <= BoardRadius; x++)
        {
            for (int y = -BoardRadius; y <= BoardRadius; y++)
            {
                if (x == 0 && y == 0)
                    continue;
                else if (!GoalPlaced)
                {
                    if((x == -2) || (x == 2)|| (y == -2) || (y == 2))
                    {
                        if ((x, y) == (2, 2) || Random.Range(0f, 1f) < 0.07f)
                        {
                            Instantiate(GoalTile, new Vector3(x * 20, 0, y * 20), Quaternion.Euler(0, Random.Range(0, 3) * 90, 0), LevelParent.transform);
                            continue;
                        }
                    }
                }
                Instantiate(GroundPrefabs[Random.Range(0, 9)], new Vector3(x * 20, 0, y * 20), Quaternion.Euler(0, Random.Range(0, 3) * 90, 0), LevelParent.transform);
            }
        }

        GenerateFood();
    }

    private void GenerateFood()
    {
        GameObject[] FoodSpawns = GameObject.FindGameObjectsWithTag("FoodSpawn");
        for(int i = 0; i <= MaxFoodCount; i++)
        {
            GameObject SpawnLoc = FoodSpawns[Random.Range(0, FoodSpawns.Length)];
            Instantiate(FoodPrefab, SpawnLoc.transform.position, Quaternion.identity, FoodParent.transform);
        }
    }
        
    
}
