import os
import shutil
from xmlUtils import *
from constants import *
import ProjectInfo
import sisyphusUtils as sis
from projCommon import *

def generateAndroidTestApp(platform, projectInfo):
    appDir = os.path.join(projectInfo.projDir(), "Android.Test", "App")
    srcDir = os.path.join(pythonSourceDir, "items", "AndroidTestApp")

    # other files
    filenames = [
        ("AndroidManifest.xml", "AndroidManifest.xml"),
        ("AndroidTestApp.java", os.path.join("src", "com", "{0}AndroidTestApp.java".format(projectInfo.name))),
        ("build.xml", "build.xml"),
        ("project.properties", "project.properties"),
        ("strings.xml", os.path.join("res", "values", "strings.xml"))
        ]

    replaceDict = {
        "APPNAME": projectInfo.testAppName(),
        "PROJNAME": projectInfo.name
        }

    for srcName, dstName in filenames:
        src = os.path.join(srcDir, srcName)
        dst = os.path.join(appDir, dstName)
        sis.ensureDirExists(dst)

        with open(src, 'r') as srcFile:
            content = srcFile.read()
            content = sis.replace(content, replaceDict)

        with open(dst, 'w') as dstFile:
            dstFile.write(content)

    # .androidproj
    root = ET.Element("Project")
    root.set("DefaultTargets", "Build")
    root.set("xmlns", msbuildXmlNamespace)
    root.append(projectConfigurations(platform))

    androidprojFilename = os.path.join(appDir, "{0}.androidproj".format(projectInfo.testAppName()))
    with open(androidprojFilename, 'w') as androidprojFile:
        androidprojFile.write(prettify(root))

    # TODO: rest of the .androidproj, try to be smart about reusing what was done for .vcxproj


