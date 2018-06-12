import os
from DKVTools.Funcs import *

ignoreList = [
    'Library/',
    'Temp/',
    'UnityPackageManager/',
    'obj/',
    '.vs/',
    'Travel.csproj',
    'Travel.sln',
]

f = open('.gitignore', 'w')

for i in ignoreList :
    f.write(i+'\n')
    
f.close()