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
