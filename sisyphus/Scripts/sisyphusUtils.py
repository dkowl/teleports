import os

def replace(str, dict):
    for k, v in dict.items():
        key = "SIS_REPLACE({0})".format(k)
        value = v
        str = str.replace(key, value)
    return str

def ensureDirExists(dir):
    dir = os.path.dirname(dir)
    if not os.path.exists(dir):
        print("Creating directory: {0}".format(dir))
        os.makedirs(dir)

def ensureFileExists(filepath, defaultContent = ''):
    ensureDirExists(os.path.dirname(filepath))
    if not os.path.exists(filepath):
        print("Creating file: {0}".format(filepath))
        with open(filepath, 'w') as file:
            file.write(defaultContent)

def updateFile(filepath, newContent):
    oldContent = None
    if os.path.exists(filepath):
        with open(filepath, 'r') as file:        
            oldContent = file.read()

    if oldContent != newContent:
        with open(filepath, 'w') as file:
            file.write(newContent)
            print(os.path.basename(filepath) + " written.")
    else:
        print("No changes to " + os.path.basename(filepath) + ".")