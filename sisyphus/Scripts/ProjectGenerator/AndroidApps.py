import os
import shutil
from xmlUtils import *
import constants
import ProjectInfo
import sisyphusUtils as sis
from projCommon import *
import SolutionCommon
import SolutionProject
import Platform

class AndroidApp:    
    def __init__(self, projectInfo):
        self.projectInfo = projectInfo

    def appName(self):
        return self.projectInfo.gameAppName()

    def javaMainName(self):
        return None

    def libProjDirName(self):
        return 'Android'

    def libDir(self):
        return os.path.join(self.projectInfo.dir(), self.libProjDirName())

    def appDir(self):
        return os.path.join(self.libDir(), 'App')

    def fileSrcDir(self):
        return os.path.join(constants.pythonSourceDir, 'items', 'AndroidApp')

    def filesToCopy(self):
        result = [
            ("AndroidManifest.xml", "AndroidManifest.xml"),
            ("build.xml", "build.xml"),
            ("project.properties", "project.properties"),
            ("strings.xml", os.path.join("res", "values", "strings.xml"))
            ]  

        if self.javaMainName():
            result.append(
                (self.javaMainName(), os.path.join("src", "com", self.appName(), self.projectInfo.name + self.javaMainName()))
                )
        return result

    def replaceDict(self):
        return {
            'APPNAME': self.appName(),
            'PROJNAME': self.projectInfo.name
            }

    def copyNeededFiles(self):
        sis.generateFiles(self.fileSrcDir(), self.appDir(), self.filesToCopy(), self.replaceDict())
        gitignorePaths = [filename for ignored, filename in self.filesToCopy()]

        srcDir = os.path.join(self.fileSrcDir(), 'res')
        dstDir = os.path.join(self.appDir(), 'res')
        gitignorePaths += copyDirContent(srcDir, self.appDir(), self.fileSrcDir())

        return gitignorePaths

    def generate(self):
        copiedFiles = self.copyNeededFiles()
        generateGitignore(self.appDir(), copiedFiles)

        androidprojFilename = os.path.join(self.appDir(), f'{self.appName()}.androidproj')
        projGuid = readProjectGuid(androidprojFilename)

        root = ET.Element("Project")
        root.set("DefaultTargets", "Build")
        root.set("xmlns", msbuildXmlNamespace)
        root.append(projectConfigurations(Platform.platforms['Android']))

        # globals
        globalsElem = ET.Element("PropertyGroup")
        globalsElem.set("Label", "Globals")

        guidElem = ET.SubElement(globalsElem, "ProjectGuid")
        guidElem.text = sis.formatGuid(projGuid)

        rootNsElem = ET.SubElement(globalsElem, "RootNamespace")
        rootNsElem.text = self.appName()

        packagingProjWithoutNativeCompElem = ET.SubElement(globalsElem, "_PackagingProjectWithoutNativeComponent")
        packagingProjWithoutNativeCompElem.text = "true"

        launchActivityElem = ET.SubElement(globalsElem, "LaunchActivity")
        launchActivityElem.set("Condition", "'$(LaunchActivity)' == ''")
        launchActivityElem.text = "com.{0}.{0}".format(self.appName())

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
        appNameElem.text = self.projectInfo.name

        itemGroup = ET.SubElement(root, "ItemGroup")
        itemPaths = getFilepathsRecursive(os.path.join(self.appDir(), 'assets'), self.appDir())
        for itemPath in itemPaths:        
            contentElem = ET.SubElement(itemGroup, "Content")
            contentElem.set("Include", str(itemPath))
        itemPaths = getFilepathsRecursive(os.path.join(self.appDir(), 'res'), self.appDir())
        for itemPath in itemPaths:        
            contentElem = ET.SubElement(itemGroup, "Content")
            contentElem.set("Include", str(itemPath))
        buildXmlElem = ET.SubElement(itemGroup, "AntBuildXml")
        buildXmlElem.set("Include", "build.xml")
        manifestElem = ET.SubElement(itemGroup, "AndroidManifest")
        manifestElem.set("Include", "AndroidManifest.xml")
        propertiesElem = ET.SubElement(itemGroup, "AntProjectPropertiesFile")
        propertiesElem.set("Include", "project.properties")
        if self.javaMainName():
            javaCompileElem = ET.SubElement(itemGroup, "JavaCompile")
            javaCompileElem.set("Include", "src\com\{0}\{0}.java".format(self.appName()))

        projReferenceElem = ET.SubElement(itemGroup, "ProjectReference")
        libProjFile = os.path.relpath(os.path.join(self.libDir(), f'{self.projectInfo.name}.{self.libProjDirName()}.vcxproj'), constants.solutionDir)
        projReferenceElem.set("Include", "$(SolutionDir)" + str(libProjFile))

        importElem3 = ET.SubElement(root, "Import")
        importElem3.set("Project", "$(AndroidTargetsPath)\Android.targets")

        sis.updateFile(androidprojFilename, prettify(root))

        solutionProject = SolutionProject.SolutionProject()
        solutionProject.name = self.appName()
        solutionProject.id = str(projGuid).upper()
        solutionProject.path = os.path.relpath(androidprojFilename, constants.solutionDir)
        solutionProject.projTypeId = SolutionCommon.projectTypeIds['android']
        solutionProject.updateConfigPlatformsForPlatform(Platform.platforms['Android'])

        return solutionProject

class AndroidGameApp(AndroidApp):

    def copyNeededFiles(self):
        gitignorePaths = super().copyNeededFiles()

        gitignorePaths += copyDirContent(
            self.projectInfo.assetDir(),
            self.appDir(),
            self.projectInfo.dir()
            )
        return gitignorePaths


class AndroidTestApp(AndroidApp):

    def appName(self):
        return self.projectInfo.testAppName()

    def javaMainName(self):
        return 'AndroidTestApp.java'

    def libProjDirName(self):
        return 'Android.Test'

    def fileSrcDir(self):
        return os.path.join(constants.pythonSourceDir, 'items', 'AndroidTestApp')

    def copyNeededFiles(self):
        gitignorePaths = super().copyNeededFiles()

        gitignorePaths += copyDirContent(
            self.projectInfo.testDataDir(),
            os.path.join(self.appDir(), 'assets'),
            self.projectInfo.dir(),
            self.appDir()
            )
        return gitignorePaths



