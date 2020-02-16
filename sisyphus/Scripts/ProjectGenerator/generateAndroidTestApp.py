import os
from xmlUtils import *
from constants import *
import ProjectInfo


def generateAndroidTestApp(projectInfo):
    appDir = os.path.join(projectInfo.projDir, "Android.Test", "App");



