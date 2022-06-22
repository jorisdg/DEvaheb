# Using DEvaheb

## Command line
DEvaheb 2.0 is command line only. However, it can open a text editor of your choosing when it has decompiled (such as notepad, BehavED,VS Code, etc.).

## Quick Start
The DEvahebOpen.cmd file wraps the executable and opens notepad. You can use this cmd file instead of the .exe to open your IBI files. You can edit the .CMD file in notepad (right-click Edit in Windows explorer) and instead of opening "notepad" you can point it to the location of BehavED.exe on your system, etc.

## Command Line Options
DEvaheb.exe *path_to_IBI_file*

All of the command line options can be specified in any order, including the IBI file path. Without any further options, the IBI file will be decompiled to the same file with extension .icarus and in the same folder as the original IBI file.

- *-output "path_for_new_source_file"* : Optional argument to explicitly specify a path and filename for the source file DEvaheb will generate.
-  *-extension "txt"* : Optional argument to use a different extension for the generated source file. This is only useful when not specifying the -output argument explicitly.
- *-open "path_for_tooling* : Optional argument with a path to a tool of your choosing. DEvaheb will start this tool when decompilation is finished, and pass the source file path as the first and only argument to this tool.

## Variables and Types
Depending on the game and potential modifications, you may want to adjust types and variables being put in source files, for usage with the BehavED editor to drive choices in the UI. The *variable_types.csv* file can be edited to add or remove variable information. The current file was generated from the Q3_Interface.h file in the JEDI Academy SDK.

The following columns are read by DEvaheb to generate the type comments in source:

* First column: variable name
* Second column: type name. Use STRING, FLOAT, INT, VECTOR  **without quotes** for the standard data types. Specify any other type names in quotes to indicate an enumeration that will be included in source.
* Third column: indicate if the variables is readonly (use the value 1). There is currently no use for this option.