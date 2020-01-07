# DiskSpeedTest

Use the [DiskSpd](https://github.com/microsoft/diskspd) utility to run iterative IO performance tests.  
I created the utility to help [troubleshoot](https://blog.insanegenius.com/2019/06/10/unraid-in-production-a-bit-rough-around-the-edges-and-terrible-smb-performance/) UnRaid SMB performance issues.

_**Use at your own risk, DiskSpd can be destructive.**_

## License

![GitHub](https://img.shields.io/github/license/ptr727/DiskSpeedTest)

## Usage

- Clone a copy of the code from [GitHub](https://github.com/ptr727/DiskSpeedTest), and open the project with Visual Studio or Visual Studio Code.
- Modify the source code and adjust the output file location, and the test parameters as desired.
- Download [DiskSpd](https://aka.ms/diskspd), and place the binary in the same location as the compiled project binary.
- Compile and run the code.
- Analyze the CSV results.

## TODO

- Load the test parameters configuration from an external configuration file.