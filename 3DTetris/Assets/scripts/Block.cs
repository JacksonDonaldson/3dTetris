using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public GameObject block;
    public bool isPivot;
    public bool isActive;

    public Block(Vector3 pos, Material color, bool isPivot)
    {
        pos = new Vector3(pos.x, pos.y,pos.z);
        block = GameObject.CreatePrimitive(PrimitiveType.Cube);
        block.transform.position = pos;
        block.GetComponent<Renderer>().material = color;
        this.isPivot = isPivot;
        isActive = true;
    }

    public Block copy()
    {
        return new Block(block.transform.position, block.GetComponent<Renderer>().material, isPivot);
    }

}
