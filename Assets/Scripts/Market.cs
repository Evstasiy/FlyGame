using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Market
{
    public List<int> items = new List<int>();



    public void AddItem() {
        items.Add(Random.Range(1, 100));
    }
    public void AddItem(int count) {
        for (int i = 0; i < count; i++)
        {
            AddItem();
        }
    }

}
