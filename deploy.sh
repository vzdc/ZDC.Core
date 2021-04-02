cd /home/ZDC.Core

sudo dotnet publish --output /var/www/ZDC.Core

cd /home/ZDC.Core/ZDC.Core

sudo cp appsettings.json /var/www/ZDC.Core/appsettings.json

sudo dotnet ef database update --configuration Release

cd /var/www/ZDC.Core

sudo chmod -R 777 *

sudo systemctl restart core
