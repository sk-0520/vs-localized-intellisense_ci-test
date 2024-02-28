cd /d %~dp0

set SOLUTION_PATH=src\VsLocalizedIntellisense.App\VsLocalizedIntellisense.App.sln
set APP_BIN_DIR=src\VsLocalizedIntellisense.App\VsLocalizedIntellisense\bin\Release
set TEST_BASE_DIR=src\VsLocalizedIntellisense.App\VsLocalizedIntellisense.Test\bin
set TEST_BIN_NAME=VsLocalizedIntellisense.Test.dll
set VS_TEST_CONSOLE_PATH=C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TestWindow
set NUGET_TOOL_OpenCover=src\VsLocalizedIntellisense.App\packages\OpenCover.4.7.1221\tools\OpenCover.Console.exe
set NUGET_TOOL_ReportGenerator=src\VsLocalizedIntellisense.App\packages\ReportGenerator.5.2.1\tools\net47\ReportGenerator.exe


"%NUGET_TOOL_OpenCover%" ^
	-register:user ^
	-target:"%VS_TEST_CONSOLE_PATH%\VSTest.Console.exe" ^
	-targetdir:"%TEST_BASE_DIR%\Debug" ^
	-targetargs:"%TEST_BIN_NAME%" ^
	-filter:"+[VsLocalizedIntellisense*]* -[VsLocalizedIntellisense.Test]* " ^
	-output:"result.xml"

