@echo off

cd Source

pyinstaller --onefile main.py

echo Ejecutable creado

cd dist

MOVE main.exe ../../../Grafica

cd ../../

main.exe

echo Grafica creada

@pause