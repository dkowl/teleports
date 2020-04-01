from SolutionCommon import *
from SolutionBlock import *

class ConfigPlatformEntry:
    def __init__(self):
        self.activeCfg = ''
        self.build = False
        self.deploy = False

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
                    entry = fromDictOrDefault(projectConfigPlatform, leftConfigPlatform, ConfigPlatformEntry())
                    if leftMode == 'ActiveCfg':
                        entry.activeCfg = right
                    elif leftMode == 'Build.0':
                        entry.build = True
                    elif leftMode == 'Deploy.0':
                        entry.deploy = True
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
            slnConfigPlatformBlock.content.append('{0} = {0}'.format(slnConfigPlatform))

        projConfigPlatformBlock = SolutionBlock()
        projConfigPlatformBlock.name = 'GlobalSection'
        projConfigPlatformBlock.arg = 'ProjectConfigurationPlatforms'
        projConfigPlatformBlock.values = ['postSolution']
        for projId, projConfigPlatform in self.projectConfigPlatforms.items():
            for configPlatform, entry in projConfigPlatform.items():
                projConfigPlatformBlock.content.append('{%s}.%s.ActiveCfg = %s' % (projId, configPlatform, entry.activeCfg))
                if entry.build == True:
                    projConfigPlatformBlock.content.append('{%s}.%s.Build.0 = %s' % (projId, configPlatform, entry.activeCfg))
                if entry.deploy == True:
                    projConfigPlatformBlock.content.append('{%s}.%s.Deploy.0 = %s' % (projId, configPlatform, entry.activeCfg))

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
