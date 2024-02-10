@echo off
SETLOCAL EnableDelayedExpansion

cd ..
:: Assign command line arguments to variable
SET "PROJECT_DIR=%~1"
SET "PROJECT_PATH=%~2"

python scripts\pack.py %PROJECT_DIR% %PROJECT_PATH%


:END
ENDLOCAL
