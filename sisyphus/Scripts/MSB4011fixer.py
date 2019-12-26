import xml.etree.ElementTree as ET
import os

namespace = 'http://schemas.microsoft.com/developer/msbuild/2003'
ET.register_namespace('', namespace)

propsPath = '../props/'

def addImportedTag(propertyGroup, propsName):
	elemName = propsName + 'Imported'
	elem = propertyGroup.find('{' + namespace + '}' + elemName)
	if elem == None:
		elem = ET.SubElement(propertyGroup, elemName)
	elem.text = 'true'

def processFile(filename):
	path = propsPath + filename + '.props'
	tree = ET.parse(path)
	root = tree.getroot()

	importGroup = root[0]
	importedProps = []
	for elem in importGroup.iter('{' + namespace + '}' + 'Import'):
		project = elem.get('Project')
		importedFilename = project.partition('.')[0]
		importedProps.append(importedFilename)
		elem.set('Condition', '\'$(' + importedFilename + 'Imported)\' ==\'\'')

	propertyGroup = root[2]
	addImportedTag(propertyGroup, filename)
	for name in importedProps:
		addImportedTag(propertyGroup, name)

	tree.write(open(path, 'wb'), encoding='UTF-8')

for r, d, f in os.walk(propsPath):
	for file in f:
		processFile(file.partition('.')[0])