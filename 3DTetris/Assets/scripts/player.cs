using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;
public class player : MonoBehaviour
{
    public camera cam;

    public double gameTick = 2;
    private double lastGameTick;
    public static int height = 11;
    public Transform floor;
    public int score = 0;

    public int levelupScore = 500;

    public MeshRenderer nWall;
    public MeshRenderer bWall;
    public MeshRenderer sWall;
    public MeshRenderer eWall;
    public MeshRenderer wWall;

    public Block[,,] gameBoard = new Block[5, height, 5];

    private Block[] final;

    private bool pause;

    private double gameTickDiv = 1;

    // Start is called before the first frame update
    void Start()
    {
        lastGameTick = Time.time;
        createNewPiece();
        nextTet = Random.Range(0, 6);
    }

    // Update is called once per frame
    void Update()
    {
        if (pause)
        {
            return;
        }
        if (Time.time - lastGameTick > gameTickDiv * gameTick)
        {
            lastGameTick = Time.time;
            advanceGame();

        }

        doTranslationalMovement();
        doRotationalMovement();

        showFinalLocation();

        doSpecialCommands();


    }
    void advanceGame()
    {
        if (!tryMoveActive(new Vector3(0, -1, 0)))
        {
            //print("found a collision");
            setAllInactive();

            checkForFullMatrix();

            createNewPiece();
            return;
        }
    }

    void doTranslationalMovement()
    {
        if (Input.GetButtonDown("Up"))
        {
            tryMoveActive(cam.up);
        }
        if (Input.GetButtonDown("Down"))
        {
            tryMoveActive(cam.down);
        }
        if (Input.GetButtonDown("Left"))
        {
            tryMoveActive(cam.left);
        }
        if (Input.GetButtonDown("Right"))
        {
            tryMoveActive(cam.right);
        }
    }

    void doRotationalMovement()
    {
        if (Input.GetButtonDown("RotateAway"))
        {
            tryRotate(cam.pitch, cam.pitchpol);
        }
        if (Input.GetButtonDown("RotateTowards"))
        {
            tryRotate(cam.pitch, !cam.pitchpol);
        }
        if (Input.GetButtonDown("RotateLeft"))
        {
            tryRotate(cam.yaw, cam.yawpol);
        }
        if (Input.GetButtonDown("RotateRight"))
        {
            tryRotate(cam.yaw, !cam.yawpol);
        }
        if (Input.GetButtonDown("RotateCounterclockwise"))
        {
            tryRotate(cam.roll, cam.rollpol);
        }
        if (Input.GetButtonDown("RotateClockwise"))
        {
            tryRotate(cam.roll, !cam.rollpol);
        }
    }

    public GameObject gameOverPanel;
    void gameOver()
    {
        pause = true;

        gameOverPanel.SetActive(true);
    }


    void showFinalLocation()
    {
        Block[] blocks = findActiveBlocks();


        //print("done");
        if (!(final is null))
        {
            foreach (Block b in final)
            {
                Destroy(b.block);
            }
        }
        final = new Block[4];
        Color c = blocks[0].block.GetComponent<MeshRenderer>().material.color;

        for (int i = 0; i < 4; i++)
        {
            final[i] = blocks[i].copy();

            final[i].block.GetComponent<MeshRenderer>().material.color = new Color(c.r, c.g, c.b, .5f);
        }

        while (tryMoveActive(new Vector3(0, -1, 0))) { }


        //extra loop here so null writes always get overridden by future to prevent bug
        for (int i = 0; i < 4; i++)
        {
            Vector3 endLocation = blocks[i].block.transform.position;
            gameBoard[(int)endLocation.x, (int)endLocation.y, (int)endLocation.z] = null;
        }
        for (int i = 0; i < 4; i++)
        {
            Vector3 endLocation = blocks[i].block.transform.position;
            Vector3 fPos = final[i].block.transform.position;
            blocks[i].block.transform.position = fPos;


            gameBoard[(int)fPos.x, (int)fPos.y, (int)fPos.z] = blocks[i];

            final[i].block.transform.position = endLocation;
        }

    }

