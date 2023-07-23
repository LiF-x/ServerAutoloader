rm art.zip
copy .\utility\utility.cs utility.cs /Y
"C:\Program Files\7-Zip\7z.exe" a art.zip -pchangeme main.cs.dso LICENSE AutoloadConfig.cs jettison.cs utility.cs sha256.cs