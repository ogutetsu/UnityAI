using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static PriorityQueue openList;
    public static HashSet<Node> closedList;


    private static float HeuristicEstimateCost(Node curNode, Node goalNode)
    {
        Vector3 vecCost = curNode.position - goalNode.position;
        return vecCost.magnitude;
    }

    public static ArrayList FindPath(Node start, Node goal)
    {
        openList = new PriorityQueue();
        openList.Push(start);
        start.nodeTotalCost = 0.0f;
        start.estimatedCost = HeuristicEstimateCost(start, goal);

        closedList = new HashSet<Node>();
        Node node = null;

        while (openList.Length != 0)
        {
            node = openList.First();
            if (node.position == goal.position)
            {
                Debug.Log("goal : " + goal.position.ToString());
                return CalculatePath(node);
            }

            ArrayList neighbors = new ArrayList();
            
            GridManager.Instance.GetNeighbours(node, neighbors);

            for (int i = 0; i < neighbors.Count; i++)
            {
                Node neighborNode = (Node) neighbors[i];

                if (!closedList.Contains(neighborNode))
                {
                    float cost = HeuristicEstimateCost(node, neighborNode);

                    float totalCost = node.nodeTotalCost + cost;
                    float neighborNodeEstCost = HeuristicEstimateCost(neighborNode, goal);

                    neighborNode.nodeTotalCost = totalCost;
                    neighborNode.parent = node;
                    neighborNode.estimatedCost = totalCost + neighborNodeEstCost;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Push(neighborNode);
                    }
                }
            }

            closedList.Add(node);
            openList.Remove(node);
        }

        if (node.position != goal.position)
        {
            Debug.LogError("Goal Not Found");
            return null;
        }

        return CalculatePath(node);
    }


    private static ArrayList CalculatePath(Node node)
    {
        ArrayList list = new ArrayList();
        while (node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

}
