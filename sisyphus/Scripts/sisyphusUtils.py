import os
import shutil
import sys
import logging
logger = logging.getLogger()

def replace(str, dict):
    for k, v in dict.items():
        key = "SIS_REPLACE({0})".format(k)
        value = v
        str = str.replace(key, value)
    return str

def ensureDirExists(dir):
    if os.path.isfile(dir):
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

# example
#   src = project/assets
#   dst = app
#   result - symlink at app/assets
# returns the symlink path
def ensureSymlinkExists(src, dst):
    src = os.path.abspath(src)
    dst = os.path.abspath(dst)
    dst = os.path.join(dst, os.path.basename(src))

    if sys.version_info < (3, 8):
        logging.warn("Your python version is <3.8, creating symlinks may not work properly. \
            make sure to run Windows in developer mode and install python 3.8 or greater.")

    if not os.path.exists(src):
        return None

    ensureDirExists(os.path.dirname(dst))

    if os.path.islink(dst):
        prevSrc = os.readlink(dst)
        if not os.path.isabs(prevSrc):
            prevSrc = os.path.abspath(os.path.join(os.path.dirname(dst), os.readlink(dst)))
        if prevSrc == src:
            # correct symlink already there, nothing to do
            return dst
        logging.info(f'symlink at {dst} alrady exists, but points to {prevSrc} instead of {src}, deleting...')
        os.unlink(dst)
    elif os.path.isfile(dst):
        logging.info(f'{dst} already exists and is a file, deleting...')
        os.remove(dst)
    elif os.path.isdir(dst):
        logging.info(f'{dst} already exists and is a directory, deleting...')
        shutil.rmtree(dst)

    # for some reason creating a symlink with an absolute path produces weird results during os.readlink()
    relSrc = os.path.relpath(src, os.path.dirname(dst))
    logging.info(f'Creating symlink to {relSrc} at {dst}')
    os.symlink(relSrc, dst, True)
    return dst

def getFileContent(filepath, binary = False):
    content = None
    if os.path.exists(filepath):
        with open(filepath, 'rb' if binary else 'r') as file:        
            content = file.read()
    return content

def writeFile(filepath, content, binary = False):
    ensureDirExists(filepath)
    with open(filepath, 'wb' if binary else 'w') as file:
        file.write(content)
        logger.info(filepath + " written.")

def updateFile(filepath, newContent, binary = False):
    oldContent = getFileContent(filepath, binary)

    if oldContent != newContent:
        writeFile(filepath, newContent, binary)
    else:
        logger.debug("No changes to " + os.path.basename(filepath) + ".")

def appendFile(filepath, newContent, binary = False):
    oldContent = getFileContent(filepath, binary)

    if oldContent != None:
        writeFile(filepath, oldContent + newContent, binary)
    else:
        writeFile(filepath, newContent, binary)

def copyFile(src, dst, binary = False):
    with open(src, 'rb' if binary else 'r') as file:
        content = file.read()
    updateFile(dst, content, binary)

def getSubdirectories(dir):
    return next(os.walk(dir))[1]

def appendToFilename(path, prefix):
    dir, filename = os.path.split(path)
    filename = prefix + filename
    return os.path.join(dir, filename)

# filenames is a list fo tuples in the form of (srcName, dstName)
# replaceDict is used when executing the SIS_REPLACE() macro
def generateFiles(srcDir, dstDir, filenames, replaceDict):
    copiedFiles = []
    for srcName, dstName in filenames:
        src = os.path.join(srcDir, srcName)
        dst = os.path.join(dstDir, dstName)
        ensureDirExists(dst)

        with open(src, 'r') as srcFile:
            content = srcFile.read()
            content = replace(content, replaceDict)

        updateFile(dst, content)
        copiedFiles.append(dstName)
    return copiedFiles

def formatGuid(guid):
    guidStr = str(guid)
    guidStr = guidStr.strip('{}')
    return '{%s}' % guidStr.upper()