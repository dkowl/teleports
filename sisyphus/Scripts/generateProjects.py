import xml.etree.ElementTree as ET
from xml.dom import minidom
import os
import uuid
import json

msbuildXmlNamespace = 'http://schemas.microsoft.com/developer/msbuild/2003'
ET.register_namespace('', msbuildXmlNamespace)

def prettify(elem):
    """Return a pretty-printed XML string for the Element.
    """
    rough_string = ET.tostring(elem, 'utf-8')
    reparsed = minidom.parseString(rough_string)
    return reparsed.toprettyxml(indent="  ")

class Platform:
    def __init__(self, name, configurations, architectures):
        self.name = name
        self.configurations = configurations
        self.architectures = architectures

platforms = [
    Platform(
        "Windows",
        ["Debug", "Release"],
        ["x64"]
    ),
    Platform(
        "Android",
        ["Debug", "Release"],
        ["ARM", "ARM64"]
    )
]

solutionDir = "../"

def readProjectGuid(path):
    if os.path.exists(path):
        try:
            with open(path) as file:
                root = ET.parse(file).getroot()
                guidElem = root.find(".//{" + msbuildXmlNamespace + "}ProjectGuid")
                print("guid of " + os.path.basename(path) + " is " + guidElem.text)
                return uuid.UUID(guidElem.text)
        except Exception as e:
            print("Failed to read guid from " + str(path) + ": " + str(e))
    
    return uuid.uuid4()

class ProjectInfo:
    def __init__(self, projName):
        self.name = projName
        with open(os.path.join(self.projDir(), "projectInfo.json")) as jsonFile:
            jsonData = json.load(jsonFile)
            self.outputType = jsonData["outputType"]
            self.dependencies = jsonData["dependencies"]
            self.precompiledHeaders = jsonData["precompiledHeaders"]
            self.test = jsonData["test"]

    def projDir(self):
        return os.path.join(solutionDir, self.name)

def projectConfigurations(platform):
    root = ET.Element("ItemGroup")
    root.set("Label", "ProjectConfigurations")
    for config in platform.configurations:
        for arch in platform.architectures:
            projConfigElem = ET.SubElement(root, "ProjectConfiguration")
            projConfigElem.set("Include", config + "|" + arch)
            configElem = ET.SubElement(projConfigElem, "Configuration")
            configElem.text = config
            archElem = ET.SubElement(projConfigElem, "Platform")
            archElem.text = arch

    return root

def globals(platform, projectInfo, projGuid):
    root = ET.Element("PropertyGroup")
    root.set("Label", "Globals")

    guidElem = ET.SubElement(root, "ProjectGuid")
    guidElem.text = str(projGuid)

    rootNsElem = ET.SubElement(root, "RootNamespace")
    rootNsElem.text = projectInfo.name

    if platform.name == "Windows":
        wtpverElem = ET.SubElement(root, "WindowsTargetPlatformVersion")
        wtpverElem.text = "10.0"
    elif platform.name == "Android":
        keywordElem = ET.SubElement(root, "Keyword")
        keywordElem.text = "Android"
        appTypeElem = ET.SubElement(root, "ApplicationType")
        appTypeElem.text = "Android"
        appTypeRevElem = ET.SubElement(root, "ApplicationTypeRevision")
        appTypeRevElem.text = "3.0"

    return root

def configurations(platform, projectInfo):
    roots = []
    for config in platform.configurations:
        for arch in platform.architectures:
            root = ET.Element("PropertyGroup")
            root.set("Condition", "'$(Configuration)|$(Platform)'=='" + config + "|" + arch + "'")
            root.set("Label", "Configuration")
            configTypeElem = ET.SubElement(root, "ConfigurationType")
            configTypeElem.text = projectInfo.outputType
            useDebugElem = ET.SubElement(root, "UseDebugLibraries")
            useDebugElem.text = "true" if config == "Debug" else "false"
            toolsetElem = ET.SubElement(root, "PlatformToolset")
            toolsetElem.text = "v142" if platform.name == "Windows" else "Clang_5_0"
            if platform.name == "Windows":
                if config == "Release":
                    wpoElem = ET.SubElement(root, "WholeProgramOptimization")
                    wpoElem.text = "true"
                charsetElem = ET.SubElement(root, "CharacterSet")
                charsetElem.text = "MultiByte"
            roots.append(root)
    return roots

