# ModuleHelper
Windows Presentation Foundation application that helps writing music in old tracker software.
<br />

![image](https://user-images.githubusercontent.com/47994455/91340968-200f1400-e7d9-11ea-9c77-4de61121d259.png)
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
- .NET Core 3.1
## Usage:
Compile with:
```
dotnet build
```
In order to modify musical scales used by program edit musicalscales.xml file. Be sure to use proper tree structure and valid XML syntax.
