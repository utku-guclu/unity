using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph
{
    List<Edge> edges = new List<Edge>();
    List<Node> nodes = new List<Node>();
    public List<Node> pathList = new List<Node>();

    public Graph()
    {

    }
    public void AddNode(GameObject id)
    {
        Node node = new Node(id);
        nodes.Add(node);
    }

    public void AddEdge(GameObject fromNode, GameObject toNode)
    {
        Node from = FindNode(fromNode);
        Node to = FindNode(toNode);

        if (from != null && to != null)
        {
            Edge e = new Edge(from, to);
            edges.Add(e);
            from.edgelist.Add(e);
        }
    }
    Node FindNode(GameObject id)
    {
        foreach (Node n in nodes)
        {
            if (n.getId() == id)
                return n;
        }
        return null;
    }

    public bool Astar(GameObject startId, GameObject endId)
    {

        if (startId == endId)
        {
            pathList.Clear();
            return false;
        }

        Node start = FindNode(startId);
        Node end = FindNode(endId);

        if (start == null || end == null)
        {
            return false;
        }

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        float tentative_g_scores = 0;
        bool tentative_is_better;

        start.g = 0;
        start.h = distance(start, end);
        start.f = start.h;

        open.Add(start);
        while (open.Count > 0)
        {
            int i = lowestF(open);
            Node thisNode = open[i];
            if (thisNode.getId() == endId)
            {
                ReconstructPath(start, end);
                return true;
            }
            open.RemoveAt(i);
            closed.Add(thisNode);
            Node neighbor;
            foreach (Edge e in thisNode.edgelist)
            {
                neighbor = e.endNode;
                if (closed.IndexOf(neighbor) > -1)
                    continue;

                tentative_g_scores = thisNode.g + distance(thisNode, neighbor);
                if (open.IndexOf(neighbor) == -1)
                {
                    open.Add(neighbor);
                    tentative_is_better = true;
                }
                else if (tentative_g_scores < neighbor.g)
                {
                    tentative_is_better = true;
                }
                else
                {
                    tentative_is_better = false;
                }
                if (tentative_is_better)
                {
                    neighbor.cameFrom = thisNode;
                    neighbor.g = tentative_g_scores;
                    neighbor.h = distance(thisNode, end);
                    neighbor.f = neighbor.g + neighbor.h;
                }
            }
        }
        return false;
    }

    public void ReconstructPath(Node startId, Node endId)
    {
        pathList.Clear();
        pathList.Add(endId);

        var p = endId.cameFrom;
        while (p != startId && p != null)
        {
            pathList.Insert(0, p);
            p = p.cameFrom;
        }
        pathList.Insert(0, startId);
    }

    float distance(Node a, Node b)
    {
        return (Vector3.SqrMagnitude(a.getId().transform.position - b.getId().transform.position));
    }
    int lowestF(List<Node> l)
    {
        float lowestf = 0;
        int count = 0;
        int iteratorCount = 0;

        lowestf = l[0].f;

        for (int i = 1; i < l.Count; i++)
        {
            if (l[i].f < lowestf)
            {
                lowestf = l[i].f;
                iteratorCount = count;
            }
            count++;
        }
        return iteratorCount;
    }
}