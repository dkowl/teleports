import os
import json
import traceback
from constants import solutionDir

class ProjectInfo:
    def __init__(self, projName):
        self.name = projName
        try:
            path = os.path.join(self.projDir(), "{0}.projectInfo.json".format(projName))
            with open(path) as jsonFile:
                jsonData = json.load(jsonFile)
                self.outputType = jsonData["outputType"]
                self.dependencies = jsonData["dependencies"]
                self.precompiledHeaders = jsonData["precompiledHeaders"]
                self.test = jsonData["test"]
        except:
            print("Failed to read project info from {0}: {1}".format(path, traceback.format_exc()))

    def projDir(self):
        return os.path.join(solutionDir, self.name)

    def testAppName(self):
        return self.name + "AndroidTestApp"
