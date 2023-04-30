A hotkey application using C# and C++/Arduino.

The C# side can automatically scan the COM ports to locate the Arduino, to achieve this I have the Arduino ping out a unique ID that the application can detect,
once it detects the string it will store that COM port.

Once the COM port is set, the application sends the time to the Arduino to be displayed on the OLED screen, it can then receive button presses from the Arduino,
each are associated with a selected application or macro file set in the application, upon button press these will be launched on the host PC.

There is a built in macro creator, able to create basic macro files which seems to be working quite well.

Application settings are stored in an XML file and are loaded if available on launch and saved on every exit.
