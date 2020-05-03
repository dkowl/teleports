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

def copyTestDataContent(platform, projectInfo, appDir):
    testDataDir = os.path.join(projectInfo.projDir(), 'test_data')

    if not os.path.exists(testDataDir):
        return ''

    copiedPaths = []
    for dirpath, dirnames, filenames in os.walk(testDataDir):
        for filename in filenames:
            srcPath = os.path.join(dirpath, filename)
            itemPath = os.path.relpath(srcPath, projectInfo.projDir())
            if platform.name == 'Android':
                itemPath = os.path.join('assets', itemPath)
            dstPath = os.path.join(appDir, itemPath)
            sis.copyFile(srcPath, dstPath, True)
            copiedPaths.append(itemPath)

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
