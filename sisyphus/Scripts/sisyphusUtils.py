import os
import logging
logger = logging.getLogger()

def replace(str, dict):
    for k, v in dict.items():
        key = "SIS_REPLACE({0})".format(k)
        value = v
        str = str.replace(key, value)
    return str

def ensureDirExists(dir):
    dir = os.path.dirname(dir)
    if not os.path.exists(dir):
        logger.info("Creating directory: {0}".format(dir))
        os.makedirs(dir)

def ensureFileExists(filepath, defaultContent = ''):
    ensureDirExists(os.path.dirname(filepath))
    if not os.path.exists(filepath):
        logger.info("Creating file: {0}".format(filepath))
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
            logger.info(os.path.basename(filepath) + " written.")
    else:
        logger.debug("No changes to " + os.path.basename(filepath) + ".")

def getSubdirectories(dir):
    return next(os.walk(dir))[1]

def appendToFilename(path, prefix):
    dir, filename = os.path.split(path)
    filename = prefix + filename
    return os.path.join(dir, filename)

# filenames is a list fo tuples in the form of (srcName, dstName)
# replaceDict is used when executing the SIS_REPLACE() macro
def generateFiles(srcDir, dstDir, filenames, replaceDict):
    gitignoreContent = ''
    for srcName, dstName in filenames:
        src = os.path.join(srcDir, srcName)
        dst = os.path.join(dstDir, dstName)
        ensureDirExists(dst)

        gitignoreContent += '/' + dstName + '\n'

        with open(src, 'r') as srcFile:
            content = srcFile.read()
            content = replace(content, replaceDict)

        updateFile(dst, content)

    # .gitignore only understands '/', not '\'
    gitignoreContent = gitignoreContent.replace('\\', '/')
    updateFile(os.path.join(dstDir, '.gitignore'), gitignoreContent)