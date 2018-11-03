using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    private Renderer pickedFlower;
    private Renderer pickedGarlic;
    private Renderer human;
    private Renderer mouse;

    // Use this for initialization
    void Start()
    {
        pickedFlower = GameObject.Find("Picked Flower").GetComponent<Renderer>();
        pickedGarlic = GameObject.Find("Picked Garlic").GetComponent<Renderer>();
        human = GameObject.Find("Human").GetComponent<Renderer>();
        mouse = GameObject.Find("Mouse").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveTo(Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTo(Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTo(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTo(Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RevealBlock();
        }
    }

    public bool HasFlower
    {
        get { return pickedFlower.enabled; }
        set { pickedFlower.enabled = value; }
    }

    public bool HasGarlic
    {
        get { return pickedGarlic.enabled; }
        set { pickedGarlic.enabled = value; }
    }

    public bool IsHuman
    {
        get { return human.enabled; }
        set { human.enabled = value; }
    }

    public bool IsMouse
    {
        get { return mouse.enabled; }
        set { mouse.enabled = value; }
    }

    public int CurrentHorizontalPosition
    {
        get { return Convert.ToInt32(this.transform.position.x); }
    }

    public int CurrentVerticalPosition
    {
        get { return Convert.ToInt32(this.transform.position.z); }
    }

    private void RevealBlock()
    {
        var currentBlock = GetCurrentBlock();
        var item = currentBlock.transform.Find("Item Container");

        if (item == null)
        {
            return;
        }

        item.transform.position = new Vector3(item.transform.position.x, .25f, item.transform.position.z);

        if (item.Find("Flower") != null)
        {
            HasFlower = true;
        }

        if (item.Find("Garlic") != null)
        {
            HasGarlic = true;
        }

        if (HasFlower && HasGarlic)
        {
            IsHuman = false;
            IsMouse = true;
            HasFlower = false;
            HasGarlic = false;
        }
    }

    private Transform GetCurrentBlock()
    {
        return GetBlock(CurrentHorizontalPosition, CurrentVerticalPosition);
    }

    private Transform GetBlock(int horizontal, int vertical)
    {
        var row = GameObject.Find("Row (" + vertical + ")");
        if (row == null)
        {
            return null;
        }
        return row.transform.Find("TerrainBlock (" + horizontal + ")");
    }

    private void MoveTo(Direction direction)
    {
        int horizontalChange = 0;
        int verticalChange = 0;

        switch (direction)
        {
            case Direction.Up:
                verticalChange = 1;
                break;
            case Direction.Down:
                verticalChange = -1;
                break;
            case Direction.Right:
                horizontalChange = 1;
                break;
            case Direction.Left:
                horizontalChange = -1;
                break;
        }

        int nextHorizontal = CurrentHorizontalPosition + horizontalChange;
        int nextVertical = CurrentVerticalPosition + verticalChange;

        if (IsValidPosition(nextHorizontal, nextVertical))
        {
            this.transform.position = new Vector3(nextHorizontal, 0, nextVertical);
        }        
    }

    private bool IsValidPosition(int horizontal, int vertical)
    {
        var nextBlock = GetBlock(horizontal, vertical);

        if (nextBlock == null)
        {
            return false;
        }

        if (nextBlock.CompareTag("Grass"))
        {
            return true;
        }

        if (IsMouse && nextBlock.CompareTag("Rock"))
        {
            return true;
        }

        return false;
    }
}

public enum Direction
{
    Up, Down, Right, Left
}
