import os
import json
import traceback
import Platform
from constants import solutionDir
from SolutionProject import SolutionProject

# this data is passed to Solution to manage project build dependencies
class PlatformSolutionProjects():
    def __init__(self):
        self.mainProj = None
        self.testProj = None
        self.testAppProj = None

    def getAllProjects(self):
        projects = [self.mainProj, self.testProj, self.testAppProj]
        result = []
        for project in projects:
            if project != None:
                result.append(project)
        return result

    def updateProjectDependencies(self):
        assert self.mainProj != None
        if self.testProj != None:
            self.testProj.dependencies.add(self.mainProj.id)
        if self.testAppProj != None:
            assert self.testProj != None
            self.testAppProj.dependencies.add(self.testProj.id)

    def addInterProjectDependency(self, other):
        assert self.mainProj != None and other.mainProj != None
        self.mainProj.dependencies.add(other.mainProj.id)

def readOrDefault(jsonData, key, default):
    if key in jsonData:
        return jsonData[key]
    else:
        return default

class ProjectInfo:
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
        self.solutionProjects = {} # dict<Platform, PlatformSolutionProjects>

    def projDir(self):
        return os.path.join(solutionDir, self.name)

    def includeDir(self):
        return os.path.join(self.projDir(), 'include')

    def sourceDir(self):
        return os.path.join(self.projDir(), 'src')

    def testSourceDir(self):
        return os.path.join(self.projDir(), 'test')

    def testAppName(self):
        return self.name + "AndroidTestApp"

    def platforms(self):
        return [Platform.platforms[name] for name in self.platformNames]

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
            assert platform in other.solutionProjects
            self.solutionProjects[platform].addInterProjectDependency(other.solutionProjects[platform])
