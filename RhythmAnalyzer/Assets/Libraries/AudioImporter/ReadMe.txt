The package contains 3 different importers:
	-MobileImporter, which is for mobile platforms only. It uses Unity's WWW class, that can stream mp3 files on mobile platforms only.
	-NAudioImporter, which uses NAudio, which is licensed under the Ms-PL. It might require licensing for using MP3 technology.
	-BassImporter, which uses Bass.dll and Bass.net, that require separate licenses for commercial projects. This importer can use the OS's mp3 decoder, so no MP3 license is needed.

MP3 technology is patented and might need additional licensing. More information: https://en.wikipedia.org/wiki/MP3#Licensing.2C_ownership_and_legislation

In order for some of the importers to work properly, the API compatibility level needs to be set to ".NET 2.0" in player settings.
Otherwise it will only work in the editor, if at all.

For Android, Write Permission has to be set to External.

Some assets have external dependencies. These are optional and can be removed.
	-BassImporter requires bass.dll and bass.dll.net
	-VisualizerDemo requires RhythmTool

Importing bass.dll and bass.net.dll:
1. Get bass.dll from http://www.un4seen.com/ and place the required libraries (based on the editor platform and the project's build target) in a Plugins folder. 
2. Get bass.net.dll from http://www.un4seen.com/ and place it somewhere your project's assets folder.
   Bass.Net comes with a number of versions. Unity needs the one from the "V2.0" folder.
   You can register Bass.Net on http://bass.radio42.com/bass_register.html and by editing line 17 of BassImporter.cs.