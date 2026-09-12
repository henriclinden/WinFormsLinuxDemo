# Getting Started

Steps:

 - Install dotnet 8 or 10.
 - Then install X11 dependencies.

         sudo apt install -y libgdiplus \
            libx11-6 \
            libx11-dev \
            libxext6 \
            libxrender1 \
            libxtst6

    or (simpler)

         sudo apt install -y libgdiplus libc6-dev

 - Enable X11 support in Wayland
 - Create project

         dotnet new console -n WinFormsLinuxDemo

 - Add WinForms

         dotnet add package Core.System.Windows.Forms



# Troubleshooting Tips
Missing Font Rendering: If text appears incorrectly or crashes, install Microsoft core fonts on Ubuntu:

    sudo apt install -y ttf-mscorefonts-installer
    sudo fc-cache -f -v

GDI Exception: If you encounter TypeInitializationException involving System.Drawing, double-check that libgdiplus is installed and the <RuntimeHostConfigurationOption Include="System.Drawing.EnableUnixSupport" Value="true"/> entry is present in your .csproj.

