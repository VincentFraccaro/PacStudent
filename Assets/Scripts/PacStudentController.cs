using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    
    private Vector3 targetPosition;
    private Vector3 currentInput;  
    private Vector3 lastInput;     
    private bool isMoving = false;
    [SerializeField] private Tilemap[] wallTilemap;
    [SerializeField] private TileBase[] wallTiles;
    [SerializeField] private TileBase[] safeTiles;

    
    public Vector3 gridCellSize = new Vector3(1.0f, 1.0f, 0.0f);


    // Start is called before the first frame update
    void Start()
    {
        targetPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();

        if (!isMoving)
        {
            Vector3 nextPosition = transform.position + lastInput;
            Vector3 currentNextPosition = transform.position + currentInput;

            bool canMoveLastInput = IsWalkable(nextPosition);
            bool canMoveCurrentInput = IsWalkable(currentNextPosition);

            if (canMoveLastInput || canMoveCurrentInput)
            {
                if (canMoveLastInput)
                {
                    currentInput = lastInput;
                    targetPosition = GetCenterOfGridCell(nextPosition);
                }
                else
                {
                    targetPosition = GetCenterOfGridCell(currentNextPosition);
                }

                isMoving = true;
            }
        }

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (transform.position == targetPosition)
        {
            isMoving = false;
        }
    }

    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = new Vector3(1, 0, 0); // Right
            print("Pressed D");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = new Vector3(-1, 0, 0); // Left
            print("Pressed A");
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = new Vector3(0, 1, 0); // Up
            print("Pressed W");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = new Vector3(0, -1, 0); // Down
            print("Pressed S");
        }

    }
    
    private bool IsWalkable(Vector3 position)
    {
        int gridX = Mathf.FloorToInt(position.x);
        int gridY = Mathf.FloorToInt(position.y);

        Vector3Int cellPosition = new Vector3Int(gridX, gridY, 0);

        foreach (Tilemap tilemap in wallTilemap)
        {
            TileBase tile = tilemap.GetTile(cellPosition);

            foreach (TileBase wallTile in wallTiles)
            {
                if (tile == wallTile)
                {
                    return false;
                }
            }

            foreach (TileBase safeTile in safeTiles)
            {
                if (tile == safeTile)
                {
                    return true;
                }
            }
        }

        return true;
    }

    
    private Vector3 GetCenterOfGridCell(Vector3 position)
    {
        int gridX = Mathf.FloorToInt(position.x);
        int gridY = Mathf.FloorToInt(position.y);
        Vector3 center = new Vector3(gridX + 0.5f, gridY + 0.5f, position.z);
        return center;
    }

}
