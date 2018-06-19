using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//游戏对象显示组件
public class RenderObj : MonoBehaviour {
    //暂定是显示Sprite
    public SpriteRenderer ObjSprie;
    //public float HP;
    //public ObjData Data;


    public bool InitByID(string id)
    {
        if(null == ObjSprie)
        {
            ObjSprie = gameObject.AddComponent<SpriteRenderer>();
            var data = DataManagers.Instance.ObjDataManager.GetDataById(id);
            if (null == data)
            {
                return false;
            }
            var path = data.Sprite;
            var split = path.Split('/');
            path = string.Format("{0}/{1}/{2}", split[0], split[1], split[2]);
            var file = "";
            for (int i = 3; i < split.Length; ++i)
            {
                file += split[i];;
                if (i != split.Length - 1)
                {
                    file += "/";
                }
            }
            //file = file.Substring(0, file.Length - 1);
            //TODO:LoadRes 优化： 1.传入一个文件参数分割成两个
            GameResManager.Instance.LoadRes<Sprite>(path, file, delegate(UnityEngine.Object obj) {
            
                if (null != obj)
                {
                    ObjSprie.sprite = obj as Sprite;
                }
            });
        }
        return true;
    }

    //public 
}
