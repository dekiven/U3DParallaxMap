import os
from DKVTools.Funcs import *

ignoreList = [
    'Library/',
    'Temp/',
    'UnityPackageManager/',
    'obj/',
    'Assets/test/\n'
    'Assets/test.meta\n'
    '.vs/',
    '*.csproj',
    '*.sln',
]

f = open('.gitignore', 'w')

for i in ignoreList :
    f.write(i+'\n')
    
f.close()