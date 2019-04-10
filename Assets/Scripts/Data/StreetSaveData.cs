using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetSaveData {
    public Queue<ShopData> shopDatas = new Queue<ShopData>();
    public Queue<string> streetCfgs = new Queue<string>();
    public string layoutId;
    public bool isEntrance = false;
    public List<int> tmpList = new List<int>();
    public Vector3 localPosition;
}

