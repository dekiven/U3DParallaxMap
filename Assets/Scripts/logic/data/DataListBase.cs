using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 从Asset中读取一个T 的list配置
public class DataListBase<T> : BaseConfig where T : BaseData
{

    public List<T> DataList;

    public DataListBase()
    {
        DataList = new List<T>();
    }
}
