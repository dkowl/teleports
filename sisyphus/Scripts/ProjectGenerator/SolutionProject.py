import SolutionCommon as Common
import SolutionBlock as Block

class SolutionProject:
    def __init__(self, block):
        self.projTypeId = block.arg.strip('\"{}')
        self.name = block.values[0].strip('\"')
        self.path = block.values[1].strip('\"')
        self.id = block.values[2].strip('\"{}')

        # populated from SolutionGlobals
        self.configPlatforms = {} # dict<configPlatformName, tuple<configPlatformName, build(bool)>>
        self.nestedProjects = [] # list<projectId>

        self.dependencies = []
        subBlocks = Block.readBlocks(block.content)
        for subBlock in subBlocks:
            if subBlock.arg == "ProjectDependencies":
                for line in subBlock.content:
                    ids = Common.readAssignmentExpr(line)
                    self.dependencies.append(ids[0].strip('{}'))

    def toBlock(self):
        block = Block.SolutionBlock()
        block.name = 'Project'
        block.arg = '\"{%s}\"' % self.projTypeId
        block.values = ['\"%s\"' % self.name, '\"%s\"' % self.path, '\"{%s}\"' % self.id]

        if len(self.dependencies) > 0:
            subBlock = Block.SolutionBlock()
            subBlock.name = "ProjectSection"
            subBlock.arg = "ProjectDependencies"
            subBlock.values = ['postProject']
            for dependency in self.dependencies:
                subBlock.content.append('{{{0}}} = {{{0}}}'.format(dependency))
            block.content = subBlock.asLines()

        return block

    def write(self):
        return self.toBlock().write()
