using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGenerator : MonoBehaviour
{

    public Transform Hero;
    public Chunk StartChunk;
    public Chunk[] ChunkPrefabs;

    const int PRELOAD_CHUNKS = 2;
    

    private List<Chunk> spawnedChunks = new List<Chunk>();

    // Start is called before the first frame update
    void Start()
    {
        spawnedChunks.Add(StartChunk);

        for (int i = 0; i < PRELOAD_CHUNKS; i++) {
            SpawnChunk();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Hero.position.x + 20 > spawnedChunks[spawnedChunks.Count - 1].End.position.x)
        {
            SpawnChunk();
        }
    }

    private Chunk GetRandomChunk() 
    {
        return ChunkPrefabs[Random.Range(0, ChunkPrefabs.Length)];
    }

    private void SpawnChunk() 
    {
        Chunk chunk = Instantiate(GetRandomChunk());
        chunk.transform.position = spawnedChunks[spawnedChunks.Count - 1].End.position - chunk.Begin.localPosition;
        spawnedChunks.Add(chunk);

    }
}
