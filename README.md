# DiskSpeedTest

Automate the [DiskSpd](https://github.com/microsoft/diskspd) utility to run iterative IO performance tests.  

_**Use at your own risk, DiskSpd can be destructive.**_

## License

![GitHub](https://img.shields.io/github/license/ptr727/DiskSpeedTest)

## Usage

- Clone a copy of the code from [GitHub](https://github.com/ptr727/DiskSpeedTest), and open the project with Visual Studio or Visual Studio Code.
- Modify the source code to set the output file location, the test targets, and the test parameters as desired.
- Download [DiskSpd](https://aka.ms/diskspd), and place the binary in the working directory.
- Compile and run the code.
- Analyze the CSV results.

## Notes

- I created the utility to help [troubleshoot](https://blog.insanegenius.com/2019/06/10/unraid-in-production-a-bit-rough-around-the-edges-and-terrible-smb-performance/) UnRaid v6.7.2 SMB performance issues.
- While retesting I encountered IO failures with Unraid v6.8.1, appears to be with block sizes 512KB, 1MB, and 2MB, no such failures when using v6.7.2.
```shell
diskspd.exe -w50 -b524288 -F2 -o8 -W30 -d120 -r -Rxml -Srw \\Server-2\CacheSpeedTest\SpeedTestDataFile.dat
error during overlapped IO operation (error code: 59)
error during overlapped IO operation (error code: 59)
There has been an error during threads execution
Error generating I/O requests

diskspd.exe -w50 -b1048576 -F2 -o8 -W30 -d120 -r -Rxml -Srw \\Server-2\CacheSpeedTest\SpeedTestDataFile.dat
error during overlapped IO operation (error code: 59)
error during overlapped IO operation (error code: 59)
There has been an error during threads execution
Error generating I/O requests

diskspd.exe -w0 -b2097152 -F2 -o8 -W30 -d120 -r -Rxml -Srw \\Server-2\CacheSpeedTest\SpeedTestDataFile.dat
Error opening file: \\Server-2\CacheSpeedTest\SpeedTestDataFile.dat [64]
There has been an error during threads execution
Error generating I/O requests
Failed to run test

diskspd.exe -w50 -b2097152 -F2 -o8 -W30 -d120 -r -Rxml -Srw \\Server-2\CacheSpeedTest\SpeedTestDataFile.dat
error during overlapped IO operation (error code: 59)
error during overlapped IO operation (error code: 59)
There has been an error during threads execution
Error generating I/O requests
```

## TODO

- Load the test parameter configuration from an external configuration file.
- Specify the DiskSpd executable path.