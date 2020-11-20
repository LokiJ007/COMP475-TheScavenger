using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    public int columns = 8;  
    public int rows = 8;
    private int cellLength = 10;
    //public Count wallCount = new Count(5, 9);
    //public Count foodCount = new Count(1, 5);
    public GameObject[] exit;
    public GameObject[] torch;
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] sodaTiles;
    public GameObject[] enemy1Tiles;
    public GameObject[] enemy2Tiles;
    public GameObject[] enemy3Tiles;
    public GameObject[] moneyTiles;
    public GameObject[] bagTiles;
    public GameObject[] outerWallTiles;

    private Transform boardHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private char[,] gameMap = new char[20, 30];
    //{
    //    { 'X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X' },
    //    { 'X','2',' ',' ',' ',' ',' ',' ','E','X','X','B',' ',' ','S','W',' ',' ',' ','X','T',' ',' ',' ',' ',' ',' ',' ','S','X' },
    //    { 'X',' ',' ',' ',' ',' ',' ',' ',' ','X','X',' ',' ',' ','1','W',' ',' ',' ',' ',' ',' ',' ','2',' ',' ',' ',' ',' ','X' },
    //    { 'X',' ',' ',' ',' ','W','W','W','W','X','X','F',' ',' ','W','W',' ',' ',' ','X','X','F','W',' ',' ','W',' ',' ','B','X' },
    //    { 'X',' ',' ','1',' ','W','S',' ','B','X','X',' ',' ','1',' ',' ',' ',' ',' ','X','X',' ','W','F',' ','W','2',' ',' ','X' },
    //    { 'X','3',' ',' ',' ','W',' ',' ',' ','X','X',' ',' ',' ',' ','W',' ','2',' ','X','X',' ','W',' ',' ','W',' ',' ',' ','X' },
    //    { 'X','F',' ','M',' ',' ',' ',' ','F','X','X',' ',' ',' ',' ','W','W','W','W','X','X',' ',' ',' ',' ',' ',' ',' ',' ','X' },
    //    { 'X',' ',' ',' ',' ','W','W','W','W','X','T',' ',' ',' ',' ','2',' ',' ','S','X','X',' ',' ',' ',' ',' ','W','W','W','X' },
    //    { 'X',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X','X',' ',' ',' ',' ',' ',' ',' ',' ','X' },
    //    { 'X','X','X','X','X','X','X','X','X','X','X','X','X','X',' ','X','X','X','X','X','X','X','X','X','X','X','X','X',' ','X' },
    //    { 'X','X','X','X','X','X','X','X','X','X','X','X','X','X',' ','X','X','X','X','X','X','X','X','X','X','X','X','T',' ','X' },
    //    { 'X','S','W',' ',' ',' ',' ',' ',' ','X','X',' ',' ','W','3','W',' ',' ',' ','X','X',' ',' ','B',' ',' ',' ',' ',' ','X' },
    //    { 'X','W','W',' ',' ',' ',' ',' ',' ','X','X',' ',' ','W','W','W',' ',' ',' ','X','X','S','W',' ',' ',' ','1',' ',' ','X' },
    //    { 'X',' ',' ',' ',' ','F',' ',' ',' ','T','X',' ',' ',' ',' ',' ',' ','F',' ','X','X',' ','W',' ',' ',' ',' ',' ',' ','X' },
    //    { 'X',' ',' ','W','W','W',' ',' ',' ',' ',' ',' ',' ',' ',' ','W','W','W',' ','X','X',' ',' ',' ',' ','W','W','W','W','X' },
    //    { 'X',' ',' ',' ',' ',' ',' ','W','W','X','X','W','W',' ',' ',' ',' ',' ','M','X','X',' ',' ','1',' ','W',' ',' ','F','X' },
    //    { 'X',' ',' ','F',' ',' ',' ',' ',' ','X','X',' ',' ',' ',' ',' ','1',' ',' ','X','X',' ',' ',' ',' ',' ',' ',' ',' ','X' },
    //    { 'X',' ',' ',' ',' ',' ',' ',' ',' ','X','X','S',' ',' ',' ','W','W',' ',' ','T','X','W','W',' ',' ',' ','M',' ',' ','X' },
    //    { 'X','P',' ',' ',' ',' ',' ',' ',' ','X','X',' ',' ',' ',' ',' ',' ',' ','F',' ',' ',' ',' ',' ',' ',' ',' ',' ',' ','X' },
    //    { 'X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X','X' }
    //}; //I was in that special mood where doing the hard thing was eaisier than doing the easy thing



    // Start is called before the first frame update
    void Awake()
    {
        int k = 0;
        char[] nibbleMap;
        string[] lines = File.ReadAllLines("Assets/Resources/MAP.txt");
        foreach (string line in lines)
        {
            nibbleMap = (line.ToCharArray());
            for (int j = 0; j < 30; j++)
            {
                gameMap[k, j] = nibbleMap[j];
                //Debug.Log(nibbleMap[j]);
            }
            k++;
        }
    }

    void obliterateBoard(string obTag) //Might be able to use this to remap the array
    {
        GameObject[] things = GameObject.FindGameObjectsWithTag(obTag);
        foreach(GameObject thing in things)
        {
            GameObject.Destroy(thing);
        }
    }

    void clearBoard()
    {
        obliterateBoard("Enemy");
        obliterateBoard("OuterWall");
        obliterateBoard("Food");
        obliterateBoard("Soda");
        obliterateBoard("Floor");
        obliterateBoard("Wall");
        obliterateBoard("Exit");
        obliterateBoard("Torch");
        obliterateBoard("Money");
        obliterateBoard("Bag");
    }

    void saveLoc(string obTag, int xOffset, int yOffset)
    {
        char mapLetter = ' ';
        switch (obTag) 
        {
            case "Enemy":
                mapLetter = '1';
                break;
            case "Food":
                mapLetter = 'F';
                break;
            case "Soda":
                mapLetter = 'S';
                break;
            case "Wall":
                mapLetter = 'W';
                break;
        }

        GameObject[] things = GameObject.FindGameObjectsWithTag(obTag);
        foreach (GameObject thing in things)
        {
            int x = (int)thing.transform.position.x;
            int y = (int)thing.transform.position.y;
            if (gameMap[(8 - y) + (cellLength * xOffset), (x + 1) + (cellLength * yOffset)] != mapLetter)
            gameMap[(8 - y) + (cellLength * xOffset), (x + 1) + (cellLength * yOffset)] = mapLetter;
        }
    }

    public void saveBoard(int xOffset, int yOffset)
    {
        saveLoc("Enemy", xOffset, yOffset);
        saveLoc("Food", xOffset, yOffset);
        saveLoc("Soda", xOffset, yOffset);
        saveLoc("Wall", xOffset, yOffset);
    }

    void BoardSetup(int xOffset, int yOffset)
    {
        //boardHolder = new GameObject("Board").transform;

        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if ((x == -1 || x == columns || y == -1 || y == rows) && (gameMap[(8 - y) + (cellLength * xOffset), (x + 1) + (cellLength * yOffset)] == 'X'))
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];
                }

                GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

            }
        }
    }


    void LayoutObject(GameObject[] tiles, char tileChar, int xOffset, int yOffset)
    {
        GameObject tileChoice = tiles[Random.Range(0, tiles.Length)];
        for (int x = -1; x < columns + 1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                if ((gameMap[(8-y) + (cellLength* xOffset), (x + 1) + (cellLength * yOffset)] == tileChar))
                {
                    tileChoice = tiles[Random.Range(0, tiles.Length)];
                    GameObject instance = Instantiate(tileChoice, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                    if (tileChar == 'K')
                        Debug.Log("T");
                }
            }
        }
    }

    public void SetupScene(int xOffset, int yOffset)
    {
        //saveBoard(xOffset, yOffset);
        clearBoard();
        BoardSetup(xOffset, yOffset);
        LayoutObject(wallTiles, 'W', xOffset, yOffset);
        LayoutObject(foodTiles, 'F', xOffset, yOffset);
        LayoutObject(sodaTiles, 'S', xOffset, yOffset);
        LayoutObject(exit, 'E', xOffset, yOffset);
        LayoutObject(enemy1Tiles, '1', xOffset, yOffset);
        LayoutObject(enemy2Tiles, '2', xOffset, yOffset);
        LayoutObject(enemy3Tiles, '3', xOffset, yOffset);
        LayoutObject(moneyTiles, 'M', xOffset, yOffset);
        LayoutObject(bagTiles, 'B', xOffset, yOffset);
        LayoutObject(torch, 'K', xOffset, yOffset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
