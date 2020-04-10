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
        ("Globals.Android.h", os.path.join('test', "Globals.Android.h"))
        ]

    replaceDict = {
        "APPNAME": projectInfo.testAppName()
        }

    sis.generateFiles(srcDir, dstDir, filenames, replaceDict)

