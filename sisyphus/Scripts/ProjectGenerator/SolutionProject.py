import SolutionCommon as Common
import SolutionBlock as Block
import Platform

class SolutionProject:
    def __init__(self, block = None):
        self.projTypeId = ''
        self.name = ''
        self.path = ''
        self.id = ''
        self.dependencies = []

        # populated from SolutionGlobals
        self.configPlatforms = {} # dict<configPlatformName, tuple<configPlatformName, build(bool)>>
        self.nestedProjects = [] # list<projectId>

        if block != None:
            self.initFromBlock(block)

    def initFromBlock(self, block):
        self.projTypeId = block.arg.strip('\"{}')
        self.name = block.values[0].strip('\"')
        self.path = block.values[1].strip('\"')
        self.id = block.values[2].strip('\"{}')

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

    def updateConfigPlatformsForPlatform(self, platform):
        for slnConfig in Common.solutionConfigurations:
            buildPlatforms = platform.architectures
            defaultPlatform = buildPlatforms[0]
            for slnPlatform in Common.solutionPlatforms:
                configPlatformString = '%s|%s' % (slnConfig, slnPlatform)
                defaultConfigPlatformString = '%s|%s' % (slnConfig, defaultPlatform)
                if slnPlatform in buildPlatforms:
                    self.configPlatforms[configPlatformString] = (configPlatformString, True)
                else:
                    self.configPlatforms[configPlatformString] = (defaultConfigPlatformString, False)
