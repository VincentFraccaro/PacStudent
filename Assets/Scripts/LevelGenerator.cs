using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ManualLevel : MonoBehaviour
{
    [SerializeField] private GameObject destroyMe;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile[] tileList;
    private Tile[,] gridTiles;
    
    private int[,] levelMap =
    {
        { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 7 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 4 },
        { 2, 6, 4, 0, 0, 4, 5, 4, 0, 0, 0, 4, 5, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 4, 4, 4, 3, 5, 3 },
        { 2, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5, 5 },
        { 2, 5, 3, 4, 4, 3, 5, 3, 3, 5, 3, 4, 4, 4 },
        { 2, 5, 3, 4, 4, 3, 5, 4, 4, 5, 3, 4, 4, 3 },
        { 2, 5, 5, 5, 5, 5, 5, 4, 4, 5, 5, 5, 5, 4 },
        { 1, 2, 2, 2, 2, 1, 5, 4, 3, 4, 4, 3, 0, 4 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 3, 4, 4, 3, 0, 3 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 2, 5, 4, 4, 0, 3, 4, 4, 0 },
        { 2, 2, 2, 2, 2, 1, 5, 3, 3, 0, 4, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 5, 0, 0, 0, 4, 0, 0, 0 },
    };
    
    void LoadLevel()
    {
        
        int mapHeight = levelMap.GetLength(0);
        int mapWidth = levelMap.GetLength(1);
        
        LoadTiles();
        Tilemap topRight = Instantiate(tilemap, tilemap.transform.parent);
        topRight.transform.localScale = new Vector3(-1, 1, 1);
        topRight.transform.localPosition = new Vector3(levelMap.GetLength(1)*2,0,0);
        Tilemap bottomLeft = Instantiate(tilemap, tilemap.transform.parent);
        bottomLeft.transform.localScale = new Vector3(1, -1, 1);
        bottomLeft.transform.localPosition = new Vector3(0,-levelMap.GetLength(0)*2+1,0);
        Tilemap bottomRight = Instantiate(tilemap, tilemap.transform.parent);
        bottomRight.transform.localScale = new Vector3(-1, -1, 1);
        bottomRight.transform.localPosition = new Vector3(levelMap.GetLength(1)*2,-levelMap.GetLength(0)*2+1,0);
        



    }

    void LoadTiles()
    {
        for (int i = 0; i < levelMap.GetLength(0); i++)
        {
            for (int j = 0; j < levelMap.GetLength(1); j++)
            {
                int tileIndex = levelMap[i, j];
                if(tileIndex == 0) continue; 
                if(tileIndex > 0 && tileIndex < tileList.Length)
                {
                    Quaternion rotation = GetTileRotation(tileIndex, i, j);
                    Vector3Int position = new Vector3Int(j, -i, 0);
                    tilemap.SetTile(position, tileList[tileIndex]);
                    Matrix4x4 matrix = tilemap.GetTransformMatrix(position);
                    matrix *= Matrix4x4.Rotate(rotation);
                    tilemap.SetTransformMatrix(position, matrix);
                    tilemap.RefreshTile(position);
                    gridTiles[i, j] = tileList[tileIndex];
                }
            }
        }
    }


    Quaternion GetTileRotation(int tileIndex, int i, int j)
    {
        switch (tileIndex)
        {
            case 1: // Outside corner
                if (OutsideWallOrCorner(i + 1, j) && OutsideWallOrCorner(i, j + 1))
                    return Quaternion.Euler(0, 0, 0);
                if (OutsideWallOrCorner(i + 1, j) && OutsideWallOrCorner(i, j - 1))
                    return Quaternion.Euler(0, 0, 270);
                if (OutsideWallOrCorner(i - 1, j) && OutsideWallOrCorner(i, j + 1))
                    return Quaternion.Euler(0, 0, 90);
                if (OutsideWallOrCorner(i - 1, j) && OutsideWallOrCorner(i, j - 1))
                    return Quaternion.Euler(0, 0, 180);
                break;

            case 2: // Outside wall
                if (OutsideWallOrCornerOrJunction(i, j - 1) && OutsideWallOrCornerOrJunction(i, j + 1))
                    return Quaternion.Euler(0, 0, 0);
                if (OutsideWallOrCornerOrJunction(i - 1, j) && OutsideWallOrCornerOrJunction(i + 1, j))
                    return Quaternion.Euler(0, 0, 90);
                break;

            case 3: // Inside corner
                if (IsTileType(i - 1, j, 4) && (IsTileType(i, j + 1, 4) || IsTileType(i, j - 1, 4)) && IsTileType(i + 1, j, 3))
                    return Quaternion.Euler(0, 0, 90);
                if (InsideWallOrCorner(i + 1, j) && InsideWallOrCorner(i, j + 1))
                    return Quaternion.Euler(0, 0, 0);
                if (InsideWallOrCorner(i + 1, j) && InsideWallOrCorner(i, j - 1))
                    return Quaternion.Euler(0, 0, 270);
                if (InsideWallOrCorner(i - 1, j) && InsideWallOrCorner(i, j + 1))
                    return Quaternion.Euler(0, 0, 90);
                if (InsideWallOrCorner(i - 1, j) && InsideWallOrCorner(i, j - 1))
                    return Quaternion.Euler(0, 0, 180);

                break;


            case 4: // Inside wall
                if (InsideWallOrCornerOrJunction(i, j - 1) && InsideWallOrCornerOrJunction(i, j + 1))
                    return Quaternion.Euler(0, 0, 0);
                if (InsideWallOrCornerOrJunction(i - 1, j) && InsideWallOrCornerOrJunction(i + 1, j))
                    return Quaternion.Euler(0, 0, 90);
                break;
                

            default:
                return Quaternion.identity;
        }

        return Quaternion.identity;
    }
    
    bool OutsideWallOrCorner(int i, int j)
    {
        return IsTileType(i, j, 1) || IsTileType(i, j, 2);
    }

    bool InsideWallOrCorner(int i, int j)
    {
        return IsTileType(i, j, 3) || IsTileType(i, j, 4);
    }

    bool OutsideWallOrCornerOrJunction(int i, int j)
    {
        return OutsideWallOrCorner(i, j) || IsTileType(i, j, 7);
    }

    bool InsideWallOrCornerOrJunction(int i, int j)
    {
        return InsideWallOrCorner(i, j) || IsTileType(i, j, 7);
    }
    
    bool IsTileType(int i, int j, int type)
    {
        if (i < 0 || i >= levelMap.GetLength(0) || j < 0 || j >= levelMap.GetLength(1))
            return false;

        return levelMap[i, j] == type;
    }


    // Start is called before the first frame update
    void Start()
    {
        Destroy(destroyMe);
        gridTiles = new Tile[levelMap.GetLength(0), levelMap.GetLength(1)];
        LoadLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
