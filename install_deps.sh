# for Ubuntu 24.04

apt install -y \
            dotnet-sdk-10.0 \
            libgdiplus \
            libx11-6 \
            libx11-dev \
            libxext6 \
            libxrender1 \
            libxtst6 \
            fonts-liberation

mkdir -p ~/.config/fontconfig
cp fonts.conf ~/.config/fontconfig/fonts.conf
fc-cache -f -v
