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
3. 重新启动 `ArchiSteamFarm` , 使用命令 `ABB` 来检查插件是否正常工作

### 使用命令升级插件

> 可以使用插件自带的命令自带更新插件
> ASF 版本升级有可能出现不兼容情况, 如果发现插件无法加载请尝试更新 ASF

- `ABBVERSION` / `ABBV` 检查插件更新
- `ABBUPDATE` / `ABBU` 自动更新插件到最新版本 (需要手动重启 ASF)

### 更新日志

| ASFOAuth 版本                                                      | 适配 ASF 版本 | 更新说明   |
| ------------------------------------------------------------------ | :-----------: | ---------- |
| [1.0.0.1](https://github.com/chr233/ASFOAuth/releases/tag/1.0.0.1 |    5.4.5.2    | 第一个版本 |

## 插件配置说明

> 本插件的配置不是必须的, 保持默认配置即可使用大部分功能

ASF.json

```json
{
  //ASF 配置
  "CurrentCulture": "...",
  "IPCPassword": "...",
  "...": "...",
  //ASFOAuth 配置
  "ASFOAuth": {
    "Statistic": true,
    "DisabledCmds": ["foo", "bar"]
  }
}
```

| 配置项         | 类型 | 默认值 | 说明                                                                            |
| -------------- | ---- | ------ | ------------------------------------------------------------------------------- |
| `Statistic`    | bool | `true` | 是否允许发送统计数据, 仅用于统计插件用户数量, 不会发送任何其他信息              |
| `DisabledCmds` | list | `null` | 在此列表中的命令将会被禁用\*\* , **不区分大小写**, 仅对 `ASFOAuth` 中的命令生效 |

> \*\* `DisabledCmds` 配置说明: 该项配置**不区分大小写**, 仅对 `ASFOAuth` 中的命令有效
> 例如配置为 `["foo","BAR"]` , 则代表 `FOO` 和 `BAR` 命令将会被禁用
> 如果无需禁用任何命令, 请将此项配置为 `null` 或者 `[]`
> 当某条命令被禁用时, 仍然可以使用 `ASFO.xxx` 的形式调用被禁用的命令, 例如 `ASFO.OAUTH`

## 插件指令说明

### 插件更新

| 命令          | 缩写    | 权限            | 说明                                            |
| ------------- | ------- | --------------- | ----------------------------------------------- |
| `ASFOAuth`    | `ASFO`  | `FamilySharing` | 查看 ASFOAuth 的版本                            |
| `ASFOVERSION` | `ASFOV` | `Operator`      | 检查 ASFOAuth 是否为最新版本                    |
| `ASFOUPDATE`  | `ASFOU` | `Owner`         | 自动更新 ASFOAuth 到最新版本 (需要手动重启 ASF) |

### 功能指令

| 命令              | 缩写 | 权限     | 说明                                                                      |
| ----------------- | ---- | -------- | ------------------------------------------------------------------------- |
| `OAUTH [Bot] Uri` | `O`  | `Master` | 自动使用机器人身份通过 SteamOpenId 登录第三方网站, 返回跳转回第三方的网址 |
