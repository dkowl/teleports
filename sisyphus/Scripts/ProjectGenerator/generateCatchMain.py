import os
from constants import *
import ProjectInfo

def generateCatchMain(projectInfo):
    filename = "catch.main.cpp"

    sourcePath = os.path.join(pythonSourceDir, "items", filename)
    destPath = os.path.join(solutionDir, projectInfo.name, "test", filename)

    with open(sourcePath, 'r') as sourceFile:
        content = sourceFile.read()
        content = content.replace("APPNAME", projectInfo.testAppName())

    with open(destPath, 'w') as destFile:
        destFile.write(content)
        print("{0}: catch main generated".format(projectInfo.name))