def propsImportGroup(projectInfo):
    root = ET.Element("ImportGroup")
    root.set("Label", "PropertySheets")

    userPropsElem = ET.SubElement(root, "Import")
    userPropsElem.set("Project", R"$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props")
    userPropsElem.set("Condition", R"exists('$(UserRootDir)\Microsoft.Cpp.$(Platform).user.props')")
    userPropsElem.set("Label", "LocalAppDataPlatform")

    dependencies = ["General"]
    if projectInfo.precompiledHeaders:
        dependencies.append("PrecompiledHeaders")

    for dep in projectInfo.dependencies:
        dependencies.append(dep)

    for dep in dependencies:
        importElem = ET.SubElement(root, "Import")
        importElem.set("Project", "$(SolutionDir)props\\" + dep + ".props")

    return root

def getCppPaths(projName, platform):
    sourceDirs = ["src", "include"]
    result = []
    excludedPlatforms = platforms.copy()
    excludedPlatforms.remove(platform)
    for srcDir in sourceDirs:
        for root, d, files in os.walk(os.path.join(solutionDir, projName, srcDir)):
            for file in files:
                exclude = False
                for excludedPlatform in excludedPlatforms:
                    if file.find("." + excludedPlatform.name + ".") != -1:
                        exclude = True
                        break
                if exclude:
                    continue
                result.append(os.path.relpath(os.path.join(root, file), os.path.join(solutionDir)))

    return result

def getIncludesAndCompiles(platform, projName):
    cppPaths = getCppPaths(projName, platform)
    includeGroup = ET.Element("ItemGroup")
    compileGroup = ET.Element("ItemGroup")
    for path in cppPaths:
        stem, extension = os.path.splitext(os.path.basename(path))
        if extension in (".h", ".hpp"):
            includeElem = ET.SubElement(includeGroup, "ClInclude")
            includeElem.set("Include", "$(SolutionDir)" + str(path))
        elif extension in (".c", ".cpp"):
            compileElem = ET.SubElement(compileGroup, "ClCompile")
            compileElem.set("Include", "$(SolutionDir)" + str(path))
            if (stem == "Pch." + projName or stem == "Pch_" + projName) and platform.name == "Windows":
                pchElem = ET.SubElement(compileElem, "PrecompiledHeader")
                pchElem.text = "Create"
    return (includeGroup, compileGroup)

def generateVcxproj(platform, projectInfo, projGuid):
    root = ET.Element("Project")
    root.set("DefaultTargets", "Build")
    root.set("xmlns", msbuildXmlNamespace)

    root.append(projectConfigurations(platform))
    root.append(globals(platform, projectInfo, projGuid))

    defaultPropsElem = ET.Element("Import")
    defaultPropsElem.set("Project", R"$(VCTargetsPath)\Microsoft.Cpp.Default.props")
    root.append(defaultPropsElem)

    for elem in configurations(platform, projectInfo):
        root.append(elem)

    cppPropsElem = ET.Element("Import")
    cppPropsElem.set("Project", R"$(VCTargetsPath)\Microsoft.Cpp.props")
    root.append(cppPropsElem)

    extSettingsElem = ET.Element("ImportGroup")
    extSettingsElem.set("Label", "ExtensionSettings")
    root.append(extSettingsElem)

    sharedElem = ET.Element("ImportGroup")
    sharedElem.set("Label", "Shared")
    root.append(sharedElem)
    
    root.append(propsImportGroup(projectInfo))

    userMacrosElem = ET.Element("PropertyGroup")
    userMacrosElem.set("Label", "UserMacros")

    includeGroup, compileGroup = getIncludesAndCompiles(platform, projectInfo.name)
    root.append(includeGroup)
    root.append(compileGroup)

    targetsImportElem = ET.Element("Import")
    targetsImportElem.set("Project", R"$(VCTargetsPath)\Microsoft.Cpp.targets")
    root.append(targetsImportElem)

    extTargetsElem = ET.Element("ImportGroup")
    extTargetsElem.set("Label", "ExtensionTargets")
    root.append(extTargetsElem)

    return prettify(root)

def generateProject(projectInfo):
    for platform in platforms:
        platformDir = os.path.join(projectInfo.projDir(), platform.name)
        if not os.path.exists(platformDir):
            os.makedirs(platformDir)
        projFilename = projectInfo.name + "." + platform.name + ".vcxproj"
        projPath = os.path.join(platformDir, projFilename)
        projGuid = readProjectGuid(projPath)
        with open(projPath, 'w') as projFile:
            projFile.write(generateVcxproj(platform, projectInfo, projGuid))
            print(projFilename + "generated!")
info = ProjectInfo("AssetManagement")
generateProject(info)

