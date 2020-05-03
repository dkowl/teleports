import os
import shutil
from xmlUtils import *
from constants import *
import ProjectInfo
import sisyphusUtils as sis
from projCommon import *
import SolutionCommon
import SolutionProject

def appendTestDataContent(projectInfo, itemGroup, appDir):
    testDataDir = os.path.join(projectInfo.projDir(), 'test_data')

    if not os.path.exists(testDataDir):
        return

    paths = []
    for dirpath, dirnames, filenames in os.walk(testDataDir):
        for filename in filenames:
            srcPath = os.path.join(dirpath, filename)
            itemPath = os.path.join('assets', os.path.relpath(srcPath, projectInfo.projDir()))
            dstPath = os.path.join(appDir, itemPath)
            sis.copyFile(srcPath, dstPath, True)
            contentElem = ET.SubElement(itemGroup, "Content")
            contentElem.set("Include", str(itemPath))

    sis.appendFile(os.path.join(appDir, '.gitignore'), 'assets/test_data*')

def generateAndroidTestApp(platform, projectInfo):
    libDir = os.path.join(projectInfo.projDir(), "Android.Test")
    appDir = os.path.join(libDir, "App")
    srcDir = os.path.join(pythonSourceDir, "items", "AndroidTestApp")

    # other files
    filenames = [
        ("AndroidManifest.xml", "AndroidManifest.xml"),
        ("AndroidTestApp.java", os.path.join("src", "com", projectInfo.testAppName(), "{0}.java".format(projectInfo.testAppName()))),
        ("build.xml", "build.xml"),
        ("project.properties", "project.properties"),
        ("strings.xml", os.path.join("res", "values", "strings.xml"))
        ]

    replaceDict = {
        "APPNAME": projectInfo.testAppName(),
        "PROJNAME": projectInfo.name
        }

    sis.generateFiles(srcDir, appDir, filenames, replaceDict)

    # .androidproj    
    androidprojFilename = os.path.join(appDir, "{0}.androidproj".format(projectInfo.testAppName()))
    projGuid = readProjectGuid(androidprojFilename)

    root = ET.Element("Project")
    root.set("DefaultTargets", "Build")
    root.set("xmlns", msbuildXmlNamespace)
    root.append(projectConfigurations(platform))

    # globals
    globalsElem = ET.Element("PropertyGroup")
    globalsElem.set("Label", "Globals")

    guidElem = ET.SubElement(globalsElem, "ProjectGuid")
    guidElem.text = sis.formatGuid(projGuid)

    rootNsElem = ET.SubElement(globalsElem, "RootNamespace")
    rootNsElem.text = projectInfo.testAppName()

    packagingProjWithoutNativeCompElem = ET.SubElement(globalsElem, "_PackagingProjectWithoutNativeComponent")
    packagingProjWithoutNativeCompElem.text = "true"

    launchActivityElem = ET.SubElement(globalsElem, "LaunchActivity")
    launchActivityElem.set("Condition", "'$(LaunchActivity)' == ''")
    launchActivityElem.text = "com.{0}.{0}".format(projectInfo.testAppName())

    javaSrcElem = ET.SubElement(globalsElem, "JaveSourceRoots")
    javaSrcElem.text = "src"

    root.append(globalsElem)


    importElem1 = ET.SubElement(root, "Import")
    importElem1.set("Project", "$(AndroidTargetsPath)\Android.Default.props")

    configGroup = ET.SubElement(root, "PropertyGroup")
    configGroup.set("Label", "Configuration")

    targetNameElem = ET.SubElement(configGroup, "TargetName")
    targetNameElem.text = "$(RootNamespace)"

    configTypeElem = ET.SubElement(configGroup, "ConfigurationType")
    configTypeElem.text = "Application"

    useDebugElem_Debug = ET.SubElement(configGroup, "UseDebugLibraries")
    useDebugElem_Debug.set("Condition", "'$(Configuration)'=='Debug'")
    useDebugElem_Debug.text = "true"

    useDebugElem_Release = ET.SubElement(configGroup, "UseDebugLibraries")
    useDebugElem_Release.set("Condition", "'$(Configuration)'=='Release'")
    useDebugElem_Release.text = "false"

    importElem2 = ET.SubElement(root, "Import")
    importElem2.set("Project", "$(AndroidTargetsPath)\Android.props")

    itemDefGroup = ET.SubElement(root, "ItemDefinitionGroup")
    antPackageElem = ET.SubElement(itemDefGroup, "AntPackage")
    appNameElem = ET.SubElement(antPackageElem, "AndroidAppLibName")
    appNameElem.text = projectInfo.name

    itemGroup = ET.SubElement(root, "ItemGroup")
    appendTestDataContent(projectInfo, itemGroup, appDir)
    contentElem = ET.SubElement(itemGroup, "Content")
    contentElem.set("Include", "res\\values\strings.xml")
    buildXmlElem = ET.SubElement(itemGroup, "AntBuildXml")
    buildXmlElem.set("Include", "build.xml")
    manifestElem = ET.SubElement(itemGroup, "AndroidManifest")
    manifestElem.set("Include", "AndroidManifest.xml")
    propertiesElem = ET.SubElement(itemGroup, "AntProjectPropertiesFile")
    propertiesElem.set("Include", "project.properties")
    javaCompileElem = ET.SubElement(itemGroup, "JavaCompile")
    javaCompileElem.set("Include", "src\com\{0}\{0}.java".format(projectInfo.testAppName()))

    projReferenceElem = ET.SubElement(itemGroup, "ProjectReference")
    testLibProjFile = os.path.join(projectInfo.name, "Android.Test", "{0}.Android.Test.vcxproj".format(projectInfo.name))
    projReferenceElem.set("Include", "$(SolutionDir)" + str(testLibProjFile))

    importElem3 = ET.SubElement(root, "Import")
    importElem3.set("Project", "$(AndroidTargetsPath)\Android.targets")

    sis.updateFile(androidprojFilename, prettify(root))

    solutionProject = SolutionProject.SolutionProject()
    solutionProject.name = projectInfo.testAppName()
    solutionProject.id = str(projGuid).upper()
    solutionProject.path = os.path.relpath(androidprojFilename, solutionDir)
    solutionProject.projTypeId = SolutionCommon.projectTypeIds['android']
    solutionProject.updateConfigPlatformsForPlatform(platform)

    return solutionProject



