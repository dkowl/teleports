import os
import constants
import SolutionCommon as Common
from SolutionCommon import readAfterPrefix, readVar, writeVar
import SolutionBlock as Block
import SolutionProject as Project
import SolutionGlobalsBlock as GlobalsBlock

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

        self.readHeader(lines[1:5])
        self.projects = {}
        self.globals = None
        blocks = Block.readBlocks(lines[5:])
        for block in blocks:
            if block.name == "Project":
                newProject = Project.SolutionProject(block)
                self.projects[newProject.id] = newProject
            elif block.name == 'Global':
                self.globals = GlobalsBlock.SolutionGlobalsBlock(block)
        self.populateProjectDataFromGlobals()

    def write(self):
        self.populateGlobalsFromProjectData()

        content = '\n'
        content += self.writeHeader()
        for project in self.projects.values():
            content += project.write()
        content += self.globals.write()
        content += '\n'

        return content

    def __init__(self, filepath):
        with open(filepath) as file:
            content = file.read()
            
        self.read(content)

    

solutionFilename = "Sisyphus.sln"
solutionPath = constants.solutionDir + solutionFilename

solution = Solution(solutionPath)
print(solution.write())

