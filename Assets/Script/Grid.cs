using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
	public int gridSizeX, gridSizeY;
	public Vector2 startPos, offset;
	public GameObject tilePrefab;
	// array 2 dimensi untuk membuat tiles
	public GameObject[,] tiles;
	public GameObject[] candies;

    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid()
    {
    	tiles = new GameObject[gridSizeX, gridSizeY];
    	offset = tilePrefab.GetComponent<SpriteRenderer>().bounds.size;
    	// menentukan offset, didapatkan dari size prefab
    	startPos = transform.position + (Vector3.left * (offset.x * gridSizeX / 2) + (Vector3.down * (offset.y * gridSizeY / 3)));
    	// looping untuk membuat tile
    	for (int x = 0; x < gridSizeX; x++)
    	{
    		for (int y = 0; y < gridSizeY; y++)
    		{
    			Vector2 pos = new Vector3(startPos.x + (x * offset.x), startPos.y + (y * offset.y));
    			GameObject backgroundTile = Instantiate(tilePrefab, pos, tilePrefab.transform.rotation);
    			backgroundTile.transform.parent = transform;
    			backgroundTile.name = "(" + x + "," + y + ")";

    			int index = Random.Range(0, candies.Length);
                // lakukan iterasi sampai tile tidak ada yag sama dengan sebelahnya
                int MAX_ITERATION = 0;

                while (MatchesAt(x, y, candies[index]) && MAX_ITERATION < 100)
                {
                    index = Random.Range(0, candies.Length);
                    MAX_ITERATION++;
                }
                MAX_ITERATION = 0;
    			// create object
    			GameObject candy = ObjectPooler.Instance.SpawnFromPool(index.ToString(), pos, Quaternion.identity);
    			candy.name = "(" + x + "," + y + ")";
    			tiles[x, y] = candy;
    		}
    	}
    }

    private bool MatchesAt(int column, int row, GameObject piece){
        //Cek jika ada tile yang sama dengan dibawah dan samping nya
        if (column > 1 && row > 1){
            if (tiles[column - 1, row].tag == piece.tag && tiles[column - 2, row].tag == piece.tag){
                return true;
            }
            if (tiles[column, row - 1].tag == piece.tag && tiles[column, row - 2].tag == piece.tag){
                return true;
            }
        }
        else if (column <= 1 || row <= 1){
            //Cek jika ada tile yang sama dengan atas dan sampingnya
            if (row > 1){
                if (tiles[column, row - 1].tag == piece.tag && tiles[column, row - 2].tag == piece.tag){
                    return true;
                }
            }
            if (column > 1){
                if (tiles[column - 1, row].tag == piece.tag && tiles[column - 2, row].tag == piece.tag){
                    return true;
                }
            }
        }
        return false;
    }

    private void DestroyMatchesAt(int column, int row)
    {
        //Destroy tile di indeks tertentu
        if(tiles[column, row].GetComponent<Tile>().isMatched)
        {
            GameManager.instance.GetScore(10);
            tiles[column, row].gameObject.SetActive(false);
            tiles[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        // lakukan looping untuk cek tile yang null lalu di Destroy
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if(tiles[i, j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
    }

    private void RefillBoard()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (tiles[x, y] == null)
                {
                    Vector2 tempPosition = new Vector3(startPos.x + (x * offset.x), startPos.y + (y * offset.y));
                    int candyToUse = Random.Range(0, candies.Length);
                    GameObject tileToRefill = Instantiate(candies[candyToUse], tempPosition, Quaternion.identity);
                    tiles[x, y] = tileToRefill;
                }
            }
        }
    }

    private bool MatchesOnBoard()
    {
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] != null)
                {
                    if (tiles[i, j].GetComponent<Tile>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private IEnumerator DecreaseRow()
    {
        int nullCount = 0;
        for (int i = 0; i < gridSizeX; i++)
        {
            for (int j = 0; j < gridSizeY; j++)
            {
                if (tiles[i, j] == null)
                {
                    nullCount++;
                }
                else if (nullCount > 0)
                {
                    tiles[i, j].GetComponent<Tile>().row -= nullCount;
                    tiles[i, j] = null;
                }
            }
            nullCount = 0;
        }
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoard());
    }
    private IEnumerator FillBoard()
    {
        RefillBoard();
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}

// catatan : lanjutkan di bagian checking Move pada part Destroy Matching Tile