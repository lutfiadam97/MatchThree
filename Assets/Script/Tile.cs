using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
	private Vector3 firstPos; // posisi awal saat jari menyentuh layar
	private Vector3 finalPos; // posisi akhir saat jari melepaskan sentuhan
	private float swipeAngle;
	private Vector3 tempPosition;
  private int previousColumn;
  private int previousRow;
	// menampung data posisi tile
	public float xPosition;
	public float yPosition;
	public int column;
	public int row;
	private Grid grid;
	private GameObject otherTile;
  public bool isMatched = false;
    // Start is called before the first frame update
    void Start()
    {
        // menentukan posisi tile
        grid = FindObjectOfType<Grid>();
        xPosition = transform.position.x;
        yPosition = transform.position.y;
        row = Mathf.RoundToInt((xPosition - grid.startPos.x)/ grid.offset.x);
        column = Mathf.RoundToInt((yPosition - grid.startPos.x)/ grid.offset.x);
    }

    void Update()
    {
      if(isMatched)
      {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.color = Color.grey;
      }
    }

    void OnMouseDown()
    {
    	// mendapatkan titik awal sentuhan jari
    	firstPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseUp()
    {
    	finalPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    	CalculateAngle();
    }

    void CalculateAngle()
    {
    	// menghitung sudut antara posisi awal dan posisi akhir
    	swipeAngle = Mathf.Atan2(finalPos.y - firstPos.y, finalPos.x - firstPos.x) * 180/Mathf.PI;
    	MoveTile();
    }

    void SwipeTile()
    {
       if (Mathf.Abs(xPosition - transform.position.x) > .1){
           //Move towards the target
           Vector3 tempPosition = new Vector2(xPosition, transform.position.y);
           transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
       }
       else{
           //Directly set the position
           tempPosition = new Vector2(xPosition, transform.position.y);
           transform.position = tempPosition;
           grid.tiles[column, row] = this.gameObject;
       }
       
       if (Mathf.Abs(yPosition - transform.position.y) > .1){
          //Move towards the target
          Vector3 tempPosition = new Vector2(transform.position.x, yPosition);
          transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);
       }
       else
       {
          //Directly set the position
          Vector3 tempPosition = new Vector2(transform.position.x, yPosition);
          transform.position = tempPosition;
          grid.tiles[column, row] = this.gameObject;
      }
    }


    void MoveTile()
    {
      previousColumn = column;
      previousRow = row;
      
    	if(swipeAngle > -45 && swipeAngle <= 45)
    	{
    		//Right swipe
    		RightMove();
        //Debug.Log("Right swipe");
    	}
    	else if (swipeAngle > 45 && swipeAngle <= 135)
    	{
    		// up swipe
    		UpMove();
        //Debug.Log("Up swipe");
    	}
    	else if (swipeAngle > 135 || swipeAngle <= -135)
    	{
    		// left swipe
    		LeftMove();
        //Debug.Log("Left swipe");
    	}
    	else if (swipeAngle < -45 && swipeAngle >= -135)
    	{
    		// down swipe
    		DownMove();
        //Debug.Log("Down swipe");
    	}

      StartCoroutine(checkMove());
    }

    void RightMove()
    {
      if(column + 1 < grid.gridSizeX)
      {
        // menukar posisi tile dengan sebelah kanannya
        otherTile = grid.tiles[column + 1, row];
        otherTile.GetComponent<Tile>().column -= 1;
        column += 1;
      }
    }

    void UpMove()
    {
      if(row + 1 < grid.gridSizeY)
      {
        // menukar posisi tile dengan sebelah atasnya
        otherTile = grid.tiles[column, row + 1];
        otherTile.GetComponent<Tile>().row -= 1;
        row += 1;
      }   
    }

    void LeftMove()
    {
      if(column - 1 >= 0)
      {
        // menukar posisi tile dengan sebelah kanannya
        otherTile = grid.tiles[column - 1, row];
        otherTile.GetComponent<Tile>().column += 1;
        column -= 1;
      }      
    }

    void DownMove()
    {
      if(row - 1 >= 0)
      {
        // menukar posisi tile dengan sebelah kanannya
        otherTile = grid.tiles[column, row - 1];
        otherTile.GetComponent<Tile>().column += 1;
        column -= 1;
      }
    }

    void CheckMatches()
    {
      // check horizontal matching
      if (column > 0 && column < grid.gridSizeX -1 )
      {
        // check samping kiri dan kanan nya
        GameObject leftTile = grid.tiles[column - 1, row];
        GameObject rightTile = grid.tiles[column + 1, row];
        if(leftTile != null && rightTile != null)
        {
          if(leftTile.CompareTag(gameObject.tag) && rightTile.CompareTag(gameObject.tag))
          {
            isMatched = true;
            rightTile.GetComponent<Tile>().isMatched = true;
            leftTile.GetComponent<Tile>().isMatched = true;
          }
        }
      }
      // check vertical matching
      if (row > 0 && row < grid.gridSizeY - 1)
      {
        // check samping atas dan bawahnya
        GameObject upTile = grid.tiles[column, row + 1];
        GameObject downTile = grid.tiles[column, row - 1];
        if(upTile != null && downTile != null)
        {
          if(upTile.CompareTag(gameObject.tag) && downTile.CompareTag(gameObject.tag))
          {
            isMatched = true;
            downTile.GetComponent<Tile>().isMatched = true;
            upTile.GetComponent<Tile>().isMatched = true;
          }
        }
      }
    }

    IEnumerator checkMove()
    {
      yield return new WaitForSeconds(.5f);
      // cek jika tilenya tidak sama kembalikan, jika ada yg sama panggil destroy matches
      if (otherTile != null)
        {
            if (!isMatched && !otherTile.GetComponent<Tile>().isMatched)
            {
                otherTile.GetComponent<Tile>().row = row;
                otherTile.GetComponent<Tile>().column = column;
                row = previousRow;
                column = previousColumn;
            }
            else
            {
                grid.DestroyMatches();
            }
        }
        otherTile = null;
    }
}
