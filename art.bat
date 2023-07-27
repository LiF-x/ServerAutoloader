rm art.zip
copy .\ServerUtility\utility.cs utility.cs /Y
"C:\Program Files\7-Zip\7z.exe" a art.zip -pchangeme main.cs LICENSE AutoloadConfig.cs jettison.cs utility.cs sha256.cs dump.sql