using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementHandler : MonoBehaviour
{
    public Material dirtMaterial;

    private Renderer pickedFlower;
    private Renderer pickedGarlic;
    private Renderer pickedMushroom;
    private Renderer pickedBerry;
    private Renderer pickedKey;

    private Renderer human;
    private Renderer mouse;
    private Renderer frog;
    private Renderer chestClosed;
    private Renderer chestOpened;

    private Renderer life1;
    private Renderer life2;
    private Renderer life3;
    private Renderer life1b;
    private Renderer life2b;
    private Renderer life3b;
    private Renderer gameOver;
    private Renderer youWon;

    // Use this for initialization
    void Start()
    {
        pickedFlower = GameObject.Find("Picked Flower").GetComponent<Renderer>();
        pickedGarlic = GameObject.Find("Picked Garlic").GetComponent<Renderer>();
        pickedMushroom = GameObject.Find("Picked Mushroom").GetComponent<Renderer>();
        pickedBerry = GameObject.Find("Picked Berry").GetComponent<Renderer>();
        pickedKey = GameObject.Find("Picked Key").GetComponent<Renderer>();
        human = GameObject.Find("Human").GetComponent<Renderer>();
        mouse = GameObject.Find("Mouse").GetComponent<Renderer>();
        chestClosed = GameObject.Find("Chest Closed").GetComponent<Renderer>();
        chestOpened = GameObject.Find("Chest Opened").GetComponent<Renderer>();
        frog = GameObject.Find("Frog").GetComponent<Renderer>();
        life1 = GameObject.Find("Life1").GetComponent<Renderer>();
        life2 = GameObject.Find("Life2").GetComponent<Renderer>();
        life3 = GameObject.Find("Life3").GetComponent<Renderer>();
        life1b = GameObject.Find("Life1b").GetComponent<Renderer>();
        life2b = GameObject.Find("Life2b").GetComponent<Renderer>();
        life3b = GameObject.Find("Life3b").GetComponent<Renderer>();
        gameOver = GameObject.Find("Game Over").GetComponent<Renderer>();
        youWon = GameObject.Find("You Won").GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("MainScene");
            }
        }

        if (youWon.enabled)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene("MainScene");
            }
        }

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

    public bool HasMushroom
    {
        get { return pickedMushroom.enabled; }
        set { pickedMushroom.enabled = value; }
    }

    public bool HasBerry
    {
        get { return pickedBerry.enabled; }
        set { pickedBerry.enabled = value; }
    }

    public bool HasKey
    {
        get { return pickedKey.enabled; }
        set { pickedKey.enabled = value; }
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

    public bool IsChestOpened
    {
        get { return chestOpened.enabled; }
        set { chestOpened.enabled = value; }
    }

    public bool IsFrog
    {
        get { return frog.enabled; }
        set { frog.enabled = value; }
    }

    public int CurrentHorizontalPosition
    {
        get { return Convert.ToInt32(this.transform.position.x); }
    }

    public int CurrentVerticalPosition
    {
        get { return Convert.ToInt32(this.transform.position.z); }
    }

    public int PlayerLives
    {
        get
        {
            if (life3.enabled)
            {
                return 3;
            }
            if (life2.enabled)
            {
                return 2;
            }
            if (life1.enabled)
            {
                return 1;
            }
            return 0;
        }
        set
        {
            life3.enabled = value >= 3;
            life2.enabled = value >= 2;
            life1.enabled = value >= 1;
            life3b.enabled = value >= 3;
            life2b.enabled = value >= 2;
            life1b.enabled = value >= 1;
        }
    }

    private void RevealBlock()
    {
        var currentBlock = GetCurrentBlock();

        currentBlock.GetComponent<Renderer>().material = dirtMaterial;

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

        if (item.Find("Mushroom") != null)
        {
            HasMushroom = true;
        }

        if (item.Find("Berry") != null)
        {
            HasBerry = true;
        }

        if (item.Find("Garlic") != null)
        {
            HasGarlic = true;
        }

        if (item.Find("Key") != null)
        {
            HasKey = true;
        }

        if (HasFlower && HasGarlic)
        {
            IsMouse = true;
            IsHuman = false;
            IsFrog = false;
            HasFlower = false;
            HasGarlic = false;
        }

        if (HasMushroom && HasBerry)
        {
            IsFrog = true;
            IsHuman = false;
            IsMouse = false;
            HasMushroom = false;
            HasBerry = false;
        }

        if (item.Find("Spider") != null)
        {
            PlayerLives--;
            if (PlayerLives == 0)
            {
                gameOver.enabled = true;
            }
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
        int faceDirection = 0;

        switch (direction)
        {
            case Direction.Up:
                verticalChange = 1;
                faceDirection = 180;
                break;
            case Direction.Down:
                verticalChange = -1;
                faceDirection = 0;
                break;
            case Direction.Right:
                horizontalChange = 1;
                faceDirection = 270;
                break;
            case Direction.Left:
                horizontalChange = -1;
                faceDirection = 90;
                break;
        }

        int nextHorizontal = CurrentHorizontalPosition + horizontalChange;
        int nextVertical = CurrentVerticalPosition + verticalChange;

        this.transform.eulerAngles = new Vector3(0, faceDirection, 0);

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

        if (IsFrog && nextBlock.CompareTag("Water"))
        {
            return true;
        }

        if (HasKey && nextBlock.CompareTag("Chest Block"))
        {
            IsChestOpened = true;
            HasKey = false;
            youWon.enabled = true;
            return false;
        }

        return false;
    }
}

public enum Direction
{
    Up, Down, Right, Left
}
