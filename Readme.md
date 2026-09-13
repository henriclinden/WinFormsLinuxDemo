# WinForms Linux Demo

## Introduction

This project is a demonstration of running a Windows Forms (WinForms) application on Linux under .NET with Mono/GDI+ support (`libgdiplus`). It showcases desktop UI capabilities, standard controls, dialogs, custom GDI+ drawing, and a real-time industrial-style Signal HMI dashboard featuring analog gauges and multi-channel trend viewers.

While the setup steps below focus on Ubuntu 24.04 as an example, this application works across many types of Linux environments, including various embedded Linux distributions (such as Yocto-, Buildroot-, or Debian-based systems on ARM/x86 hardware) provided that .NET runtime, X11/Wayland-XWayland, and `libgdiplus` are available.

# Getting Started on Ubuntu 24.04

Steps:

- Install .NET 10.

        sudo apt install -y dotnet-sdk-10.0

- Install the X11 dependencies:

        sudo apt install -y libgdiplus \
            libx11-6 \
            libx11-dev \
            libxext6 \
            libxrender1 \
            libxtst6

- Enable X11 support in Wayland.
- Clone the project and build.

         dotnet restore
         dotnet run

# Troubleshooting Tips

## Missing Font Rendering #1
No text in standard buttons or message boxes

This is caused by a font scaling problem. On many Linux systems, the default Microsoft fonts are aliased to Noto Sans. This is a great font, but it has scaling problems when used together with Mono.

Install the Liberation Sans font package and create aliases for the fonts used by Mono.

Create the font configuration file and edit:

                mkdir -p ~/.config/fontconfig
                vim ~/.config/fontconfig/fonts.conf

Paste the XML block below into the file. In this example, "System" and "Microsoft Sans Serif" are aliased to Liberation Sans, while "MS Sans Serif" is aliased to Ubuntu. You can replace the names inside the <string> tags under <edit> with any font currently shown in your fc-list.

                <?xml version="1.0"?>
                <!DOCTYPE fontconfig SYSTEM "fonts.dtd">
                <fontconfig>

                <!-- Map "System" font to Liberation Sans -->
                <match target="pattern">
                        <test name="family" qual="any">
                        <string>System</string>
                        </test>
                        <edit name="family" mode="assign" binding="strong">
                        <string>Liberation Sans</string>
                        </edit>
                </match>

                <!-- Map Windows "Microsoft Sans Serif" to Liberation Sans -->
                <match target="pattern">
                        <test name="family" qual="any">
                        <string>Microsoft Sans Serif</string>
                        </test>
                        <edit name="family" mode="assign" binding="strong">
                        <string>Liberation Sans</string>
                        </edit>
                </match>

                <!-- Map Windows "MS Sans Serif" (or "San Serif") to Ubuntu font -->
                <match target="pattern">
                        <test name="family" qual="any">
                        <string>MS Sans Serif</string>
                        </test>
                        <edit name="family" mode="assign" binding="strong">
                        <string>Ubuntu</string>
                        </edit>
                </match>

                </fontconfig>

Rebuild the font configuration cache:

                fc-cache -f -v

Check that the Microsoft fonts are now aliased to Liberation Sans and not Noto Sans:

                fc-match System
                LiberationSans-Regular.ttf: "Liberation Sans" "Regular"

                fc-match "Microsoft Sans Serif"
                LiberationSans-Regular.ttf: "Liberation Sans" "Regular"

## Missing Font Rendering #2
If text appears incorrectly or the application crashes, install the Microsoft Core Fonts on Ubuntu:

    sudo apt install -y ttf-mscorefonts-installer
    sudo fc-cache -f -v

GDI Exception: If you encounter a TypeInitializationException involving System.Drawing, double-check that libgdiplus is installed and that the following entry is present in your .csproj:

                <RuntimeHostConfigurationOption Include="System.Drawing.EnableUnixSupport" Value="true"/>

