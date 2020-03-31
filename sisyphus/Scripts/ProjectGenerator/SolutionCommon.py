slnFormatVersionHeader = "Microsoft Visual Studio Solution File, Format Version "
vsMajorVersionHeader = "# Visual Studio Version "
varVsVersion = "VisualStudioVersion"
varMinVsVersion = "MinimumVisualStudioVersion"

solutionConfigurations = ['Debug', 'Release']
solutionPlatforms = ['ARM', 'ARM64', 'x64']

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
