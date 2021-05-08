import os
import Platform
import uuid
from xmlUtils import *
import logging
import sisyphusUtils as sis

def readProjectGuid(path):
    if os.path.exists(path):
        try:
            with open(path) as file:
                root = ET.parse(file).getroot()
                guidElem = root.find(".//{" + msbuildXmlNamespace + "}ProjectGuid")
                logging.debug("guid of " + os.path.basename(path) + " is " + guidElem.text)
                return uuid.UUID(guidElem.text)
        except Exception as e:
            logging.warning("Failed to read guid from " + str(path) + ": " + str(e))
    
    return uuid.uuid4()

def projectConfigurations(platform):
    root = ET.Element("ItemGroup")
    root.set("Label", "ProjectConfigurations")
    for config in platform.configurations:
        for arch in platform.architectures:
            projConfigElem = ET.SubElement(root, "ProjectConfiguration")
            projConfigElem.set("Include", config + "|" + arch)
            configElem = ET.SubElement(projConfigElem, "Configuration")
            configElem.text = config
            archElem = ET.SubElement(projConfigElem, "Platform")
            archElem.text = arch

    return root

# returns a list of copied file paths relative to rootDir
# Example 1: 
#   srcDir - project/assets/
#   dstDir - app/
#   rootDir - project/
# effect: files copied to app/assets
# return value: ['assets/file1.txt', 'assets/file2.txt'] ... ]
#
# Example 2: (with dstRootDir)
#   srcDir - project/assets/
#   dstDir - app/weapons/swords/
#   rootDir - project/assets/
#   dstRootDit - app/
# effect: files copied to app/weapons/swords/
# return value: ['weapons/swords/file1.txt', 'weapons/swords/file2.txt' ... ]
def copyDirContent(srcDir, dstDir, rootDir, dstRootDir = None):
    if not os.path.exists(srcDir):
        return []

    if not dstRootDir:
        dstRootDir = dstDir

    copiedPaths = []
    for dirpath, dirnames, filenames in os.walk(srcDir):
        for filename in filenames:
            srcPath = os.path.join(dirpath, filename)
            itemPath = os.path.relpath(srcPath, rootDir)
            dstPath = os.path.join(dstDir, itemPath)
            sis.copyFile(srcPath, dstPath, True)
            copiedPaths.append(os.path.relpath(dstPath, dstRootDir))

    return copiedPaths

def generateGitignore(dir, ignoredPaths):
    if len(ignoredPaths) == 0:
        return
    gitignoreContent = ''
    for path in ignoredPaths:
        gitignoreContent += path + '\n'
    # .gitignore only understands '/', not '\'
    gitignoreContent = gitignoreContent.replace('\\', '/')
    sis.updateFile(os.path.join(dir, '.gitignore'), gitignoreContent)

def getFilepathsRecursive(dir, rootDir = dir):
    paths = []
    for dirpath, dirnames, filenames in os.walk(dir):
        for filename in filenames:
            paths.append(os.path.relpath(os.path.join(dirpath, filename), rootDir))
    return paths