    void doSpecialCommands()
    {
        if (Input.GetButtonDown("HardDrop"))
        {
            //print("down");
            while (tryMoveActive(Vector3.down)) { }
            setAllInactive();
            createNewPiece();

            checkForFullMatrix();
        }


        if (Input.GetButton("SoftDrop"))
        {
            gameTickDiv = .33;
        }
        else
        {
            gameTickDiv = 1;
        }
    }

    bool checkForLevelUp()
    {

        if ((score + 100) % levelupScore != 0) {
            return false;
        }
        gameTick *= .85;

        Color c;
        if (score == 400)
        {
            c = new Color(201f / 255, 240f / 255, 1);
        }
        else if (score == 900)
        {
            c = new Color(201f / 255, 1, 230f / 255);
        }
        else if (score == 1400)
        {
            c = new Color(245f / 255, 1, 155f / 255);
        }
        else
        {
            c = new Color(1, 138f / 255, 115f / 255);
        }

        bWall.materials[0].color = c;
        nWall.materials[0].color = c;
        eWall.materials[0].color = c;
        wWall.materials[0].color = c;
        sWall.materials[0].color = c;

        return true;
    }

    IEnumerator playLevelUp(int y)
    {
        pause = true;

        for (int x = 0; x < 5; x++)
        {

            for (int z = 0; z < 5; z++)
            {
                Destroy(gameBoard[x, y, z].block);
                score += 4;
            }
            soundEffect.Play();

            while (soundEffect.isPlaying)
            {
                yield return null;
            }
        }

        for (int ny = y; ny < height - 1; ny++)
        {
            for (int x = 0; x < 5; x++)
            {
                for (int z = 0; z < 5; z++)
                {

                    gameBoard[x, ny, z] = gameBoard[x, ny + 1, z];
                    gameBoard[x, ny + 1, z] = null;

                    if (!(gameBoard[x, ny, z] is null))
                    {
                        //print("found 1");
                        //print(gameBoard[x, ny, z].isActive);
                        gameBoard[x, ny, z].block.transform.position = new Vector3(x, ny, z);

                    }



                }
            }
        }

        pause = false;

    }
    public AudioSource soundEffect;
    private double story;
    void checkForFullMatrix()
    {
        for (int y = 0; y < height; y++)
        {
            bool good = true;

            for (int x = 0; x < 5; x++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (gameBoard[x, y, z] is null)
                    {

                        good = false;
                        break;
                    }
                }
                if (!good)
                {
                    break;
                }
            }

            if (good)
            {
                // a matrix is full

                if (checkForLevelUp())
                {
                    story = y;
                    StartCoroutine("playLevelUp", y);
                }
                else
                {
                    for (int x = 0; x < 5; x++)
                    {

                        for (int z = 0; z < 5; z++)
                        {
                            Destroy(gameBoard[x, y, z].block);
                            score += 4;
                        }


                    }

                    //move it all down
                    for (int ny = y; ny < height - 1; ny++)
                    {
                        for (int x = 0; x < 5; x++)
                        {
                            for (int z = 0; z < 5; z++)
                            {

                                gameBoard[x, ny, z] = gameBoard[x, ny + 1, z];
                                gameBoard[x, ny + 1, z] = null;

                                if (!(gameBoard[x, ny, z] is null))
                                {
                                    //print("found 1");
                                    //print(gameBoard[x, ny, z].isActive);
                                    gameBoard[x, ny, z].block.transform.position = new Vector3(x, ny, z);

                                }



                            }
                        }
                    }
                    y--;

                }

            }

            //destroy it





        }
    }
    Block findPivot()
    {
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (!(gameBoard[x, y, z] is null) && gameBoard[x, y, z].isPivot && gameBoard[x, y, z].isActive)
                    {
                        return gameBoard[x, y, z];
                    }
                }
            }
        }
        print("VERY BAD NO PIVOT");
        return null;
    }
    Block[] findActiveBlocks()
    {
        Block[] blocks = new Block[4];
        int i = 0;
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (!(gameBoard[x, y, z] is null) && gameBoard[x, y, z].isActive)
                    {
                        blocks[i] = gameBoard[x, y, z];
                        i++;
                    }
                }
            }
        }
        return blocks;
    }



    bool tryRotate(String axis, bool clockwise)
    {
        Block pivot = findPivot();
        Block[] blocks = findActiveBlocks();
        Vector2[] difs = new Vector2[blocks.Length];
        Vector3[] newPositions = new Vector3[blocks.Length];
        //find all the blocks that aren't in line with the pivot on axis
        //then rotate them around the pivot 
        //clockwise = (-y,x) anti = (y,-x)
        float px = pivot.block.transform.position.x;
        float py = pivot.block.transform.position.y;
        float pz = pivot.block.transform.position.z;
        if (axis == "x")
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                difs[i] = new Vector2(blocks[i].block.transform.position.y - py, blocks[i].block.transform.position.z - pz);
                //print(difs[i]);
            }

            if (clockwise)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    newPositions[i] = new Vector3(blocks[i].block.transform.position.x, py + difs[i].y, pz - difs[i].x);
                }
            }
            else
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    newPositions[i] = new Vector3(blocks[i].block.transform.position.x, py - difs[i].y, pz + difs[i].x);
                }
            }
        }

        if (axis == "y")
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                difs[i] = new Vector2(blocks[i].block.transform.position.x - px, blocks[i].block.transform.position.z - pz);
            }
            if (clockwise)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    newPositions[i] = new Vector3(px + difs[i].y, blocks[i].block.transform.position.y, pz - difs[i].x);
                }
            }
            else
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    newPositions[i] = new Vector3(px - difs[i].y, blocks[i].block.transform.position.y, pz + difs[i].x);
                }
            }
            //return attemptAssignment(blocks, newPositions);
        }

        if (axis == "z")
        {
            for (int i = 0; i < blocks.Length; i++)
            {
                difs[i] = new Vector2(blocks[i].block.transform.position.x - px, blocks[i].block.transform.position.y - py);
            }
            if (clockwise)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    newPositions[i] = new Vector3(px + difs[i].y, py - difs[i].x, blocks[i].block.transform.position.z);
                }
            }
            else
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    newPositions[i] = new Vector3(px - difs[i].y, py + difs[i].x, blocks[i].block.transform.position.z);
                }
            }
            //return attemptAssignment(blocks, newPositions);
        }
        if (attemptAssignment(blocks, newPositions))
        {
            //print("assignment succeeded");
            return true;
        }
        //print("assignment failed");
        //if the original assignment failed, we can try having everything move to the side in every direction
        Vector3[] displacements = { Vector3.left, Vector3.right, Vector3.forward, Vector3.back };
        foreach (Vector3 dif in displacements)
        {
            for (int i = 0; i < 4; i++)
            {
                newPositions[i] += dif;
            }

            if (attemptAssignment(blocks, newPositions))
            {
                return true;
            }

            for (int i = 0; i < 4; i++)
            {
                newPositions[i] -= dif;
            }

        }
        return false;



    }

    bool attemptAssignment(Block[] blocks, Vector3[] newPositions)
    {
        //check to see if we'll end up outside of the play area or inside another piece

        for (int i = 0; i < blocks.Length; i++)
        {
            Vector3 pos = newPositions[i];
            if (pos.x < 0 || pos.x >= 5 || pos.y >= height || pos.y < 0 || pos.z >= 5 || pos.z < 0)
            {
                return false;
            }
            if (!(gameBoard[(int)pos.x, (int)pos.y, (int)pos.z] is null) && !gameBoard[(int)pos.x, (int)pos.y, (int)pos.z].isActive)
            {
                return false;
            }
        }

        //we won't, so continue with assignment
        //starting by removing all old blocks
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < 5; z++)
                {
                    if (!(gameBoard[x, y, z] is null) && gameBoard[x, y, z].isActive)
                    {
                        gameBoard[x, y, z] = null;
                    }
                }
            }
        }

        //continuing by assigning newPosition to each block
        for (int i = 0; i < blocks.Length; i++)
        {
            Vector3 pos = newPositions[i];
            gameBoard[(int)pos.x, (int)pos.y, (int)pos.z] = blocks[i];
            blocks[i].block.transform.position = pos;

        }
        return true;
    }
    public Material green, red, cyan, orange, purple, yellow;

    public int nextTet;
    void createNewPiece()
    {
        int tet = nextTet;
        nextTet = Random.Range(0, 6);

        int[,] place = new int [12,3]{ { 2, height - 4, 2 }, { 3, height - 4, 2 }, { 3, height - 4, 3 }, { 3, height - 3, 2 }, { 3, height - 3, 3 },{ 2, height - 3, 2 },{ 2, height - 3, 1 },{ 2, height - 3, 2 },{ 2, height - 3, 0 },{ 1, height - 1, 1 },{ 1, height - 3, 2 },{ 3, height - 3, 1 }};

        for(int i = 0; i < 12; i++)
        {
            if(gameBoard[place[i,0],place[i,1],place[i,2]] != null)
            {
                gameOver();
                return;
            }
        }

        switch(tet){
            case 0:
                //green
                gameBoard[2, height - 4,2] = new Block(new Vector3(2, height - 4,2), green, false, floor);
                gameBoard[3, height - 4,2] = new Block(new Vector3(3, height - 4,2), green, true, floor);
                gameBoard[3, height - 4,3] = new Block(new Vector3(3, height - 4,3), green, false, floor);
                gameBoard[3, height - 3,2] = new Block(new Vector3(3, height - 3,2), green, false, floor);
                break;
            case 1:
                //red
                gameBoard[3, height - 3, 2] = new Block(new Vector3(3, height - 3, 2), red, true, floor);
                gameBoard[3, height - 3, 3] = new Block(new Vector3(3, height - 3,3), red, false, floor);
                gameBoard[2, height - 3, 2] = new Block(new Vector3(2, height - 3,2), red, false, floor);
                gameBoard[2, height - 3, 1] = new Block(new Vector3(2, height - 3,1), red, false, floor);
                break;
            case 2:
                //cyan
                gameBoard[2, height - 3, 3] = new Block(new Vector3(2, height - 3,3), cyan, false, floor);
                gameBoard[2, height - 3, 2] = new Block(new Vector3(2, height - 3,2), cyan, true, floor);
                gameBoard[2, height - 3, 1] = new Block(new Vector3(2, height - 3,1), cyan, false, floor);
                gameBoard[2, height - 3, 0] = new Block(new Vector3(2, height - 3,0), cyan, false, floor);
                //print(2);
                break;
            case 3:
                //orange
                gameBoard[1, height - 3, 1] = new Block(new Vector3(1, height - 3,1), orange, false, floor);
                gameBoard[2, height - 3, 1] = new Block(new Vector3(2, height - 3,1), orange, false, floor);
                gameBoard[2, height - 3, 2] = new Block(new Vector3(2, height - 3,2), orange, true, floor);
                gameBoard[2, height - 3, 3] = new Block(new Vector3(2, height - 3,3), orange, false, floor);
                //print(3);
                break;
            case 4:
                //purple
                gameBoard[1, height - 3,2] = new Block(new Vector3(1, height - 3,2), purple, false, floor);
                gameBoard[2, height - 3, 1] = new Block(new Vector3(2, height - 3,1), purple, false, floor);
                gameBoard[2, height - 3, 2] = new Block(new Vector3(2, height - 3,2), purple, true, floor);
                gameBoard[2, height - 3, 3] = new Block(new Vector3(2, height - 3,3), purple, false, floor);
                //print(4);
                break;
            case 5:
                //yellow
                gameBoard[3, height - 3, 1] = new Block(new Vector3(3, height - 3,1), yellow, false, floor);
                gameBoard[2, height - 3, 1] = new Block(new Vector3(2, height - 3,1), yellow, false, floor);
                gameBoard[2, height - 3, 2] = new Block(new Vector3(2, height - 3,2), yellow, true, floor);
                gameBoard[3, height - 3, 2] = new Block(new Vector3(3, height - 3,2), yellow, false, floor);
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
        //print("moving");
        
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
                        //print("unnull");
                        //print(y);
                    }
                }
            }
        }
    }
}
