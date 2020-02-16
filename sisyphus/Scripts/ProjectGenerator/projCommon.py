import Platform
from xmlUtils import *

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
