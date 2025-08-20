using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class NavAgent : MonoBehaviour
{
    private PriorityQueue<AstarNode> _priorityQueue;
    private List<Vector3Int> _visit;

    private List<Vector3Int> routePath;
    private int moveIdx = 0;

    private Vector3Int currentPos;
    private Vector3Int destinationPos;

    public Vector3Int Destination
    {
        get => destinationPos;
        set
        {
            if (destinationPos == value) return;    //목적지가 변했는지 확인

            SetCurrentPosition();
            destinationPos = value;
            CalculatePath();
        }
    }

    public bool GetNextPath(out Vector3Int nextPos)
    {
        nextPos = new Vector3Int();
        if (routePath.Count == 0 || moveIdx >= routePath.Count)
        {
            return false;
        }

        nextPos = routePath[moveIdx++];
        return true;
    }

    private LineRenderer lineRenderer;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        _visit = new List<Vector3Int>();
        routePath = new List<Vector3Int>();
        _priorityQueue = new PriorityQueue<AstarNode>();
    }

    private void Start()
    {
        SetCurrentPosition();
        transform.position = MapManager.Instance.GetWorldPosition(currentPos); //계산된 타일좌표의 중심으로 이동
    }

    private void PrintRoute()
    {
        lineRenderer.positionCount = routePath.Count;
        lineRenderer.SetPositions(routePath.Select(p => MapManager.Instance.GetWorldPosition(p)).ToArray());
    }

    private void SetCurrentPosition()
    {
        currentPos = MapManager.Instance.GetTilePos(transform.position);
    }

    #region Astar

    private void CalculatePath()
    {
        _priorityQueue.Clear();
        _visit.Clear();

        routePath = GetPath(currentPos, destinationPos);
    }

    public List<Vector3Int> GetPath(Vector3Int start, Vector3Int end, int maxPathDistance = 100)
    {
        if (_priorityQueue == null) _priorityQueue = new PriorityQueue<AstarNode>();
        _priorityQueue.Clear();
        List<Vector3Int> path = new List<Vector3Int>();

        Vector3Int currentPosition = start;
        AstarNode currentNode = new AstarNode(start, end, currentPosition, null);

        _priorityQueue.Enqueue(currentNode);

        while (_priorityQueue.IsEmpty() == false)
        {
            currentNode = _priorityQueue.Dequeue();
            currentPosition = new Vector3Int(currentNode.x, currentNode.y);

            if (currentPosition == end || currentNode.TotalDistance > maxPathDistance)
                break;

            for (int i = 0; i < 4; i++)
            {
                Vector3Int newPosition = currentPosition + NavDirection.directions[i];

                if (newPosition.x < 0 || newPosition.y < 0 || MapManager.Instance.CanMove((Vector3Int)newPosition) == false) continue;
                if (_visit.Contains(newPosition)) continue;

                _visit.Add(newPosition);
                _priorityQueue.Enqueue(new AstarNode(start, end, newPosition, currentNode));
            }
        }

        if (currentNode == null || new Vector3Int(currentNode.x, currentNode.y) != end)
        {
            Debug.LogWarning("Path not found");
            return path;
        }

        Stack<AstarNode> nodeStack = new Stack<AstarNode>();
        while (currentNode.prevNode != null)
        {
            nodeStack.Push(currentNode);
            currentNode = currentNode.prevNode;
        }

        while (nodeStack.Count > 0)
        {
            AstarNode node = nodeStack.Pop();
            path.Add(new Vector3Int(node.x, node.y));
        }

        return path;
    }

    #endregion
}

public static class NavDirection
{
    public static Vector3Int[] directions = new Vector3Int[4]
    {
            (Vector3Int)Vector2Int.up,
            (Vector3Int)Vector2Int.right,
            (Vector3Int)Vector2Int.down,
            (Vector3Int)Vector2Int.left
    };
}