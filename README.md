# Pinger
Util for monitoring internet connection. Use Ping for work.
Settings in *appsettings.json*.
This project just for fun
### Tech
1. Net6
### Settings
1. timeout - ping timeout in ms
2. buffer - buffer size for ping request in bytes
3. batch - count of ping packages in request
4. target - ip or domain target for ping
5. delay - delay between requests
### Dependencies
1. ConsoleTable (https://github.com/khalidabuhakmeh/ConsoleTables)
### Example
![alt text](https://github.com/Dr1N/Pinger/blob/master/2022_02_23_22_32_32_52.png)

Parameters - current work parameters

#### Current State
1. Time - last request time
2. Ping - avg ping from batch requests
3. Count - batch size
4. Fails - errors in last requests
5. Max - general max ping
6. Min - general min ping
7. Avg - general avg ping
8. Errors - general errors counter

Last Errors - it's clear
