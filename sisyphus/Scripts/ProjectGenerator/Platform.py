class PlatformData:
    def __init__(self, 
                 name, 
                 configurations, 
                 architectures, 
                 staticLibExt, 
                 dynamicLibExt):
        self.name = name
        self.configurations = configurations
        self.architectures = architectures
        self.staticLibExt = staticLibExt
        self.dynamicLibExt = dynamicLibExt

platforms = {
    "Windows":   
    PlatformData(
        "Windows",
        ["Debug", "Release"],
        ["x64"],
        ".lib",
        ".dll"
    ),
    "Android":
    PlatformData(
        "Android",
        ["Debug", "Release"],
        ["ARM", "ARM64"],
        ".a",
        ".so"
    )
}
