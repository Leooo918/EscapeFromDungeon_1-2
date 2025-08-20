using System;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T> where T : IComparable<T>
{
    private List<T> _heap = new List<T>();

    //O(log N)
    public void Enqueue(T item)
    {
        _heap.Add(item);
        int last = _heap.Count - 1;

        while (last > 0)
        {
            int mid = (last - 1) / 2;
            if (_heap[last].CompareTo(_heap[mid]) >= 0)
                break;

            Swap(last, mid);
            last = mid;
        }
    }

    //O(log N)
    public T Dequeue()
    {
        if (IsEmpty())
        {
            Debug.LogError("PriorityQueue is empty but you still try to dequeue");
            return default(T);
        }

        T root = _heap[0];
        T last = _heap[_heap.Count - 1];
        _heap.RemoveAt(_heap.Count - 1);

        if (!IsEmpty())
        {
            _heap[0] = last;
            Update(0);
        }

        return root;
    }

    public bool IsEmpty()
        => _heap.Count == 0;

    public void Clear()
        => _heap.Clear();



    private void Swap(int i, int j)
    {
        T temp = _heap[i];
        _heap[i] = _heap[j];
        _heap[j] = temp;
    }

    private void Update(int index)
    {
        int smallest = index;
        int left = 2 * index + 1;
        int right = 2 * index + 2;

        if (left < _heap.Count && _heap[left].CompareTo(_heap[smallest]) < 0)
            smallest = left;

        if (right < _heap.Count && _heap[right].CompareTo(_heap[smallest]) < 0)
            smallest = right;

        if (smallest != index)
        {
            Swap(index, smallest);
            Update(smallest);
        }
    }
}