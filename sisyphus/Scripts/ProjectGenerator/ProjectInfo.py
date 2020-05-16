import os
import json
import traceback
import Platform
import sisyphusUtils as sis
import constants
import projCommon
from SolutionProject import SolutionProject

# this data is passed to Solution to manage project build dependencies
class PlatformSolutionProjects():
    def __init__(self):
        self.mainProj = None
        self.mainAppProj = None
        self.testProj = None
        self.testAppProj = None

    def getAllProjects(self):
        projects = [self.mainProj, self.mainAppProj, self.testProj, self.testAppProj]
        result = []
        for project in projects:
            if project != None:
                result.append(project)
        return result

    def updateProjectDependencies(self):
        assert self.mainProj != None
        if self.mainAppProj != None:
            self.mainAppProj.addDependency(self.mainProj)
        if self.testProj != None:
            self.testProj.addDependency(self.mainProj)
        if self.testAppProj != None:
            assert self.testProj != None
            self.testAppProj.addDependency(self.testProj)

    def addInterProjectDependency(self, other):
        assert self.mainProj != None and other.mainProj != None
        self.mainProj.addDependency(other.mainProj)

def readOrDefault(jsonData, key, default):
    if key in jsonData:
        return jsonData[key]
    else:
        return default

class ProjectInfo:
    allProjects = {}

    def __init__(self, projName, path):
        self.name = projName
        self.path = path
        try:            
            with open(self.path) as jsonFile:
                jsonData = json.load(jsonFile)
                self.outputType = readOrDefault(jsonData, 'outputType', 'staticLibrary')
                self.dependencies = readOrDefault(jsonData, 'dependencies', [])
                self.precompiledHeaders = readOrDefault(jsonData, 'precompiledHeaders', False)
                self.test = readOrDefault(jsonData, 'test', False)
                self.platformNames = readOrDefault(jsonData, 'platforms', ['Windows', 'Android'])
        except:
            print("Failed to read project info from {0}: {1}".format(self.path, traceback.format_exc()))

        # to be filled by generateProject()
        self.solutionProjects = {} # dict<Platform name, PlatformSolutionProjects>

        self.indirectDependencies = []
        self.projectDependencies = [] # all dependencies excluding 3rd party ones

        type(self).allProjects[projName] = self

    def dir(self):
        return os.path.dirname(self.path)

    def projDir(self, platform):
        return os.path.join(self.dir(), platform.name)

    def testProjDir(self, platform):
        return os.path.join(self.dir(), f'{platform.name}.Test')

    def testDataDir(self):
        return os.path.join(self.dir(), 'test_data')

    def assetDir(self):
        return os.path.join(self.dir(), 'assets')

    def includeDir(self):
        return os.path.join(self.dir(), 'include')

    def sourceDir(self):
        return os.path.join(self.dir(), 'src')

    def testSourceDir(self):
        return os.path.join(self.dir(), 'test')

    def gameAppName(self):
        return self.name + 'AndroidApp'

    def testAppName(self):
        return self.name + "AndroidTestApp"

    def platforms(self):
        return [Platform.platforms[name] for name in self.platformNames]

    def mainProjOutputType(self, platform):
        if self.outputType == 'Application' and platform.name == 'Android':
            return 'DynamicLibrary'
        else:
            return self.outputType

    def testProjOutputType(self, platform):
        if platform.name == 'Android':
            return 'DynamicLibrary'
        else:
            return 'Application'

    def getAllSolutionProjects(self):
        result = []
        for projects in self.solutionProjects.values():
            result += projects.getAllProjects()
        return result

    def updateProjectDependencies(self):
        for projects in self.solutionProjects.values():
            projects.updateProjectDependencies()

    def addInterProjectDependency(self, other):
        for platform in self.solutionProjects.keys():
            if platform in other.solutionProjects:
                self.solutionProjects[platform].addInterProjectDependency(other.solutionProjects[platform])

    def computeDependencies(self, projectDict):
        result = set()
        for dep in self.dependencies:
            result.add(dep)
            if dep in projectDict:
                result.update(projectDict[dep].computeDependencies(projectDict))
        return result

    def updateDependencyLists(self, projectDict):
        self.indirectDependencies = []
        self.projectDependencies = []
        allDependencies = self.computeDependencies(projectDict)
        for dep in allDependencies:
            if dep not in self.dependencies:
                self.indirectDependencies.append(dep)
            if dep in projectDict:
                self.projectDependencies.append(dep)
        self.indirectDependencies.sort()
        self.projectDependencies.sort()

    def allDependencies(self):
        return [*self.dependencies, *self.indirectDependencies]

    def generateLoggerFile(self):
        assert('Logger' in self.allDependencies())
        filenames = [
            ('other/Logger.cpp.template', f'src/{self.name}.Logger.cpp')
            ]
        replaceDict = {'PROJNAME': self.name}
        return sis.generateFiles(type(self).allProjects['Logger'].dir(), self.dir(), filenames, replaceDict)

    def generateCatchFiles(self):
        assert(self.test)
        srcDir = os.path.join(constants.pythonSourceDir, "items")
        dstDir = self.dir()

        filenames = [
            ("catch.main.cpp", os.path.join('test', "catch.main.cpp")),
            ("catch.listener.cpp", os.path.join('test', "catch.listener.cpp")),
            ("catch.globals.h", os.path.join('test', "catch.globals.h"))
            ]

        replaceDict = {
            "APPNAME": self.testAppName(),
            "PROJNAME": self.name
            }

        return sis.generateFiles(srcDir, dstDir, filenames, replaceDict)

    def generateNeededFiles(self):
        gitignorePaths = []
        if self.test:
            gitignorePaths += self.generateCatchFiles()
        if 'Logger' in self.allDependencies():
            gitignorePaths += self.generateLoggerFile()

        if gitignorePaths:
            projCommon.generateGitignore(self.dir(), gitignorePaths)

        
             

    

