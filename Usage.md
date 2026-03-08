# Using The Tools

- [DEvaheb.exe usage](#DEvaheb)
- [suraci.exe usage](#suraci)

## DEvaheb

### Command line
DEvaheb 2.0 is command line only. However, it can open a text editor of your choosing when it has decompiled (such as notepad, BehavED, VS Code, etc.).

### Quick Start
The DEvahebOpen.cmd file wraps the executable and opens notepad. You can use this cmd file instead of the .exe to open your IBI files. You can edit the .CMD file in notepad (right-click Edit in Windows explorer) and instead of opening "notepad" you can point it to the location of BehavED.exe on your system, etc.

### Command Line Options
DEvaheb.exe *path_to_IBI_file*

All of the command line options can be specified in any order, including the IBI file path. Without any further options, the IBI file will be decompiled to the same file with extension .icarus and in the same folder as the original IBI file.

- *-output "path_for_new_source_file"* : Optional argument to explicitly specify a path and filename for the source file DEvaheb will generate.
-  *-extension "txt"* : Optional argument to use a different extension for the generated source file. This is only useful when not specifying the -output argument explicitly.
- *-open "path_for_tooling* : Optional argument with a path to a tool of your choosing. DEvaheb will start this tool when decompilation is finished, and pass the source file path as the first and only argument to this tool.
- *-nocompat* : Optional argument that generates source files without BehavED comments and UI signals. There will be no commented type information, and IF statements become clean expressions. These files will still compile with Ibize just fine, they will just not trigger most of the GUI dropdowns in BehavED.
- *-variables "path_to_variables_csv"* : Optional argument specifying a path to a variables CSV. By default it will load the CSV file that sits next to the DEvaheb executable.

### Variables and Types
Depending on the game and potential modifications, you may want to adjust types and variables being put in source files, for usage with the BehavED editor to drive choices in the UI. The *variable_types.csv* file can be edited to add or remove variable information. The current file was generated from the Q3_Interface.h file in the JEDI Academy SDK.

The following columns are read by DEvaheb to generate the type comments in source:

* First column: variable name
* Second column: type name. Use STRING, FLOAT, INT, VECTOR  **without quotes** for the standard data types. Specify any other type names in quotes to indicate an enumeration that will be included in source.
* Third column: indicate if the variables is readonly (use the value 1). There is currently no use for this option.

## suraci

### Command line
Suraci 1.0 is command line only.

### Quick Start
If you are using BehaveED, you can replace the location of IBIze.exe with the location of suraci. If there are warnings or errors, a log file will open with the details.

IBIze will start writing the IBI file as it's reading the Icarus file. This may result in broken IBI files. Suraci will only start writing the IBI after succesfully parsing and not finding any validation errors.

### Command Line Options
suraci.exe *path_to_source_file*

All of the command line options can be specified in any order, including the source file path.

- *-output "filepath"* : Optional argument to specify a path or filename to save compiled IBI to. By default IBI file will be placed next to their source file.
- *-extension "icarus"* : Optional argument to specify a file extension to look for when searching folders for source files, default is "txt".
- *-v132* : Optional argument to create IBI files compatible with v1.32 of Icarus (Elite Force, SoF2).
- *-v133* : This optional argument is used by default. This creates IBI files compatible with v1.33 of Icarus (Jedi Knight, Jedi Academy).
- *-a* : Optional argument for when a path is specified, this compiles all files in that directory and all sub directories recursively.
- *-e* : Optional argument that outputs errors and warnings to a log file and opens that log file when done. BehavED will use this flag when calling the compiler.
- *-logpath* : path to save log file when using -e option (default is in /logs/ subdirectory of location of suraci.exe)
- *-strict* : report warnings as errors (and fail compile on warnings)

Note: The original -e flag of IBIze.exe was "pause on error" which is used by the BehavED editor to show output. Newer versions of Windows don't seem to show the cmd window, essentially hanging BehavED on error. Using a log file is the alternative I chose to use for suraci to also surface the warnings and errors.

If output file path is not specified, the resulting IBI file will be placed next to the original source file.

### Warnings and Errors

#### Parsing Errors

| Type | ID | Message | Description |
| --- | --- | --- | --- |
| Warning | WAR0002 | No semi-colon found at the end of expression '{nodeName}' | Missing semi-colon at the end of a line. This is not strictly needed to compile correctly. |

#### Icarus Validations

| Type | ID | Message | Description |
| --- | --- | --- | --- |
| Warning | WAR0001 | Defining a task inside a loop is not allowed | Tasks should be defined outside loops. |
| Warning | WAR0003 | Task '{taskName}' (for entity '{entityName}') is not defined in this script | The task called from do/wait/dowait may be valid for that entity, but it's not defined in this script file so couldn't be verified to exist. This could be an actual mistake based on affect() scope. |