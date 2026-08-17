# Clash Of Drayvens

Clash Of Drayvens is a maintained fork of the open-source **Clash Of SL** private-server project, configured for the Drayvens server infrastructure.

> This repository is not affiliated with or endorsed by Supercell Oy. The server/tooling code is maintained here under the repository's existing license. Proprietary Clash of Clans client code, artwork, audio and trademarks are **not** redistributed by this repository or its releases.

## Production endpoint

- Drayvens game server: `irautox.ir:7676` (TCP)
- Legacy Android compatibility gateway: `TCP 9339 -> irautox.ir:7676`
- Supported legacy protocol/client version: `8.709.16`

`Clash SL Server/config.css` is preconfigured to listen on TCP `7676`.

## Components

- **Server** — game server, MySQL persistence, Redis cache and game protocol.
- **Gateway / Proxy** — listens on legacy TCP `9339` and forwards to `irautox.ir:7676`.
- **Test Client** — small .NET protocol client preconfigured for `irautox.ir:7676`.
- **Client Patcher** — patches the supported Android client's `libg.so` public key for private-server compatibility.
- **File Decryptor** — utility for supported game resource files.
- **SC Editor** — utility for supported SC resource files.

## Windows VPS requirements

- Windows Server x64
- .NET Framework 4.8 (server itself targets .NET Framework 4.6.2)
- MySQL/MariaDB with database `cssdb`
- Redis on `127.0.0.1:6379`
- Inbound TCP ports `7676` and `9339`

The default database settings are in `Clash SL Server/config.css`:

```text
Redis: 127.0.0.1:6379
MySQL: 127.0.0.1:3306
Database: cssdb
User: root
```

Set a real MySQL password before exposing the VPS publicly.

## Runtime order

1. Start MySQL/MariaDB.
2. Start Redis.
3. Start `ClashOfDrayvens.Server.exe` (game server on `7676`).
4. Start `ClashOfDrayvens.Gateway.exe` (legacy compatibility listener on `9339`).
5. Point the compatible Android client's `gamea.clashofclans.com` hostname to the VPS IP for private testing.

The gateway exists because the supported legacy Android client expects TCP `9339`; it forwards that connection to the Drayvens server on `7676`.

## Android client compatibility testing

The upstream Clash Of SL README references a **Clash Of SL 8.709.1** compatibility APK. This fork does not mirror or redistribute that proprietary APK. For private compatibility testing, obtain the client only from a source you are legally allowed to use.

The included client patcher works on `libg.so`:

1. Decode your legally obtained compatible APK with Apktool / APK Easy Tool.
2. Copy `lib/armeabi-v7a/libg.so` (or the matching ABI path in that APK) into the patcher's `Original` directory.
3. Run the Client Patcher and approve replacing the public key.
4. Copy the resulting `Patched/libg.so` back into the decoded APK.
5. For a private test build, you may change the Android app label/icon to **Clash Of Drayvens**.
6. Rebuild the APK and sign it with your own test keystore.
7. Redirect `gamea.clashofclans.com` to your VPS IP. The gateway handles `9339 -> 7676`.

### Public app-store release

Do **not** publish a renamed Supercell APK/assets as Clash Of Drayvens. A public Play Store / Bazaar / Myket release needs an original client and original/licensed art, audio, UI and branding. The open-source server can be reused as a protocol/backend reference where its license permits it.

## Build and releases

GitHub Actions builds the Windows server, gateway, test client and available tools. Successful pushes to `main` create a versioned GitHub Release containing a ZIP runtime bundle.

The release bundle is organized as:

```text
Clash-Of-Drayvens/
  Server/
  Gateway/
  TestClient/
  Tools/
  README-RUN.txt
```

## Credits

Clash Of Drayvens is based on the Clash Of SL open-source project by its original contributors. Original attribution and repository license remain applicable to inherited code.
