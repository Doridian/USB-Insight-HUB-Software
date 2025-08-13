[Setup]
AppName=USB Insight Hub Agent
AppVersion=1.0
DefaultDirName={autopf}\USBInsightHubAgent
CloseApplications=yes
CloseApplicationsFilter=UIHExtractionService.exe
DefaultGroupName=USB Insight Hub Agent
OutputDir=.\InstallerOutput
OutputBaseFilename=USBInsightHubAgent_Setup
Compression=lzma2
SolidCompression=yes
SetupIconFile=yesCon.ico
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "bin\Release\net8.0-windows\win-x64\publish\UIHExtractionService.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\win-x64\publish\noCon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\win-x64\publish\yesCon.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\win-x64\publish\pauseCon.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{userstartup}\USB Insight Hub Agent"; Filename: "{app}\UIHExtractionService.exe"; IconFilename: "{app}\yesCon.ico"
;Name: "{commondesktop}\USB Insight Hub Agent"; Filename: "{app}\UIHExtractionService.exe"
Name: "{group}\USB Insight Hub Agent"; Filename: "{app}\UIHExtractionService.exe"; IconFilename: "{app}\yesCon.ico"

[Run]
Filename: "{app}\UIHExtractionService.exe"; Description: "Launch application"; Flags: postinstall nowait