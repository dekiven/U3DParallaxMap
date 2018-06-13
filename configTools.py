import os
from DKVTools.Funcs import *
import json

# import DKVTools
# help(DKVTools.Funcs)

curDir = os.getcwd()


def genXAllConfig(folder) :
    configPath = pathJoin(curDir, 'Assets/Res/conf/'+folder)
    resPath = pathJoin(curDir, 'Assets/Res')
    # print(configPath)
    if os.path.isdir(configPath) :
        name = folder+'All.json'
        conf = {}
        fialed = []
        for _dir, folders, files in os.walk(configPath) :
            for f in files :
                if f[-5:] == '.json' and f != name :
                    p = pathJoin(_dir, f)
                    d = json2Py(p)
                    i = d.get("ID")
                    if d and i :
                        conf[i] = d
                        # conf[i] = getRelativePath(p, resPath)[0]
                    else :
                        fialed.append(p)
        # print(conf)
        print('fialed:', fialed)

        s = json.dumps(conf, ensure_ascii=False, indent=2)
        if isinstance(s, unicode) :
            s = s.encode('utf-8')
        
        f = None
        if isPython3() :
            f = open(pathJoin(configPath, name), 'w', encoding='utf-8')
        else :
            f = open(pathJoin(configPath, name), 'w')
        f.write(s)
        f.close()

def json2Py(path) :
    rst = None
    if os.path.isfile(path) :
        f = open(path, 'rb')
        try :
            rst = json.loads(bytes2utf8Str(f.read(), 'utf-8'))
        except Exception as e :
            print(e)
        finally :
            f.close()
    return rst

def main():
    folders = ('obj', )
    for f in folders :
        genXAllConfig(f)
     


if __name__ == '__main__':
    main()