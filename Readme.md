# JLUDrcomService
欢迎使用 `JLUDrcomService` ！这是一款由 `C#` 编写，基于 `MIT License` 开源的 `Drcom` 客户端。该客户端为 `Windows 服务`，因此只针对 `Windows` 平台有效。

## 安装
本程序的安装分为四步：
1. 从[https://github.com/Yesterday17/JLUDrcomService/releases](https://github.com/Yesterday17/JLUDrcomService/releases)下载最新版本的 `JLUDrcomService`，并且通过安装包引导安装。
2. 在`服务`列表中找到 `JLUDrcomService` 或`吉林大学校园网登录服务`，选择启动。
3. 在发现服务自动停止后，打开注册表编辑器(`regedit`)，找到 `HKEY_CURRENT_CONFIG\Software\JLUDrcomService` ，修改其中的 `username` 与 `password`。
4. 重新启动服务。当服务处于`正在运行`状态时，应该就可以正常浏览了。

## 卸载
本程序的卸载分为三步：
1. 在`服务`列表中停止该服务。
2. 以管理员权限运行安装目录下的 `Remove.bat` 。
3. 正常卸载。

## 鸣谢
 - [Wireshark](https://www.wireshark.org/)
 - [jlu-drcom-client](https://github.com/drcoms/jlu-drcom-client)
 - [dr-jlu-win32](https://github.com/code4lala/dr-jlu-win32)