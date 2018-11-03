using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveHeroTo(Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveHeroTo(Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveHeroTo(Direction.Right);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveHeroTo(Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RevealBlock();
        }

    }

    public bool HasFlower
    {
        get { return GameObject.Find("Picked Flower").GetComponent<Renderer>().enabled; }
        set { GameObject.Find("Picked Flower").GetComponent<Renderer>().enabled = value; }
    }

    public bool HasGarlic
    {
        get { return GameObject.Find("Picked Garlic").GetComponent<Renderer>().enabled; }
        set { GameObject.Find("Picked Garlic").GetComponent<Renderer>().enabled = value; }
    }

    public bool IsHuman
    {
        get { return GameObject.Find("Human").GetComponent<Renderer>().enabled; }
        set { GameObject.Find("Human").GetComponent<Renderer>().enabled = value; }
    }

    public bool IsMouse
    {
        get { return GameObject.Find("Mouse").GetComponent<Renderer>().enabled; }
        set { GameObject.Find("Mouse").GetComponent<Renderer>().enabled = value; }
    }

    private void RevealBlock()
    {
        var currentBlock = GetCurrentBlock();
        var item = currentBlock.transform.Find("Item Container");
        if (item == null)
        {
            return;
        }

        item.transform.position = new Vector3(item.transform.position.x, item.transform.position.y + .25f, item.transform.position.z);
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
        var horizontal = Convert.ToInt32(this.transform.position.x);
        var vertical = Convert.ToInt32(this.transform.position.z);
        var row = GameObject.Find("Row (" + vertical + ")");
        return row.transform.Find("TerrainBlock (" + horizontal + ")");
    }

    private void MoveHeroTo(Direction direction)
    {
        float horizontal = 0f;
        float vertical = 0f;

        switch (direction)
        {
            case Direction.Up:
                vertical = 1f;
                break;
            case Direction.Down:
                vertical = -1f;
                break;
            case Direction.Right:
                horizontal = 1f;
                break;
            case Direction.Left:
                horizontal = -1f;
                break;
        }
        var currentPosition = this.transform.position;
        this.transform.position = new Vector3(currentPosition.x + horizontal, currentPosition.y, currentPosition.z + vertical);
    }
}

public enum Direction
{
    Up, Down, Right, Left
}
