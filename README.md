A hotkey application using C# and C++/Arduino.

The C# side can automatically scan the COM ports to locate the Arduino, to achieve this I have the Arduino ping out a unique ID that the application can detect,
once it detects the string it will store that COM port.

Once the COM port is set, the application sends the time to the Arduino to be displayed on the OLED screen, it can then receive button presses from the Arduino,
each are associated with a selected application or macro file set in the application, upon button press these will be launched on the host PC.

There is a built in macro creator, able to create basic macro files which seems to be working quite well.

Application settings are stored in an XML file and are loaded if available on launch and saved on every exit.


![HK1](https://github.com/aaronalam1987/Hotkeys/assets/46248931/3bd26ef2-938a-4b10-8717-a97440ad7275)
![HK2](https://github.com/aaronalam1987/Hotkeys/assets/46248931/a7ad31c9-57ee-4c58-836c-d37b1d032cc5)
![HK3](https://github.com/aaronalam1987/Hotkeys/assets/46248931/d7ec71fe-7d38-4c19-a1a8-a52c21c2d8b3)
