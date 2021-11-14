using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void moveActiveBlocksDown()
    {
        for(int x = 0; x < 5; x++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int z = 0; z < 5; z++)
                {
                    if(!(gameBoard[x,y,z] is null) && gameBoard[x, y, z].isActive)
                    {
                        gameBoard[x, y - 1, z] = gameBoard[x, y, z];
                        gameBoard[x, y, z] = null;
                        gameBoard[x, y - 1, z].block.transform.position = new Vector3(x, y - 1, z);
                    }
                }
            }
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
    void advanceGame()
    {
        print("moving");
        for(int x = 0; x < 5; x++)
        {
            for(int y = 0; y < height; y++)
            {
                for(int z = 0; z < 5; z++)
                {
                    if(!(gameBoard[x,y,z] is null) && gameBoard[x, y, z].isActive)
                    {
                        if (y == 0 || (!(gameBoard[x,y-1,z] is null) && !(gameBoard[x,y-1,z].isActive)))
                        {
                            print("found a collision");
                            print(y);
                            setAllInactive();
                            createNewPiece();
                            return;
                            
                        }
                    }
                }
            }
        }

        //if we don't find a piece that cant' fall, then everything can move down
        moveActiveBlocksDown();
    }
}
