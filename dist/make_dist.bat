set OUTDIR=Plugin_Voiceroid_1_4_4
set INDIR=..\Plugin_Voiceroid\bin\Release

rm -rf %OUTDIR%

mkdir %OUTDIR%

copy readme.txt %OUTDIR%
copy %INDIR%\Plugin_Voiceroid.dll %OUTDIR%

zip -r %OUTDIR%.zip %OUTDIR%

rem set /p TMP_RESULT="Upload OK? [y/n]: "

rem if not "%TMP_RESULT%" == "y" goto skip
rem curl -d "userfile[]=%OUTDIR%.zip&uploader=ebifrier&token=" http://ux.getuploader.com/ebifrier/html > test.html

rem :skip
pause
