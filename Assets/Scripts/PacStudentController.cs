using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{ 
    public enum TileType
    {
        None,
        Pellet,
        Wall
    }
    
    public float moveSpeed = 5.0f;

    private ParticleSystem dustParticleSystem;
    private Animator anim;
    private Vector3 targetPosition;
    private Vector3 currentInput;  
    private Vector3 lastInput;     
    private bool isMoving = false;
    private bool isEmittingDust = false;
    
    [SerializeField] private Tilemap[] wallTilemaps;
    [SerializeField] private TileBase[] wallTiles;
    [SerializeField] private TileBase[] safeTiles;

    [SerializeField] private AudioClip chompSound;
    [SerializeField] private AudioClip movingSound;

    private AudioSource chompAudio;
    private AudioSource movingAudio;

    private bool isChompSoundPlaying = false;
    private bool isMovingSoundPlaying = false;


    public Vector3 gridCellSize = new Vector3(1.0f, 1.0f, 0.0f);


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        dustParticleSystem = GetComponentInChildren<ParticleSystem>();
        if (dustParticleSystem != null)
        {
            dustParticleSystem.Stop();
        }
        chompAudio = GetComponent<AudioSource>();
        movingAudio = GetComponent<AudioSource>();
        targetPosition = transform.position;
    }

    // Update is called once per frame
    private void Update()
    {
        HandleInput();
        
        if (!isMoving)
        {
            StopEmittingDustParticles();
            Vector3 nextPosition = transform.position + lastInput;
            Vector3 currentNextPosition = transform.position + currentInput;

            TileType nextTileType = IsWalkable(nextPosition);
            TileType currentNextTileType = IsWalkable(currentNextPosition);

            if (nextTileType == TileType.Pellet || nextTileType == TileType.None)
            {
                currentInput = lastInput;
                targetPosition = GetCenterOfGridCell(nextPosition);
                isMoving = true;
                
                if (nextTileType == TileType.Pellet && !isChompSoundPlaying)
                {
                    chompAudio.PlayOneShot(chompSound);
                    isChompSoundPlaying = true;
                }
            }
            else if (currentNextTileType == TileType.Pellet || currentNextTileType == TileType.None)
            {
                targetPosition = GetCenterOfGridCell(currentNextPosition);
                isMoving = true;
                
                if (currentNextTileType == TileType.Pellet && !isChompSoundPlaying)
                {
                    chompAudio.PlayOneShot(chompSound);
                    isChompSoundPlaying = true;
                }
            }
        }
        else if(isMoving)
        {
            EmitDustParticles();
        }
        
        
        if (isMoving && !isMovingSoundPlaying)
        {
            movingAudio.PlayOneShot(movingSound);
            isMovingSoundPlaying = true;
        }

        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

        if (transform.position == targetPosition)
        {
            isMoving = false;
            isChompSoundPlaying = false;
            isMovingSoundPlaying = false;
        }
    }

    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            lastInput = new Vector3(1, 0, 0); // Right
            anim.SetInteger("Direction", 0);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            lastInput = new Vector3(-1, 0, 0); // Left
            anim.SetInteger("Direction", 2);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            lastInput = new Vector3(0, 1, 0); // Up
            anim.SetInteger("Direction", 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            lastInput = new Vector3(0, -1, 0); // Down
            anim.SetInteger("Direction", 3);
        }

    }
    
    private TileType IsWalkable(Vector3 position)
    {
        foreach (Tilemap tilemap in wallTilemaps)
        {
            Vector3Int cellPosition = tilemap.WorldToCell(position);
            TileBase tile = tilemap.GetTile(cellPosition);
            
            if (tile is AnimatedTile)
            {
                return TileType.Pellet;
            }

            foreach (TileBase wallTile in wallTiles)
            {
                if (tile == wallTile)
                {
                    print("At a wall");
                    return TileType.Wall;
                }
            }

            foreach (TileBase safeTile in safeTiles)
            {
                if (tile == safeTile)
                {
                    print("It's a pellet");
                    return TileType.Pellet;
                }
            }
        }
        
        print("It's a none");

        return TileType.None;
    }
    
    private Vector3 GetCenterOfGridCell(Vector3 position)
    {
        int gridX = Mathf.FloorToInt(position.x);
        int gridY = Mathf.FloorToInt(position.y);
        Vector3 center = new Vector3(gridX + 0.5f, gridY + 0.5f, position.z);
        return center;
    }
    
    private void EmitDustParticles()
    {
        if (!isEmittingDust)
        {
            dustParticleSystem.Play();
            isEmittingDust = true;
        }
    }

    // Stop emitting dust particles if currently emitting.
    private void StopEmittingDustParticles()
    {
        if (isEmittingDust)
        {
            dustParticleSystem.Stop();
            isEmittingDust = false;
        }
    }

}
