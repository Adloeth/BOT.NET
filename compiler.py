# I got really annoyed about retyping code everywhere. C# doesn't have support for Macros, which is a shame. Codes snippets are not macros. 
# Code generators needs Roslyn and are too overpowered when all I want is a fucking #macro preprocessor that replaces a string by another right 
# before compiling.
# It is so fucking simple to implement that I am going to use this python script to run/build my project instead of using `dotnet`.

import os
import shutil
import sys
import re

if not os.path.exists(".tmp"):
    os.mkdir(".tmp")

if not os.path.exists(".tmp/compiler"):
    os.mkdir(".tmp/compiler")

if os.path.exists(".tmp/compiler/src"):
    shutil.rmtree(".tmp/compiler/src")

shutil.copytree("src/", ".tmp/compiler/src")

def getMacro(line, path, l):
    match = re.match("^\s*((\/\*)|(\/\/)){0,1}\s*#macro\s+", line)
    if not match:
        return 0, 0, 0, 0

    macro = line[match.end():]

    macroTypeMatch = re.match("^((decl)|(use)|(end)|(import)|(execute))\s*", macro)

    if not macroTypeMatch:
        print("Error: Invalid macro type, need to be either 'decl', 'end', 'import' or 'use' at " + l + " in " + path)
        return 1, 0, 0, 0
    
    if macro.startswith("decl"):
        type = 0
    elif macro.startswith("end"):
        return 2, 1, 0, 0
    elif macro.startswith("import"):
        # path of the import is passed as the name
        return 2, 2, macro[macroTypeMatch.end():-1], 0
    elif macro.startswith("execute"):
        # path of the execute is passed as the name
        return 2, 3, macro[macroTypeMatch.end():-1], 0
    else:
        type = 4
    
    macro = macro[macroTypeMatch.end():]
    macroNameMatch = re.match("^\w+", macro)

    if not macroNameMatch:
        print("Error: Incomplete macro at line " + l + " in " + path)
        return 1, 0, 0, 0
            
    macroName = macro[:macroNameMatch.end()]
    macro = macro[macroNameMatch.end():]
    macroArgsBeginMatch = re.match("^\s*\(\s*", macro)

    if not macroArgsBeginMatch:
        print("Error: '(' expected at line " + l + " in " + path)
        return 1, 0, 0, 0
    
    macroArgsEndMatch = re.search(r"\s*\)\s*\{{0,1}\s*\n$", macro)

    if not macroArgsEndMatch:
        print("Error: ')' expected at line " + l + " in " + path)
        return 1, 0, 0, 0
            
    macro = macro[macroArgsBeginMatch.end():macroArgsEndMatch.start()]

    args = re.split("\s*,\s*", macro)

    return 2, type, macroName, args

class Macro:
    def __init__(self, line, name, args, body):
        self.line = line
        self.name = name
        self.args = args
        self.body = body

    def __str__(self) -> str:
        return (self.name + "\t" + str(self.args) + "\tdeclared at line " + self.line)

def compilefile(path):
    f = open(path, "r+")
    count = 0
    currMacro = None
    macroDict = { }
    currBody = ""

    while True:
        count += 1
        l = str(count)
        line = f.readline()

        if not line:
            if currMacro is not None:
                print("Error: Macro '" + currMacro.name + "' was never ended. Don't forget to call '#macro end'.\n       Declared at line " + currMacro.line + " in " + path)
                return False
            break

        valid, type, name, args = getMacro(line, path, l)
        if valid == 0:
            if currMacro is not None:
                currBody += line
            continue
        elif valid == 1:
            return False
        
        # Declaration
        if type == 0:
            if currMacro is not None:
                print("Error: Cannot declare another macro before ending the current macro !\n       at line " + l + " in " + path + "\n       the current macro is declared at line " + currMacro.line + " in the same file.")
                return False
            if name in macroDict:
                print("Error: Macro '" + name + "' is already declared ! at line " + l + " in " + path)
                return False
            
            macroDict[name] = Macro(l, name, args, "")
            currMacro = macroDict[name]

        # End of Declaration
        elif type == 1:
            if currMacro is None:
                print("Error: Cannot call 'end' when a macro was not declared. Called at line " + l + " in " + path)
                return False
            currMacro.body = currBody[:-1]
            print("Ended declaration of macro '" + currMacro.name + "', his body is :\n'" + currMacro.body + "'")
            currMacro = None
            currBody = ""

        # Import
        elif type == 2:
            if currMacro is None:
                print("Error: Cannot call 'import' when a macro was not declared. Called at line " + l + " in " + path)
                return False
            if not os.path.exists(name):
                print("Error: Path '" + name + "' in import macro, doesn't exist. At line " + l + " in " + path)
                return False
            
            currBody += name

        # Execute
        elif type == 3:
            print("Execute macro")

        # Use
        else:
            print("Use macro")

    print("Here are all the macros for '" + path + "':")
    for macro in macroDict.values():
        print(macro)
    print("")

    return True

def compiledir(path):
    for file in os.scandir(path):
        if file.is_dir():
            compiledir(file.path)
        else:
            file_name, file_extension = os.path.splitext(file.path)
            if file_extension == '.cs':
                if not compilefile(file.path):
                    return False
            else:
                os.remove(file.path)

    return True

if 'build' in sys.argv or 'run' in sys.argv:
    if compiledir(".tmp/compiler/src/"):
        print("All good for building !")
    else:
        print("Build failed: macro errors in code.")