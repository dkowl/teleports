import xml.etree.ElementTree as ET
from xml.dom import minidom

msbuildXmlNamespace = 'http://schemas.microsoft.com/developer/msbuild/2003'
ET.register_namespace('', msbuildXmlNamespace)

def prettify(elem):
    """Return a pretty-printed XML string for the Element.
    """
    rough_string = ET.tostring(elem, 'utf-8')
    reparsed = minidom.parseString(rough_string)
    result = str(reparsed.toprettyxml(indent="  ", encoding='utf-8'), encoding='utf-8')
    result = result.replace('/>', ' />')
    result = result.rstrip()
    return result
