using System.Collections.Generic;

// 从Asset中读取一个T 的list配置
public class BaseDataList<T> : BaseConfig where T : BaseData
{

    public List<T> DataList;

    public BaseDataList()
    {
        DataList = new List<T>();
    }
}
