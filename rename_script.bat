@echo off
echo Moving src/Core to src/Domain...
move "C:\Users\Usuario.Sooft\Documents\Repo\BotWhatsapp\src\Core" "C:\Users\Usuario.Sooft\Documents\Repo\BotWhatsapp\src\Domain"
if %errorlevel% neq 0 (
    echo Error moving directory.
    exit /b %errorlevel%
)
echo Renaming BotWhatsapp.Core.csproj to BotWhatsapp.Domain.csproj...
ren "C:\Users\Usuario.Sooft\Documents\Repo\BotWhatsapp\src\Domain\BotWhatsapp.Core.csproj" "BotWhatsapp.Domain.csproj"
if %errorlevel% neq 0 (
    echo Error renaming csproj file.
    exit /b %errorlevel%
)
echo Operations complete.