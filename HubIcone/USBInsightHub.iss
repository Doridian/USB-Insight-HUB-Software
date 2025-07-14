[Setup]
AppName=USB Insight Hub
AppVersion=1.0
DefaultDirName={autopf}\USBInsightHub
DefaultGroupName=USB Insight Hub
OutputDir=.\InstallerOutput
OutputBaseFilename=USBInsightHub_Setup
Compression=lzma2
SolidCompression=yes
SetupIconFile=greenCircle.ico
ArchitecturesAllowed=x64
ArchitecturesInstallIn64BitMode=x64

[Files]
Source: "bin\Release\net8.0-windows\win-x64\publish\HubIcone.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\win-x64\publish\greenCircle.ico"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net8.0-windows\win-x64\publish\redCircle.ico"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{userstartup}\USB Insight Hub"; Filename: "{app}\HubIcone.exe"
Name: "{commondesktop}\USB Insight Hub"; Filename: "{app}\HubIcone.exe"
Name: "{group}\USB Insight Hub"; Filename: "{app}\HubIcone.exe"

[Run]
Filename: "{app}\HubIcone.exe"; Description: "Launch application"; Flags: postinstall nowait