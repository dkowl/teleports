# Project system

## Creating a new project

A project in this system represents a C++ project that conforms to some rules. The project can either be a static or dynamic library or an application like a game.

You create projects simply by creating a new directory, which becomes your project's main directory. The name of this directory becomes your project's name.

### projectInfo.json

In there, you need to create a file named *projectName*.projectInfo.json which describes the project.

Here are the different properties that you can define:

##### outputType
Possible values:  
- staticLibrary
- dynamicLibrary
- application

*Required*

##### dependencies  
A list of other projects or external dependencies that your project depends on.  
Default: *None*

##### platforms
A list of platforms to generate targets for.  
Possible values:  
- Windows
- Android

Default: all platforms

##### test
Boolean property, if true test projects will be generated, a console app on Windows and an App on Android. The testing framework used is https://github.com/catchorg/Catch2.  
Default: *false*

##### displayName
For applications only - the name of the app to be shown to the users. Used for example for window titles or app names.  
Default: *project name*

##### precompiledHeaders
Boolean property, if true empty pch files will be automatically generated and set up to be treated as precompiled header files.  
Default: *false*

#### example

```
{
	"outputType" : "Application",
	"dependencies" : [
		"AssetManagement",
		"Filesystem",
		"Logger",
		"Utils",
		"CLI11",
		"json"
	],
	"platforms": [
		"Windows"
	],
	"test": false,
	"displayName" : "Sisyphus Editor",
	"precompiledHeaders" : true
}
```

### Directory structure

Then you put your source files in specially named folders.

- include/*projectName*
  - contains public headers
- src
  - contains source files
- test
  - contains test files

## Generating projects

Just run *generateSolution.py* or use the SisyphusVsExtension - it adds a yellow S button in your Solution Explorer. The button just runs the same script.