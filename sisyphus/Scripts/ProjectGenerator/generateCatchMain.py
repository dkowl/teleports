import os
import logging
import sisyphusUtils as sis
import constants
import ProjectInfo

def generateCatchMain(projectInfo):
    srcDir = os.path.join(constants.pythonSourceDir, "items")
    dstDir = projectInfo.projDir()

    filenames = [
        ("catch.main.cpp", os.path.join('test', "catch.main.cpp")),
        ("catch.listener.cpp", os.path.join('test', "catch.listener.cpp")),
        ("catch.globals.h", os.path.join('test', "catch.globals.h")),
        ("Globals.Android.h", os.path.join('test', "Globals.Android.h"))
        ]

    replaceDict = {
        "APPNAME": projectInfo.testAppName(),
        "PROJNAME": projectInfo.name
        }

    sis.generateFiles(srcDir, dstDir, filenames, replaceDict)

