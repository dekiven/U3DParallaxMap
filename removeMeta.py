import os
from DKVTools.Funcs import *

for _dir, folders, files in os.walk(os.getcwd()) :
    for f in files :
        if f[-5:] == '.meta' :
            print(f)
            os.remove(pathJoin(_dir, f))