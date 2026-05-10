{ pkgs ? import <nixpkgs> {} }:

pkgs.mkShell {
  packages = with pkgs; [
    dotnet-sdk_8

    fontconfig
    freetype
    libGL

    libx11
    libice
    libsm
    libxi
    libxrandr
    libxrender
    libxcursor
    libxext

    gtk3
    pango
    cairo

    openssl
    zlib
    icu
  ];

  LD_LIBRARY_PATH = pkgs.lib.makeLibraryPath [
    pkgs.fontconfig
    pkgs.freetype
    pkgs.libGL

    pkgs.libx11
    pkgs.libice
    pkgs.libsm
    pkgs.libxi
    pkgs.libxrandr
    pkgs.libxrender
    pkgs.libxcursor
    pkgs.libxext

    pkgs.gtk3
    pkgs.pango
    pkgs.cairo

    pkgs.openssl
    pkgs.zlib
    pkgs.icu
  ];
}