Hut Library
===========

frequently used library collections for system-base development 
such as log, setting, xml, json, ini, ~scheduled and/or event task~,
tcp-ip socket server/client and ftp, http, ~mail client~
with some basically design pattern

first write down on 2016, only work in windows-wpf base system.
but .net core is incredibly growth with MS support.
then now re-write library for .net core

this repository working only my needs. 
but you have something good idea, anytime talk to me.

### 1. HutLib Single Library (now small lib)

Dependency management with Nuget

#### ~1.1. HutTaskManager:~
* Time/File Task Manager
* Dependency - Win32.TaskScheduler to
* now Underconstruction. change Win32.TaskScheduler to System.Threading.Tasks(.net core)
* make will be soon

#### 1.2. HutLog
* Singleton Logger

#### 1.3. HutXML
* XML Simple Serializer

#### 1.4. HutJson
* Json Simple Serializer
* Dependency - Newtonsoft.Json

#### ~1.5. HutMailClient~
* Mail Client
* Dependency - Microsoft.Exchange.WebServices
* currently under construction. but this is not importent for me.

#### 1.6. HutFTPClient
* FTP Client

#### 1.7. HutTCPClient/Server (Not Tested)
* TCP/IP Socket Server
* TCP/IP Socket Client

#### ~1.8 HutPrinter (Not Tested)~
* Printing Document or string
* same with mail client. not importent. just plan

#### 1.9 HutINI(Planned)
* INI Simple Serializer

#### 1.10 HutSharedDirectoryClient(Planned)
* Shared Directory Client

#### 1.11 HutChecksumCalculator(Planned)
* Modulo95
* Longitude Sum
* Xor Sum

#### 1.12 Hut.Pattern
Design Patterns for C#

simplify using for design Patterns
currently implemented are:
 * Singleton
 * Mediator

currently under constructions are:
 * Factory
 * Builder
 * ~NotifyPropertyChanged Observer~ instad ms.IObserver
 
 #### 1.13 general parser(Planned)
 * general-simple text parser
 
### 2. HutTest test Program
* Just Test for HutLib


