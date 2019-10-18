using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour {

    public GameObject[] RoadBlockPrefabs; // Массив с префабами дороги
    public GameObject StartBlock;

    float startBlockXPos;                   // Начальная позиция спавна нового блока
    int blocksCount = 7;                    // Кол-во изначально генерируемых блоков
    float blockLength = 0;                  // Длина одного блока
    int safeZone = 50;

    public Transform PlayerTransf;
    List<GameObject> CurrentBlocks = new List<GameObject>();        // Список сгенерированных блоков

    Vector3 startPlayerPos;

	void Start ()
    {
        startBlockXPos = PlayerTransf.position.x + 15;
        blockLength = 30;

        StartGame();
	}

    public void StartGame()
    {
        PlayerTransf.GetComponent<PlayerMovement>().ResetPosition();

        foreach (var go in CurrentBlocks)
            Destroy(go);

        CurrentBlocks.Clear();

        for (int i = 0; i < blocksCount; i++)
            SpawnBlock();
    }

    void LateUpdate ()
    {
        CheckForSpawn();
	}

    void CheckForSpawn() //Спавним блок если расстояние от игрока до 1 блока > -25
    {
        if (CurrentBlocks[0].transform.position.x - PlayerTransf.position.x < -25)
        {
            SpawnBlock();
            DestroyBlock();
        }
    }

    void SpawnBlock()               // Спавним случайный объект из массива префабов
    {
        GameObject block = Instantiate(RoadBlockPrefabs[Random.Range(0, RoadBlockPrefabs.Length)], transform);
        Vector3 blockPos;

        if (CurrentBlocks.Count > 0)            //Если в списке есть элемент вектор равен длине последнего блока + длина блока
            blockPos = CurrentBlocks[CurrentBlocks.Count - 1].transform.position + new Vector3(blockLength, 0, 0);
        else   //иначе ставим блок в стартовой позиции
            blockPos = new Vector3(startBlockXPos, 0, 0);

        block.transform.position = blockPos;

        CurrentBlocks.Add(block);
    }

    void DestroyBlock()
    {
        Destroy(CurrentBlocks[0]);
        CurrentBlocks.RemoveAt(0);
    }
}
