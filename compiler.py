# I got really annoyed about retyping code everywhere. C# doesn't have support for Macros, which is a shame. Codes snippets are not macros. 
# Code generators needs Roslyn and are too overpowered when all I want is a fucking #macro preprocessor that replaces a string by another right 
# before compiling.
# It is so fucking simple to implement that I am going to use this python script to run/build my project instead of using `dotnet`.

import os
import shutil
import sys
import re
import subprocess
import time

class Macro:
	def __init__(self, line, name, args, body):
		self.line = line
		self.name = name
		self.args = args
		self.body = body

	def __str__(self) -> str:
		return (self.name + "\t" + str(self.args) + "\tdeclared at line " + self.line)

def getMacro(line, path, l):
	match = re.match("^\s*((\/\*)|(\/\/)){0,1}\s*#macro\s+", line)
	if not match:
		return 0, 0, 0, 0

	macro = line[match.end():]

	macroTypeMatch = re.match("^((decl)|(use)|(end)|(import)|(execute)|(header)|(implend)|(impl))\s*", macro)

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
	elif macro.startswith("header"):
		return 2, 4, macro[macroTypeMatch.end():-1], 0
	elif macro.startswith("use"):
		type = 5
	elif macro.startswith("implend"):
		return 2, 7, 0, 0
	elif macro.startswith("impl"):
		return 2, 6, 0, 0
	
	else:
		return 1, 0, 0, 0
	
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

macroDict = { }
currMacro = None
currBody = ""

def declaration(path, l, name, args):
	global macroDict
	global currMacro
	global currBody

	if currMacro is not None:
		print("Error: Cannot declare another macro before ending the current macro !\n	   at line " + l + " in " + path + "\n	   the current macro is declared at line " + currMacro.line + " in the same file.")
		return False
	if name in macroDict:
		print("Error: Macro '" + name + "' is already declared ! at line " + l + " in " + path)
		return False
			
	macroDict[name] = Macro(l, name, args, "")
	currMacro = macroDict[name]

	return True

def enddeclaration(path, l, name, args):
	global macroDict
	global currMacro
	global currBody

	if currMacro is None:
		print("Error: Cannot call 'end' when a macro was not declared. Called at line " + l + " in " + path)
		return False
	currMacro.body = currBody[:-1]
	#print("Ended declaration of macro '" + currMacro.name + "', his body is :\n'" + currMacro.body + "'")
	currMacro = None
	currBody = ""

	return True

def macroimport(path, l, macroPath, args):
	global macroDict
	global currMacro
	global currBody

	if currMacro is None:
		print("Error: Cannot call 'import' when a macro was not declared. Called at line " + l + " in " + path)
		return False
	if not os.path.exists(macroPath):
		print("Error: Path '" + macroPath + "' in import macro, doesn't exist. At line " + l + " in " + path)
		return False
			
	with open(macroPath, 'r') as file:
		newBody = file.read() + "\n"
		line = ""
		for c in newBody:
			if c == '\n':
				break
			line += c
		valid, type, name, args = getMacro(line, macroPath, "0")
		if valid == 1:
			print("Error at line 0 in " + macroPath)
			return False
		if type != 4:
			currBody += newBody
			return True
		
		currBody += newBody[len(line):]

	return True

def execute(path, l, cmdPath, args):
	global macroDict
	global currMacro
	global currBody

	if os.path.exists(cmdPath):
		print("Error: Python file at path '" + cmdPath + "' doesn't exist.")
		return False

	proc = subprocess.run(["python3", cmdPath])
	return proc.communicate()[0]

def handlebodyline(line, macro, args, first):
	global macroDict
	global currMacro
	global currBody

	word = ""
	newLine = "/**/	"
	concat = 0

	if not first:
		newLine = "\n" + newLine

	for c in line:
		if c.isalnum():
			word += c
			continue
		
		if c == '#':
			if concat == 0:
				concat = 1
			elif concat == 1:
				concat = 2
			else:
				newLine += '#'
		else:
			concat = 0
		
		if word in macro.args:
			i = macro.args.index(word)
			newLine += args[i]
		else:
			newLine += word

		if concat == 0:
			newLine += c
		word = ""

	if word in macro.args:
		i = macro.args.index(word)
		newLine += args[i]
	else:
		newLine += word

	return newLine

def use(file, line, path, l, name, args):
	global macroDict
	global currMacro
	global currBody

	if name not in macroDict:
		print("Error: Macro '" + name + "' is not declared ! at line " + l + " in " + path)
		return False
	
	macro = macroDict[name]

	file.write("//#macro impl\n")

	first = True
	for bLine in macro.body.splitlines():
		handledLine = handlebodyline(bLine, macro, args, first)
		file.write(handledLine)
		first = False
	
	file.write("\n//#macro implend\n")

	return True

