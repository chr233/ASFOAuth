# ASFOAuth

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/45b50288f8b14ebda915ed89e0382648)](https://www.codacy.com/gh/chr233/ASFOAuth/dashboard)
![GitHub Workflow Status](https://img.shields.io/github/actions/workflow/status/chr233/ASFOAuth/autobuild.yml?logo=github)
[![License](https://img.shields.io/github/license/chr233/ASFOAuth?logo=apache)](https://github.com/chr233/ASFOAuth/blob/master/license)

[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFOAuth?logo=github)](https://github.com/chr233/ASFOAuth/releases)
[![GitHub Release](https://img.shields.io/github/v/release/chr233/ASFOAuth?include_prereleases&label=pre-release&logo=github)](https://github.com/chr233/ASFOAuth/releases)
![GitHub last commit](https://img.shields.io/github/last-commit/chr233/ASFOAuth?logo=github)

![GitHub Repo stars](https://img.shields.io/github/stars/chr233/ASFOAuth?logo=github)
[![GitHub Download](https://img.shields.io/github/downloads/chr233/ASFOAuth/total?logo=github)](https://img.shields.io/github/v/release/chr233/ASFOAuth)

[![Bilibili](https://img.shields.io/badge/bilibili-Chr__-00A2D8.svg?logo=bilibili)](https://space.bilibili.com/5805394)
[![Steam](https://img.shields.io/badge/steam-Chr__-1B2838.svg?logo=steam)](https://steamcommunity.com/id/Chr_)

[![Steam](https://img.shields.io/badge/steam-donate-1B2838.svg?logo=steam)](https://steamcommunity.com/tradeoffer/new/?partner=221260487&token=xgqMgL-i)
[![爱发电](https://img.shields.io/badge/爱发电-chr__-ea4aaa.svg?logo=github-sponsors)](https://afdian.net/@chr233)

<!-- ASFOAuth 介绍 & 使用指南: [https://keylol.com/t887696-1-1](https://keylol.com/t887696-1-1) -->

> 警告: 安装后所有拥有 Master 权限的用户都将可以使用机器人账号登录第三方网站, 请谨慎使用
> 警告: 安装后所有拥有 Master 权限的用户都将可以使用机器人账号登录第三方网站, 请谨慎使用
> 警告: 安装后所有拥有 Master 权限的用户都将可以使用机器人账号登录第三方网站, 请谨慎使用

## 安装方式

### 初次安装 / 手动更新

1. 从 [GitHub Releases](https://github.com/chr233/ASFOAuth/releases) 下载插件的最新版本
2. 解压后将 `ASFOAuth.dll` 丢进 `ArchiSteamFarm` 目录下的 `plugins` 文件夹
3. 重新启动 `ArchiSteamFarm` , 使用命令 `ASFOAUTH` 或 `ASFO` 来检查插件是否正常工作

### ASFEnhance 联动

> 推荐搭配 [ASFEnhance](https://github.com/chr233/ASFEnhance) 使用, 可以通过 ASFEnhance 实现插件更新管理和禁用特定命令等功能

### 更新日志

| ASFOAuth 版本                                                      | 适配 ASF 版本 | 更新说明                         |
| ------------------------------------------------------------------ | :-----------: | -------------------------------- |
| [1.1.0.0](https://github.com/chr233/ASFOAuth/releases/tag/1.1.0.0) |   5.4.12.5    | ASF -> 5.4.12.5, 接入 ASFEnhance |
| [1.0.1.0](https://github.com/chr233/ASFOAuth/releases/tag/1.0.1.0) |    5.4.8.3    | ASF -> 5.4.8.3                   |
| [1.0.0.2](https://github.com/chr233/ASFOAuth/releases/tag/1.0.0.2) |    5.4.5.2    | 第一个版本                       |

## 插件配置说明

> 本插件的配置项名称已改为 ASFEnhance

ASF.json

```json
{
  //ASF 配置
  "CurrentCulture": "...",
  "IPCPassword": "...",
  "...": "...",
  //ASFOAuth 配置
  "ASFEnhance": {
    "EULA": true,
    "Statistic": true
  }
}
```

| 配置项      | 类型 | 默认值  | 说明                                                                 |
| ----------- | ---- | ------- | -------------------------------------------------------------------- |
| `EULA`      | bool | `false` | 是否同意 [EULA](#EULA)\*, 当设置为 `true` 时, 视为同意 [EULA](#EULA) |
| `Statistic` | bool | `true`  | 是否允许发送统计数据, 仅用于统计插件用户数量, 不会发送任何其他信息   |

> 当某条命令被禁用时, 仍然可以使用 `ASFO.xxx` 或者 `ASFOAUTH.xxx` 的形式调用被禁用的命令, 例如 `ASFO.OAUTH`

## 插件指令说明

### 插件更新

| 命令       | 缩写   | 权限            | 说明                 |
| ---------- | ------ | --------------- | -------------------- |
| `ASFOAuth` | `ASFO` | `FamilySharing` | 查看 ASFOAuth 的版本 |

### 功能指令

| 命令              | 缩写 | 权限     | 说明                                                                      |
| ----------------- | ---- | -------- | ------------------------------------------------------------------------- |
| `OAUTH [Bot] Uri` | `O`  | `Master` | 自动使用机器人身份通过 SteamOpenId 登录第三方网站, 返回跳转回第三方的网址 |
