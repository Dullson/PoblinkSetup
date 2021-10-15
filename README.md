# Poblink Setup Tool
This tool enables loading build URLs and codes directly into Path of Building, eliminating the need to start the application first and go through importing steps.

The mechanism is pretty similar to registering a file extension to open with the chosen application (like opening video files with your preferred video player). But instead of file type, we are using a custom URI protocol to forward link data directly into Path of Building.

After setup, "pob:" prefixed links are recognized as Poblinks, and they open with Path of Building when clicked.

Example Pastebin link and its Poblink version:
- https://pastebin.com/2TWTQ5z3
- [pob://pastebin/2TWTQ5z3](pob:pastebin/2TWTQ5z3)

I have also created a userscript called [Poblink](https://www.github.com/dullson/poblink) to add transformed links next to the existing Pastebin links on various sites such as Poe.Ninja, Poe forums, Youtube, Twitch chat, and many more.

Right now, the poblink format is pretty limited, but hopefully there will be improvements soon.

## How to install?
- You can either compile the project yourself or head over to the [releases page](https://www.github.com/dullson/poblinksetup/relases) and download the latest binary.
 - Run the application with Administrator permissions (or else the registry modification would fail).
 - Select your Path of Building.exe path
 - Click the "Install Settings" button.
- Done!

You can also uninstall the registry configuration by pressing the remove setting button on the bottom right.

## Manual Setup
I get that some people might not want to interact with random executables on the internet.
If that is the case, you can also manually import the required registry keys.

Copy the code below into a text editor, and replace the path on the last line with your Path of Building.exe path.
Pay attention to double slashes on this step.

```
Windows Registry Editor Version 5.00

[HKEY_CLASSES_ROOT\pob]
"URL Protocol"=""
@="URL:Path of Building"

[HKEY_CLASSES_ROOT\pob\shell]

[HKEY_CLASSES_ROOT\pob\shell\open]

[HKEY_CLASSES_ROOT\pob\shell\open\command]
@="\"C:\\path\\to\\your\\Path of Building.exe\" \"%1\""
```
Save this file with `.reg` extension (for example `poblink.reg`) and run it with Administrator privileges to import.
Now you should be able to open pob protocol links with Path of Building.