keepTmp = False

def compilefile(clearImpl, path):
	global macroDict
	global currMacro
	global currBody
	global keepTmp

	if os.path.exists(path + ".tmp"):
		os.remove(path + ".tmp")

	f = open(path, "r")
	tmpF = open(path + ".tmp", "w")
	count = 0
	macroDict = { }
	currMacro = None
	currBody = ""
	hadMacros = False
	isImplementing = False

	while True:
		count += 1
		l = str(count)
		line = f.readline()

		if not line:
			if currMacro is not None:
				print("Error: Macro '" + currMacro.name + "' was never ended. Don't forget to call '#macro end'.\n       Declared at line " + currMacro.line + " in " + path)
				return False
			if isImplementing:
				print("Error: Macro implementation was never ended. Do not touch the '#macro impl' and '#macro implend'.")
				return False
			break

		valid, type, name, args = getMacro(line, path, l)
		# There is no macros at this line
		if valid == 0:
			# Do not register what's inside the implementation macros
			if isImplementing:
				continue
			
			if currMacro is not None:
				currBody += line
				
			tmpF.write(line)
			continue
		# There was an error at this line
		elif valid == 1:
			f.close()
			tmpF.close()
			return False
		
		#There is a macro at this line !
		hadMacros = True

		# Do not register the implementation macros themselves
		if type != 6 and type != 7:
			tmpF.write(line)

		# Found declaration macro
		if type == 0:
			if not declaration(path, l, name, args):
				f.close()
				tmpF.close()
				return False

		# Found end macro
		elif type == 1:
			if not enddeclaration(path, l, name, args):
				f.close()
				tmpF.close()
				return False

		# Found import macro
		elif type == 2:
			if not macroimport(path, l, name, args):
				f.close()
				tmpF.close()
				return False

		# Found execute macro
		elif type == 3:
			result = execute(line, l, name, args)
			if not result:
				f.close()
				tmpF.close()
				return False
			currBody += result

		# Found header macro
		elif type == 4:
			print("Header can only be used in the top most line of a .macro file. At line " + l + " in " + path)
			f.close()
			tmpF.close()
			return False

		# Found use macro
		elif type == 5:
			if not clearImpl:
				continue

			if not use(tmpF, line, path, l, name, args):
				f.close()
				tmpF.close()
				return False
			
		# Found impl macro
		elif type == 6:
			if isImplementing:
				print("Error: Cannot implement another macro before ending the current macro !\n       at line " + l + " in " + path)
				f.close()
				tmpF.close()
				return False
			isImplementing = True

		# Found implend macro
		elif type == 7:
			if not isImplementing:
				print("Error: Cannot end macro implementation when none were started !\n       at line " + l + " in " + path)
				f.close()
				tmpF.close()
				return False
			isImplementing = False
			
		# Found no macro ! Not supposed to happen
		else:
			print("Invalid macro type (" + type + "). At line " + l + " in " + path)
			return False

	f.close()
	tmpF.close()

	shutil.copyfile(path + ".tmp", path)
	if not hadMacros or not keepTmp:
		os.remove(path + ".tmp")

	return True

def compiledir(clearImpl, path):
	for file in os.scandir(path):
		if file.is_dir():
			compiledir(clearImpl, file.path)
		else:
			file_name, file_extension = os.path.splitext(file.path)
			if file_extension == '.cs':
				if not compilefile(clearImpl, file.path):
					return False
			else:
				os.remove(file.path)

	return True

def compile(clearImpl):
	if not os.path.exists(".tmp"):
		os.mkdir(".tmp")

	if not os.path.exists(".tmp/compiler"):
		os.mkdir(".tmp/compiler")

	if os.path.exists(".tmp/compiler/src"):
		shutil.rmtree(".tmp/compiler/src")

	shutil.copytree("src/", ".tmp/compiler/src/")
	return compiledir(clearImpl, "src/")

if len(sys.argv) < 1:
	print("Error: Missing parameter.")
	exit(-1)

try:
	clearImpl = True
	if sys.argv[1] == 'clear':
		clearImpl = False

	#if compile(clearImpl):
	if compiledir(clearImpl, "src/"):
		print("Compiling done !")

		if sys.argv[1] == 'build' or sys.argv[1] == 'run':
			print("Calling 'dotnet " + sys.argv[1] + "'")
			subprocess.run(["dotnet", sys.argv[1]])
	else:
		print("Build failed: macro errors in code.")
		exit(-1)
except Exception as e:
	print("Build failed: errors in code:\n" + str(e))
	exit(-1)
