import os
import sys
import logging
if len(sys.argv) > 1:
    os.chdir(sys.argv[1])
    sys.path.append(sys.argv[1])
import uuid
import constants
import SolutionCommon as Common
from SolutionCommon import readAfterPrefix, readVar, writeVar
import SolutionBlock as Block
import SolutionProject as Project
import SolutionGlobalsBlock as GlobalsBlock
from generateProjects import generateProject
from ProjectInfo import ProjectInfo
import sisyphusUtils as sis

class Solution:
    def readHeader(self, lines):
        self.slnFormatVersion = readAfterPrefix(lines[0], Common.slnFormatVersionHeader)
        self.vsMajorVersion = readAfterPrefix(lines[1], Common.vsMajorVersionHeader)
        self.vsVersion = readVar(Common.varVsVersion, lines[2])
        self.minVsVersion = readVar(Common.varMinVsVersion, lines[3])

    def writeHeader(self):
        result = ''
        result += '%s%s\n' % (Common.slnFormatVersionHeader, self.slnFormatVersion)
        result += '%s%s\n' % (Common.vsMajorVersionHeader, self.vsMajorVersion)
        result += writeVar(Common.varVsVersion, self.vsVersion)
        result += writeVar(Common.varMinVsVersion, self.minVsVersion)
        return result

    def populateProjectDataFromGlobals(self):
        for projectId, configPlatforms in self.globals.projectConfigPlatforms.items():
            assert projectId in self.projects
            self.projects[projectId].configPlatforms = configPlatforms

        for parent, children in self.globals.nestedProjects.items():
            assert parent in self.projects
            self.projects[parent].nestedProjects = children

    def populateGlobalsFromProjectData(self):
        for project in self.projects.values():
            self.globals.projectConfigPlatforms[project.id] = project.configPlatforms
            self.globals.nestedProjects[project.id] = project.nestedProjects
        
    def read(self, content):
        lines = content.splitlines()

        while lines[0] == '':
            lines = lines[1:]
        self.readHeader(lines[0:4])
        self.projects = {}
        self.globals = None
        blocks = Block.readBlocks(lines[4:])
        for block in blocks:
            if block.name == "Project":
                newProject = Project.SolutionProject(block)
                self.projects[newProject.id] = newProject
            elif block.name == 'Global':
                self.globals = GlobalsBlock.SolutionGlobalsBlock(block)
        self.populateProjectDataFromGlobals()

    def write(self):
        self.populateGlobalsFromProjectData()

        content = self.writeHeader()
        for project in self.projects.values():
            content += project.write()
        content += self.globals.write()

        return content

    def __init__(self, filepath):
        with open(filepath) as file:
            content = file.read()
            
        self.read(content)

    def findProjectByName(self, name):
        for project in self.projects.values():
            if project.name == name:
                return project
        return None

    def insertProjects(self, projects):
        for project in projects:
            self.insertProject(project)

    def insertProject(self, newProject):
            self.projects[newProject.id] = newProject

    def getOrCreateSolutionFolder(self, name, path):
        solutionFolder = self.findProjectByName(name)
        if solutionFolder == None:
            solutionFolder = Project.SolutionProject()
            solutionFolder.id = sis.formatGuid(uuid.uuid4()).strip('{}')
            solutionFolder.name = name
        solutionFolder.projTypeId = Common.projectTypeIds['folder']
        solutionFolder.path = path
        return solutionFolder

def findProjects():
    projects = []
    for root, dirs, files in os.walk(constants.solutionDir):
        for dir in dirs:
            projInfoPath = os.path.join(root, dir, os.path.basename(dir) + '.projectInfo.json')
            if os.path.exists(projInfoPath):
                projects.append(ProjectInfo(dir, projInfoPath))
                logging.info(f"Project found: {dir}")
    if projects.count == 0:
        logging.error("No projects found. Are you missing .projectInfo.json files?")
    return projects

# MAIN STARTS HERE
logging.basicConfig(stream=sys.stdout, level=logging.INFO, format='%(levelname)s:%(message)s')

solutionFilename = "Sisyphus.sln"
solutionPath = constants.solutionDir + solutionFilename

solution = Solution(solutionPath)

findProjects()
projectInfos = ProjectInfo.allProjects
for projectInfo in projectInfos.values():
    projectInfo.updateDependencyLists(projectInfos)
projectsToInsert = []    

for projectInfo in projectInfos.values():
    solutionFolderHierarchy = []
    projRelPath = os.path.relpath(projectInfo.dir(), constants.solutionDir)
    while projRelPath:
        splitPath = os.path.split(projRelPath)
        solutionFolderHierarchy.append(splitPath[1])
        if splitPath[0]:
            projRelPath = splitPath[0]
        else:
            break

    prevParentSolutionFolder = None
    fullPath = ''
    for parentFolder in reversed(solutionFolderHierarchy):
        fullPath = os.path.join(fullPath, parentFolder)
        parentSolutionFolder = solution.getOrCreateSolutionFolder(parentFolder, fullPath)
        if prevParentSolutionFolder and parentSolutionFolder.id not in prevParentSolutionFolder.nestedProjects:
            prevParentSolutionFolder.nestedProjects.append(parentSolutionFolder.id)
        prevParentSolutionFolder = parentSolutionFolder
        projectsToInsert.append(parentSolutionFolder)

    solutionFolder = prevParentSolutionFolder
    solutionFolder.solutionItems = [os.path.relpath(projectInfo.path, constants.solutionDir)]

    generateProject(projectInfo)

    projectInfo.updateProjectDependencies()

    solutionProjects = projectInfo.getAllSolutionProjects()

    solutionFolder.nestedProjects = []
    for slnProject in solutionProjects:
        solutionFolder.nestedProjects.append(slnProject.id)

    projectsToInsert += solutionProjects

for projectInfo in projectInfos.values():
    for dependency in projectInfo.projectDependencies:
        if dependency in projectInfos:
            projectInfo.addInterProjectDependency(projectInfos[dependency])

solution.insertProjects(projectsToInsert)

sis.updateFile(solutionPath, solution.write())

