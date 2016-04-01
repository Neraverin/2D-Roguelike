[Setup]
AppName=2D-Roguelike
AppId=2D-Roguelike
AppVersion=v0.0.0.1
DefaultDirName={pf}\2D-Roguelike
DefaultGroupName=2D-Roguelike
UninstallDisplayIcon={app}\2D-Roguelike.exe
Compression=lzma2
SolidCompression=yes
OutputBaseFilename=2D-Roguelike_setup
OutputDir=.
; "ArchitecturesInstallIn64BitMode=x64" requests that the install be
; done in "64-bit mode" on x64, meaning it should use the native
; 64-bit Program Files directory and the 64-bit view of the registry.
; On all other architectures it will install in "32-bit mode".
ArchitecturesInstallIn64BitMode=x64
; Note: We don't set ProcessorsAllowed because we want this
; installation to run on all architectures (including Itanium,
; since it's capable of running 32-bit code too).

[Languages]
Name: en; MessagesFile: "compiler:Default.isl"
Name: ru; MessagesFile: "compiler:Languages\Russian.isl"

[Files]
; Install MyProg-x64.exe if running in 64-bit mode (x64; see above),
; MyProg.exe otherwise.
; Place all x64 files here
Source: "Build\Windows\x86_64\*"; Excludes: "*.pdb"; DestDir: "{app}"; Check: Is64BitInstallMode; Flags: recursesubdirs

; Place all x86 files here, first one should be marked 'solidbreak'
Source: "Build\Windows\x86\*"; Excludes: "*.pdb"; DestDir: "{app}"; Check: not Is64BitInstallMode; Flags: solidbreak recursesubdirs

; Place all common files here, first one should be marked 'solidbreak'
Source: "README.md"; DestDir: "{app}"; DestName: "Readme.txt"; Flags: solidbreak isreadme

[Icons]
Name: "{group}\My Program"; Filename: "{app}\2D-Roguelike.exe"