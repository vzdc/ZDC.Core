cd /home/jake/ZDC.Core

dotnet publish --output /var/www/ZDC.Core

cd /home/jake/ZDC.Core/ZDC.Core

sudo cp appsettings.json /var/www/ZDC.Core/appsettings.json

dotnet-ef database update --configuration Release

cd /var/www/ZDC.Core

sudo chmod -R 755 *

sudo systemctl restart core
