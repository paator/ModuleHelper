# ModuleHelper
Windows Presentation Foundation application that helps writing music in old tracker software.
<br />

![image](https://user-images.githubusercontent.com/47994455/91736709-633cfe80-ebae-11ea-853d-0d9649ec72f5.png)
## Currently supported features
- Quick access to various musical scales that can be manually added via xml file
- Building chords (arpeggios) on 3 piano octaves
- Restricting piano keys basing on currently selected scale
- Displaying arp notation in hexadecimal or decimal numeral system
- Current chord (arpeggio) sound preview with customizable arpeggio speed
## Libraries used
- [NAudio](https://github.com/naudio/NAudio)
- [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)
## Requirements
- .NET Core 3.1 Runtime
## Usage:
Download newest release [there](https://github.com/paator/ModuleHelper/releases/tag/v1.0). Be sure to install proper .NET Core 3.1 runtime - x64 or x32, depending on downloaded build (and your system version).
In order to modify musical scales used by program edit musicalscales.xml. Be sure to use proper tree structure and valid XML syntax.

If you want to compile the source code, run
```
dotnet build
```
in project's root folder.
