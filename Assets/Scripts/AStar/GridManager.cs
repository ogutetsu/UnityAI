using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager instance = null;

    public static GridManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GridManager>();
                if (instance == null)
                {
                    Debug.LogError("GridManager in the scene.");
                }
            }

            return instance;
        }
    }


    public int numOfRows;
    public int numOfColumns;
    public float gridCellSize;
    public bool showGrid = true;
    public bool showObstacleBlocks = true;

    private Vector3 origin = new Vector3();
    private GameObject[] obstacleList;
    public Node[,] nodes { get; set; }
    public Vector3 Origin
    {
        get { return origin; }
    }

    void Awake()
    {
        obstacleList = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();

    }

    void CalculateObstacles()
    {
        nodes = new Node[numOfColumns,numOfRows];
        int index = 0;
        for (int i = 0; i < numOfColumns; i++)
        {
            for (int j = 0; j < numOfRows; j++)
            {
                Vector3 cellPos = GetGridCellCenter(index);
                Node node = new Node(cellPos);
                nodes[i, j] = node;
                index++;
            }
        }

        if (obstacleList != null && obstacleList.Length > 0)
        {
            foreach (var data in obstacleList)
            {
                int indexCell = GetGridIndex(data.transform.position);
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                nodes[row, col].MarkAsObstacle();
                Debug.Log("Obstacles : " + col.ToString() + " : " + row.ToString());
            }
        }

    }

    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (gridCellSize / 2.0f);
        cellPosition.z += (gridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * gridCellSize;
        float zPosInGrid = row * gridCellSize;
        return Origin + new Vector3(xPosInGrid, 0.0f, zPosInGrid);
    }


    public int GetGridIndex(Vector3 pos)
    {
        if (!IsInBounds(pos))
        {
            return -1;
        }

        pos -= Origin;
        int col = (int) (pos.x / gridCellSize);
        int row = (int) (pos.z / gridCellSize);
        return (row * numOfColumns + col);
    }

    public bool IsInBounds(Vector3 pos)
    {
        float width = numOfColumns * gridCellSize;
        float height = numOfRows * gridCellSize;
        return (pos.x >= Origin.x && pos.x <= Origin.x + width &&
                pos.z <= Origin.z + height && pos.z >= Origin.z);
    }


    public int GetRow(int index)
    {
        int row = index / numOfColumns;
        return row;
    }

    public int GetColumn(int index)
    {
        int column = index % numOfColumns;
        return column;
    }

    public void GetNeighbours(Node node, ArrayList neighbors)
    {
        Vector3 neighborPos = node.position;
        int neighborIndex = GetGridIndex(neighborPos);

        int row = GetRow(neighborIndex);
        int column = GetColumn(neighborIndex);

        int leftNodeRow = row - 1;
        int leftNodeColumn = column;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);

        leftNodeRow = row + 1;
        leftNodeColumn = column;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);

        leftNodeRow = row;
        leftNodeColumn = column + 1;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);

        leftNodeRow = row;
        leftNodeColumn = column - 1;
        AssignNeighbor(leftNodeRow, leftNodeColumn, neighbors);



    }

    void AssignNeighbor(int row, int column, ArrayList neighbors)
    {
        if (row != -1 && column != -1 &&
            row < numOfRows && column < numOfColumns)
        {
            //Debug.Log("row : " + row.ToString() + " column : " + column.ToString());
            Node nodeToAdd = nodes[row, column];
            if (!nodeToAdd.bObstacle)
            {
                neighbors.Add(nodeToAdd);
            }
        }
    }

    void OnDrawGizmos()
    {
        if (showGrid)
        {
            DebugDrawGrid(transform.position, numOfRows, numOfColumns, gridCellSize, Color.blue);
        }
        Gizmos.DrawSphere(transform.position, 0.5f);
        if (showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(gridCellSize, 1.0f, gridCellSize);
            if (obstacleList != null && obstacleList.Length > 0)
            {
                foreach (var data in obstacleList)
                {
                    Gizmos.DrawCube(GetGridCellCenter(
                        GetGridIndex(data.transform.position)), cellSize);
                }
            }
        }
    }

    public void DebugDrawGrid(Vector3 origin, int numRows, int numCols, float cellSize, Color color)
    {
        float width = (numCols * cellSize);
        float height = (numRows * cellSize);
        for (int i = 0; i < numRows +1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 endPos = startPos + width * new Vector3(1.0f, 0.0f, 0.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

        for (int i = 0; i < numCols + 1; i++)
        {
            Vector3 startPos = origin + i * cellSize * new Vector3(1.0f, 0.0f, 0.0f);
            Vector3 endPos = startPos + height * new Vector3(0.0f, 0.0f, 1.0f);
            Debug.DrawLine(startPos, endPos, color);
        }

    }


}
