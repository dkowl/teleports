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
