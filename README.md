# IniFile
[![Build status](https://ci.appveyor.com/api/projects/status/7eb2p5oqr8ap7bxe?svg=true)](https://ci.appveyor.com/project/IncoCode/inifile)

DLL that allow work with ini-files in C#.

## Usage
1. Download the [latest release](https://github.com/IncoCode/IniFile/releases/tag/1.0.1-Build-8);
2. Add reference to `Ini.dll` (or you can just take [`Ini.cs`](https://github.com/IncoCode/IniFile/blob/master/IniFile/IniFile.cs) from source and add it directly in your project, then you don't need an dll);

### Example
```cs
using Ini;
...
private readonly IniFile _settings;
...
public YouConstructor()
{
  // load settings
  this._settings = new IniFile( "Path_To_Your_Ini.ini" );
}
...
// somewhere in your app

// read (default value can be of the following type: string, int, bool, double)
int myDefaultInt = 4;
int myVar = this._settings.Read( "Key", "Section", myDefaultInt );
string myStr = this._settings.Read( "MyKey", "Section2", "defaultSrting" );

// write
this._settings.Write( "Key", 5, "Section" );
this._settings.Write( "Key", "str", "Section2" );
```
