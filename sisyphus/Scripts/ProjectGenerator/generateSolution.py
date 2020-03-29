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
        blocks = readBlocks(lines[5:])
        for block in blocks:
            if block.name == "Project":
                self.projects.append(SolutionProject(block))        

    def write(self):
        content = '\n'
        content += self.writeHeader()
        for project in self.projects:
            content += project.write()
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

