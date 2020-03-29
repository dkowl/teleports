import os
import constants

slnFormatVersionHeader = "Microsoft Visual Studio Solution File, Format Version "
vsMajorVersionHeader = "# Visual Studio Version "
varVsVersion = "VisualStudioVersion"
varMinVsVersion = "MinimumVisualStudioVersion"

projectTypeIds = {
    "cpp":"8BC9CEB8-8B4A-11D0-8D11-00A0C91BC942",
    "python":"888888A0-9F3D-457C-B088-3A5042F75D52",
    "folder":"2150E333-8FDC-42A3-9474-1A3956D46DE8",
    "android":"39E2626F-3545-4960-A6E8-258AD8476CE5"
}

projectTypes = {}
for projType, id in projectTypeIds.items():
    projectTypes[id] = projType

def readAfterPrefix(line, prefix):
    assert line.startswith(prefix), "%s does not start with %s" % (line, prefix)
    return line[len(prefix):]

def readAssignmentExpr(line):
    assert ' = ' in line, "Invalid assignment expression: %s" % line
    return line.split(' = ', 1)

def readVar(varName, line):
    expr = readAssignmentExpr(line)
    assert expr[0] == varName
    return expr[1]

def writeVar(name, value):
    return '%s = %s\n' % (name, value)

class SolutionBlock:
    def parseFirstLine(self, line):
        pos = 0
        self.name = ''
        while pos < len(line) and line[pos] not in ['\n', '(']:
            self.name += line[pos]
            pos = pos + 1
        assert self.name != '', "A block must have a name"

        self.arg = None
        self.values = None
        if pos >= len(line):
            return

        if line[pos] == '(':
            argStart = pos
            argEnd = line.find(')')
            self.arg = line[argStart + 1:argEnd]

        findResult = line[argEnd:].find(' = ')
        if findResult != -1:
            self.values = line[argEnd + findResult + len(' = '):].split(', ')

    def writeFirstLine(self):
        assert len(self.name) > 0, "A block must have a name"
        result = ''
        result += self.name
        if self.arg:
            result += '(%s)' % self.arg
        if self.values:
            result += ' = %s' % self.values[0]
            for v in self.values[1:]:
                result += ', %s' % v
        return result

    def endToken(self):
        return 'End%s' % self.name

    def readContent(self, lines):
        endFound = False
        self.content = []
        for line in lines:
            if line == self.endToken():
                endFound = True
                break
            assert line[0] == '\t', "A block content must be indented with \\t"
            self.content.append(line[1:])
        assert endFound, 'A block must have an end (%s)' % self.endToken()

    def __init__(self):
        self.name = ''
        self.arg = None
        self.values = None
        self.content = []  

    def readFromLines(self, lines):
        self.parseFirstLine(lines[0])
        self.readContent(lines[1:])

    def asLines(self):
        lines = [self.writeFirstLine()]
        for line in self.content:
            lines.append('\t' + line)
        lines.append(self.endToken())
        return lines

    def write(self):
        result = ''
        for line in self.asLines():
            result += line + '\n'
        return result

    def numLines(self):
        return len(self.content) + 2


def readBlocks(lines):
    i = 0
    blocks = []
    while i < len(lines):
        if lines[i] == '':
            i = i + 1
            continue
        block = SolutionBlock()
        block.readFromLines(lines[i:])
        blocks.append(block)
        i = i + block.numLines()
    return blocks

class SolutionProject:
    def __init__(self, block):
        self.projType = projectTypes[block.arg.strip('\"{}')]
        self.name = block.values[0].strip('\"')
        self.path = block.values[1].strip('\"')
        self.id = block.values[2].strip('\"{}')

        self.dependencies = []
        subBlocks = readBlocks(block.content)
        for subBlock in subBlocks:
            if subBlock.arg == "ProjectDependencies":
                for line in subBlock.content:
                    ids = readAssignmentExpr(line)
                    self.dependencies.append(ids[0].strip('{}'))

    def toBlock(self):
        block = SolutionBlock()
        block.name = 'Project'
        block.arg = '\"{%s}\"' % projectTypeIds[self.projType]
        block.values = ['\"%s\"' % self.name, '\"%s\"' % self.path, '\"{%s}\"' % self.id]

        if len(self.dependencies) > 0:
            subBlock = SolutionBlock()
            subBlock.name = "ProjectSection"
            subBlock.arg = "ProjectDependencies"
            subBlock.values = ['postProject']
            for dependency in self.dependencies:
                subBlock.content.append('{{{0}}} = {{{0}}}'.format(dependency))
            block.content = subBlock.asLines()

        return block

    def write(self):
        return self.toBlock().write()

