using UnityEngine;
using System.Collections;
using System;

public class MyHeap<T> where T : IHeapItem<T> {

    T[] items;
    int count;

    public MyHeap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = count;
        items[count] = item;
        SortUp(item);
        count++;

    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        count--;
        items[0] = items[count];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public int Count { get { return count; } }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childindexleft = item.HeapIndex * 2 + 1;
            int childindexright = item.HeapIndex * 2 + 2;
            int swapindex = 0;

            if (childindexleft < count)
            {
                swapindex = childindexleft;
                if (childindexright < count)
                {
                    if (items[childindexleft].CompareTo(items[childindexright]) < 0)
                    {
                        swapindex = childindexright;
                    }
                }
                if (item.CompareTo(items[swapindex]) < 0)
                {
                    Swap(item, items[swapindex]);
                }
                else
                    return;
            }
            else
                return;
        }
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parrentItem = items[parentIndex];
            if(item.CompareTo(parrentItem) > 0)
            {
                Swap(item, parrentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
