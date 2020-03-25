import os
import constants

slnFormatVersionHeader = "Microsoft Visual Studio Solution File, Format Version "
vsMajorVersionHeader = "# Visual Studio Version "
varVsVersion = "VisualStudioVersion"
varMinVsVersion = "MinimumVisualStudioVersion"

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
        result = ''
        result += self.name
        if self.arg:
            result += '(%s)' % self.arg
        if self.values:
            result += ' = %s' % self.values[0]
            for v in self.values[1:]:
                result += ', %s' % v
        result += '\n'
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
            self.content.append(line)
        assert endFound, 'A block must have an end (%s)' % self.endToken()

    def __init__(self, lines):
        self.parseFirstLine(lines[0])
        self.readContent(lines[1:])     

    def write(self):
        result = ''
        result += self.writeFirstLine()
        for line in self.content:
            result += line + '\n'
        result += self.endToken() + '\n'
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
        block = SolutionBlock(lines[i:])
        blocks.append(block)
        i = i + block.numLines()
    return blocks

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
        self.blocks = readBlocks(lines[5:])
        

    def write(self):
        content = '\n'
        content += self.writeHeader()
        for block in self.blocks:
            content += block.write()
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

