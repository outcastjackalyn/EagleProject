using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct Chunk
{
    public int id;
    public int gridX;
    public int gridZ;
    public GameObject terrain;

    public Chunk (int i, int x, int z, GameObject obj)
    {
        id = i;
        gridX = x;
        gridZ = z;
        terrain = obj;
    }
}




public class ChunkManagement : MonoBehaviour
{
    [Range(1, 20)] [SerializeField] private int renderDist = 4;
    [Range(0, 6)] [SerializeField] private int genDist = 0;
    [SerializeField] private GameObject player;
    [SerializeField] public List<Chunk> chunks;
    [SerializeField] private int chunkLength = 19;
    [SerializeField] private Vector2 gridPos = new Vector2(0, 0);
    private float interval = 0.0f;
    [SerializeField] private GameObject grasslands;
    [SerializeField] private GameObject surface;
    [SerializeField] private GameObject decor;
    [SerializeField] private GameObject decorholder;

    private float radius = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        surface = new GameObject();
        surface.name = "ChunkHolder";
        decorholder = new GameObject();
        decorholder.name = "DecorHolder";
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (interval > 0)
        {
            interval -= Time.deltaTime;
        }
        else
        {
            gridPos = playerGridPos();
            generateChunks();
            loadChunks();
            interval = 0.0f;
        }
    }

    void loadChunks()
    {
        foreach (Chunk chunk in this.chunks)
        {
            if(chunk.gridX <= gridPos.x + renderDist && chunk.gridX >= gridPos.x - renderDist 
                && chunk.gridZ <= gridPos.y + renderDist && chunk.gridZ >= gridPos.y - renderDist)
            {
                chunk.terrain.SetActive(true);
                chunk.terrain.GetComponentInChildren<Floor>().Reload();
            }
            else
            {
                if (chunk.terrain.activeInHierarchy)
                {
                    if (chunk.terrain.GetComponentInChildren<Floor>().stayLoaded <= 0)
                    {
                        chunk.terrain.SetActive(false);
                    }
                }
            }
        }
    }




    void generateChunks()
    {
        
        int size = renderDist + genDist;
        int x = Mathf.RoundToInt(gridPos.x);
        int z = Mathf.RoundToInt(gridPos.y);
        for (int i = x - size; i < x + size; i++)
        {
            for (int j = z - size; j < z + size; j++)
            {
                if(!chunkExistsAt(i, j))
                {
                    GameObject terrain = new GameObject();
                    int id = chunks.Count;
                    Vector3 pos = new Vector3(i * chunkLength, 0, j * chunkLength);
                    terrain.transform.position = pos;
                    terrain.name = "Terrain" + id.ToString();
                    terrain.transform.parent = surface.transform;
                    //Instantiate(terrain, pos, Quaternion.identity);
                    Chunk chunk = new Chunk(id, i, j, terrain);
                    chunks.Add(chunk);
                    buildChunk(chunk, pos);
                }
            }
        }
    }

    bool chunkExistsAt(int x, int z)
    {
        bool found = false;
        foreach(Chunk chunk in this.chunks)
        {
            if(chunk.gridX == x && chunk.gridZ == z)
            {
                found = true;
            }
        }
        return found;
    }



    Vector2 playerGridPos()
    {
        int x = 0;
        int z = 0;
        if(player.transform.position.x < 0)
        {
            while (player.transform.position.x - chunkLength * x < 0 - chunkLength)
            {
                x--;
            }
        }
        if (player.transform.position.z < 0)
        {
            while (player.transform.position.z - chunkLength * z < 0 - chunkLength)
            {
                z--;
            }
        }
        if (player.transform.position.x > 0)
        {
            while (player.transform.position.x - chunkLength * x > 0 + chunkLength)
            {
                x++;
            }
        }
        if (player.transform.position.z > 0)
        {
            while (player.transform.position.z - chunkLength * z > 0 + chunkLength)
            {
                z++;
            }
        }
        return new Vector2(x, z);
    }

    void buildChunk(Chunk chunk, Vector3 pos) 
    {
        int angle = 90 * Mathf.FloorToInt(Random.Range(0.0f, 4.0f));
        grasslands.transform.Rotate(0, angle, 0);
        Instantiate(grasslands, chunk.terrain.transform);
        foreach (Vector3 v in DecorLocs())
        {
            GameObject d = Instantiate(decor, v + pos, Quaternion.identity);
            d.transform.parent = decorholder.transform;
            //d.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeZ | RigidbodyConstraints.FreezePositionX;
            d.GetComponent<DecorGen>().SetChunk(chunk);
        }

    }

    List<Vector3> DecorLocs()
    {
        List<Vector3> locs = new List<Vector3>();
        float[] pos = new float[2];
        int count = Mathf.FloorToInt(Random.Range(7.5f, 11.5f));
        for(int i = 0; i < count; i++)
        {
            pos = checkPos(locs);
            locs.Add(new Vector3(pos[0], 3, pos[1]));
        }
        return locs;
    }

    float[] checkPos (List<Vector3> locs)
    {
        bool clash = false;
        float[] pos = new float[2];
        pos[0] = (Random.Range(radius - chunkLength /2, chunkLength /2));
        pos[1] = (Random.Range(radius - chunkLength /2, chunkLength /2));
        foreach(Vector3 v in locs)
        {
            if(v.x + radius > pos[0] && v.x - radius < pos[0] && v.z + radius > pos[1] && v.z - radius < pos[1])
            {
                clash = true;
            }
        }
        if(clash)
        {
            return checkPos(locs);
        }
        return pos;
    }

}
