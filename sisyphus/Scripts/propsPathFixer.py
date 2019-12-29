# converts relative paths to abolute ones
import os
import fileinput

startPath = '../'

for root, d, f in os.walk(startPath):
	for filename in f:
		stem, extension = os.path.splitext(filename)
		if extension == '.vcxproj':
			filepath = os.path.join(root, filename)
			print('editing ' + filepath)
			with fileinput.FileInput(filepath, inplace=True) as file:
				for line in file:
					print(line.replace('..\props', '$(SolutionDir)props'), end='')