class SolutionGlobalsBlock:
    def __init__(self, globalBlock):
        self.solutionConfigPlatforms = []
        self.projectConfigPlatforms = {}
        self.nestedProjects = {}
        self.solutionId = ''
        blocks = readBlocks(globalBlock.content)
        
        for block in blocks:
            if block.arg == 'SolutionConfigurationPlatforms':
                for line in block.content:
                    self.solutionConfigPlatforms.append(readAssignmentExpr(line)[0])
            elif block.arg == 'ProjectConfigurationPlatforms':
                for line in block.content:
                    splitParts = line.split('.', 1)
                    projectId = splitParts[0].strip('{}')
                    rest = splitParts[1]

                    left, right = readAssignmentExpr(rest)
                    splitParts = left.split('.', 1)
                    leftConfigPlatform = splitParts[0]
                    leftMode = splitParts[1]

                    def fromDictOrDefault(dict, key, default):
                        if key in dict:
                            return dict[key]
                        else:
                            return default

                    projectConfigPlatform = fromDictOrDefault(self.projectConfigPlatforms, projectId, {})
                    entry = fromDictOrDefault(projectConfigPlatform, leftConfigPlatform, ('', False))
                    if leftMode == 'ActiveCfg':
                        entry = (right, entry[1])
                    elif leftMode == 'Build.0':
                        entry = (entry[0], True)
                    projectConfigPlatform[leftConfigPlatform] = entry
                    self.projectConfigPlatforms[projectId] = projectConfigPlatform
            elif block.arg == 'NestedProjects':
                for line in block.content:
                    child, parent = readAssignmentExpr(line)
                    child = child.strip('{}')
                    parent = parent.strip('{}')
                    if not parent in self.nestedProjects:
                        self.nestedProjects[parent] = []
                    self.nestedProjects[parent].append(child)
            elif block.arg == 'ExtensibilityGlobals':
                for line in block.content:
                    var, value = readAssignmentExpr(line)
                    if var == 'SolutionGuid':
                        self.solutionId = value.strip('{}')
                assert len(self.solutionId) > 0, 'A solution must have an id'


    def write(self):
        slnConfigPlatformBlock = SolutionBlock()
        slnConfigPlatformBlock.name = 'GlobalSection'
        slnConfigPlatformBlock.arg = 'SolutionConfigurationPlatforms'
        slnConfigPlatformBlock.values = ['preSolution']
        for slnConfigPlatform in self.solutionConfigPlatforms:
            slnConfigPlatformBlock.content.append('{{{0}}} = {{{0}}}'.format(slnConfigPlatform))

        projConfigPlatformBlock = SolutionBlock()
        projConfigPlatformBlock.name = 'GlobalSection'
        projConfigPlatformBlock.arg = 'ProjectConfigurationPlatforms'
        projConfigPlatformBlock.values = ['postSolution']
        for projId, projConfigPlatform in self.projectConfigPlatforms.items():
            for configPlatform, entry in projConfigPlatform.items():
                projConfigPlatformBlock.content.append('{%s}.%s.ActiveCfg = %s' % (projId, configPlatform, entry[0]))
                if entry[1] == True:
                    projConfigPlatformBlock.content.append('{%s}.%s.Build.0 = %s' % (projId, configPlatform, entry[0]))

        propertiesBlock = SolutionBlock()
        propertiesBlock.name = 'GlobalSection'
        propertiesBlock.arg = 'SolutionProperties'
        propertiesBlock.values = ['preSolution']
        propertiesBlock.content.append('HideSolutionNode = FALSE')

        nestedProjectsBlock = SolutionBlock()
        nestedProjectsBlock.name = 'GlobalSection'
        nestedProjectsBlock.arg = 'NestedProjects'
        nestedProjectsBlock.values = ['preSolution']
        for parent, children in self.nestedProjects.items():
            for child in children:
                nestedProjectsBlock.content.append('{%s} = {%s}' % (child, parent))

        extensibilityGlobalsBlock = SolutionBlock()
        extensibilityGlobalsBlock.name = 'GlobalSection'
        extensibilityGlobalsBlock.arg = 'ExtensibilityGlobals'
        extensibilityGlobalsBlock.values = ['postSolution']
        extensibilityGlobalsBlock.content.append('SolutionGuid = {%s}' % self.solutionId)

        globalBlock = SolutionBlock()
        globalBlock.name = 'Global'
        contentBlocks = [slnConfigPlatformBlock, projConfigPlatformBlock, propertiesBlock, nestedProjectsBlock, extensibilityGlobalsBlock]
        for contentBlock in contentBlocks:
            globalBlock.content += contentBlock.asLines()

        return globalBlock.write()               

class Solution:
    def readHeader(self, lines):
        self.slnFormatVersion = readAfterPrefix(lines[0], slnFormatVersionHeader)
        self.vsMajorVersion = readAfterPrefix(lines[1], vsMajorVersionHeader)
        self.vsVersion = readVar(varVsVersion, lines[2])
        self.minVsVersion = readVar(varMinVsVersion, lines[3])

    def writeHeader(self):
        result = ''
        result += '%s%s\n' % (slnFormatVersionHeader, self.slnFormatVersion)
        result += '%s%s\n' % (vsMajorVersionHeader, self.vsMajorVersion)
        result += writeVar(varVsVersion, self.vsVersion)
        result += writeVar(varMinVsVersion, self.minVsVersion)
        return result
        
    def read(self, content):
        lines = content.splitlines()

        self.readHeader(lines[1:5])
        self.projects = []
        self.globals = None
        blocks = readBlocks(lines[5:])
        for block in blocks:
            if block.name == "Project":
                self.projects.append(SolutionProject(block))
            elif block.name == 'Global':
                self.globals = SolutionGlobalsBlock(block)

    def write(self):
        content = '\n'
        content += self.writeHeader()
        for project in self.projects:
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

