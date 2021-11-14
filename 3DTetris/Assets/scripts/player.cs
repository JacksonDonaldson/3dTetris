using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class player : MonoBehaviour
{

    public double gameTick = 2;
    private double lastGameTick;
    private static int height = 8;

    public Block[,,] gameBoard = new Block[5,height,5];

    // Start is called before the first frame update
    void Start()
    {
        lastGameTick = Time.time;
        createNewPiece();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - lastGameTick > gameTick)
        {
            lastGameTick = Time.time;
            advanceGame();
        }

        doTranslationalMovement();
        doRotationalMovement();


        
    }
    void advanceGame()
    {
        if(!tryMoveActive(new Vector3(0, -1, 0)))
        {
            print("found a collision");
            setAllInactive();
            createNewPiece();
            return;
        }
    }

    void doTranslationalMovement()
    {
        if (Input.GetButtonDown("Up"))
        {
            tryMoveActive(new Vector3(0, 0, 1));
        }
        if (Input.GetButtonDown("Down"))
        {
            tryMoveActive(new Vector3(0, 0, -1));
        }
        if (Input.GetButtonDown("Left"))
        {
            tryMoveActive(new Vector3(-1, 0, 0));
        }
        if (Input.GetButtonDown("Right"))
        {
            tryMoveActive(new Vector3(1, 0, 0));
        }
    }

    void doRotationalMovement()
    {

    }

    public Material green, red, cyan, orange, purple, yellow;

    void createNewPiece()
    {
        int tet = Random.Range(0, 5);

        switch(tet){
            case 0:
                //green
                gameBoard[2, height - 2,2] = new Block(new Vector3(2, height - 2,2), green, false);
                gameBoard[3, height - 2,2] = new Block(new Vector3(3, height - 2,2), green, true);
                gameBoard[3, height - 2,3] = new Block(new Vector3(3, height - 2,3), green, false);
                gameBoard[3, height - 1,2] = new Block(new Vector3(3, height - 1,2), green, false);
                break;
            case 1:
                //red
                gameBoard[3, height - 1, 2] = new Block(new Vector3(3, height - 1, 2), red, true);
                gameBoard[3, height - 1, 3] = new Block(new Vector3(3, height - 1,3), red, false);
                gameBoard[2, height - 1, 2] = new Block(new Vector3(2, height - 1,2), red, false);
                gameBoard[2, height - 1, 1] = new Block(new Vector3(2, height - 1,1), red, false);
                break;
            case 2:
                //cyan
                gameBoard[2, height - 1, 3] = new Block(new Vector3(2, height - 1,3), cyan, false);
                gameBoard[2, height - 1, 2] = new Block(new Vector3(2, height - 1,2), cyan, true);
                gameBoard[2, height - 1, 1] = new Block(new Vector3(2, height - 1,1), cyan, false);
                gameBoard[2, height - 1, 0] = new Block(new Vector3(2, height - 1,0), cyan, false);
                print(2);
                break;
            case 3:
                //orange
                gameBoard[1, height - 1, 1] = new Block(new Vector3(1, height - 1,1), orange, false);
                gameBoard[2, height - 1, 1] = new Block(new Vector3(2, height - 1,1), orange, false);
                gameBoard[2, height - 1, 2] = new Block(new Vector3(2, height - 1,2), orange, true);
                gameBoard[2, height - 1, 3] = new Block(new Vector3(2, height - 1,3), orange, false);
                print(3);
                break;
            case 4:
                //purple
                gameBoard[1, height - 1,2] = new Block(new Vector3(1, height - 1,2), purple, false);
                gameBoard[2, height - 1, 1] = new Block(new Vector3(2, height - 1,1), purple, false);
                gameBoard[2, height - 1, 2] = new Block(new Vector3(2, height - 1,2), purple, true);
                gameBoard[2, height - 1, 3] = new Block(new Vector3(2, height - 1,3), purple, false);
                print(4);
                break;
            case 5:
                //yellow
                gameBoard[3, height - 1, 1] = new Block(new Vector3(3, height - 1,1), yellow, false);
                gameBoard[2, height - 1, 1] = new Block(new Vector3(2, height - 1,1), yellow, false);
                gameBoard[2, height - 1, 2] = new Block(new Vector3(2, height - 1,2), yellow, true);
                gameBoard[3, height - 1, 2] = new Block(new Vector3(3, height - 1,2), yellow, false);
                break;

        }   
    }

    void setAllInactive()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if(!(gameBoard[x,y,z] is null))
                    {
                        gameBoard[x, y, z].isActive = false;
                    }
                }
            }
        }
    }
    bool tryMoveActive(Vector3 dir)
    {
        print("moving");
        
        for(int x = 0; x < 5; x++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int z = 0; z < 5; z++)
                {
                    int nux = x + (int)dir.x;
                    int nuy = y + (int)dir.y;
                    int nuz = z + (int)dir.z;
                    if(!(gameBoard[x,y,z] is null) && gameBoard[x, y, z].isActive)
                    {
                        if (nuy < 0 || nuy >= height || nux < 0 || nux >=5 || nuz < 0 || nuz>=5 || (!(gameBoard[nux,nuy,nuz] is null) && !(gameBoard[nux,nuy,nuz].isActive)))
                        {
                            return false;
                            
                        }
                    }
                }
            }
        }

        //if we don't find a piece that cant' fall, then everything can move down
        doMoveActive(dir);
        return true;
    }

    void doMoveActive(Vector3 dir)
    {
        Block[,,] copyBoard = new Block[5, height, 5];

        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {

                    int nux = x + (int)dir.x;
                    int nuy = y + (int)dir.y;
                    int nuz = z + (int)dir.z;

                    if (!(gameBoard[x, y, z] is null) && gameBoard[x, y, z].isActive)
                    {

                        copyBoard[nux, nuy, nuz] = gameBoard[x, y, z];
                        
                        gameBoard[x, y, z].block.transform.position = new Vector3(nux, nuy, nuz);
                        gameBoard[x, y, z] = null;
                    }
                }
            }
        }
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if(!(copyBoard[x, y, z] is null)){
                        gameBoard[x, y, z] = copyBoard[x, y, z];
                        print("unnull");
                        print(y);
                    }
                }
            }
        }
    }
}
