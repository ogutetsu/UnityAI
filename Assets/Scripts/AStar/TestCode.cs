using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{

    private Transform startPos, endPos;
    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    public ArrayList pathArray;

    private GameObject objStartCube, objEndCube;
    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        objStartCube = GameObject.FindGameObjectWithTag("Start");
        objEndCube = GameObject.FindGameObjectWithTag("End");

        pathArray = new ArrayList();
        FindPath();

    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
            Debug.Log("PathArray : " + pathArray.Count.ToString());
        }
    }

    void FindPath()
    {
        startPos = objStartCube.transform;
        endPos = objEndCube.transform;

        startNode = new Node(GridManager.Instance.GetGridCellCenter(GridManager.Instance.GetGridIndex(startPos.position)));

        goalNode = new Node(GridManager.Instance.GetGridCellCenter(GridManager.Instance.GetGridIndex(endPos.position)));

        pathArray = AStar.FindPath(startNode, goalNode);
    }


    void OnDrawGizmos()
    {
        if (pathArray == null) return;

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node) pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            }
        }
    }


}
