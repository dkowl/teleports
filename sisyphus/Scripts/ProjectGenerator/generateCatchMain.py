import os
import logging
import sisyphusUtils as sis
from constants import *
import ProjectInfo

def generateCatchMain(projectInfo):
    for filename in ["catch.main.cpp", "Globals.Android.h"]:
        sourcePath = os.path.join(pythonSourceDir, "items", filename)
        destPath = os.path.join(solutionDir, projectInfo.name, "test", filename)

        with open(sourcePath, 'r') as sourceFile:
            content = sourceFile.read()
            content = content.replace("APPNAME", projectInfo.testAppName())

        sis.updateFile(destPath, content)