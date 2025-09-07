@echo off
REM Change to the directory where this batch file is located
cd /d "%~dp0"

REM Symbolic links in ImportantInterfaces\ICompilationResult
REM Create symbolic link folder "ICompiledJsonData" inside "Interfaces\CompilationResult"
REM pointing to "../ICompiledJsonData" relative to "CompilationResult"
mklink /D "ImportantInterfaces\ICompilationResult\ICompiledJsonData" "..\ICompiledJsonData"

REM Create symbolic link folder "IParsedValue" inside "Interfaces\CompilationResult"
REM pointing to "../IParsedValue" relative to "CompilationResult"
mklink /D "ImportantInterfaces\ICompilationResult\IParsedValue" "..\IParsedValue"

REM Create symbolic link folder "IRootParsedValue" inside "Interfaces\CompilationResult"
REM pointing to "../IRootParsedValue" relative to "CompilationResult"
mklink /D "ImportantInterfaces\ICompilationResult\IRootParsedValue" "..\IRootParsedValue"

REM Symbolic links in ImportantInterfaces\IJsonValueQueryResult
REM Create symbolic link folder "IParsedValue" inside "Interfaces\IJsonValueQueryResult"
REM pointing to "../IParsedValue" relative to "IJsonValueQueryResult"
mklink /D "ImportantInterfaces\IJsonValueQueryResult\IParsedValue" "..\IParsedValue"
pause
