Hut Library
===========

frequently used library collections for system-base development 
such as log, setting, xml, json, ini, scheduled and/or event task,
tcp-ip socket server/client and ftp, http, ~mail client~
with some basically design pattern

first write down on 2016, only work in windows-wpf base system.
but .net core is incredibly growth with MS support.
then now re-write library for .net core

this repository working only my needs. 
but you have something good idea, anytime talk to me.

### 1. Hut.Lib.Core

Dependency management with Nuget

#### 1.1 Hut.Pattern
Design Patterns for C#

simplify using for design Patterns
currently implemented are:
 * Singleton
 * Mediator

currently under constructions are:
 * Factory
 * Builder
 * ~NotifyPropertyChanged Observer~ instad ms.IObserver
 
#### 1.2. HutTaskManager
* Time/File base Task Manager
* Asynchronous Task with Sequential Multi Action base working
* Task Management with Json
* Implemented Action - Archive(moving file), Execute(execute file), Transfer(using (s)ftp)
* TODO: work management with System.Threading.Tasks(.net core)

#### 1.3. HutLog
* Singleton Logger

#### 1.4. Hut Formatted File
* CSV Simple Reader
* XML Simple Serializer
* Json Simple Serializer
* INI Simple Serializer(Planned)
* Dependency - Newtonsoft.Json

#### 1.5 general parser
* general-simple text parser
 
#### 1.6. HutFTPClient
* FTP Client
* SFTP Client
* Dependency - SSH.NET

#### 1.7. HutTCPClient/Server (Not Tested)
* TCP/IP Socket Server
* TCP/IP Socket Client

#### 1.8 HutSharedDirectoryClient(Planned)
* Shared Directory Client

#### 1.9 HutChecksumCalculator(Planned)
* Modulo95
* Longitude Sum
* Xor Sum

### 2. HutTest test Program
* Just Test for HutLib

