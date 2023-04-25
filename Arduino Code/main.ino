#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>
#define SCREEN_WIDTH 128
#define SCREEN_HEIGHT 64
Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1);

String conState, initTime;
int hr, min, sec, move, changeDirection;
unsigned long previousTime = 0;
bool connected = false;

void defaultDisplay(){
  display.setTextSize(3);
  display.setCursor(15 + move, 25);
  display.println("0");
  
  display.setCursor(57, 25);
  display.println(":");
  
  display.setCursor(80 + move, 25);
  display.println("0");
  display.drawLine(40,50,80,50,WHITE);
  display.drawLine(41,51,81,51,WHITE);

  if(changeDirection == 0){
    if(move < 10){
      move++;
    }
      else{
        changeDirection = 1;
        }
    }
    else{
      if(move >= 0){
        move--;
      }
        else{
          changeDirection = 0;
        }
    }
}

//Used to keep time, goes out of sync.
void doTime() {
  unsigned long currentTime = millis();
  display.setTextSize(3);

  if(hr < 10){
    display.setCursor(15, 25);
    display.println("0");
    display.setCursor(35, 25);
    display.println(hr);
  }
    else{
    display.setCursor(15, 25);
    display.println(hr);
    }
  
  display.setCursor(57, 25);
  display.println(":");
  
  if(min < 10){
    display.setCursor(80, 25);
    display.println("0");
    display.setCursor(100, 25);
    display.println(min);
  }
    else{
      display.setCursor(80, 25);
      display.println(min);
    }

    if (currentTime - previousTime >= 1000) {
      previousTime = currentTime;
      sec++;
    }
    if (sec >= 60) {
      min++;
      sec = 00;
    }
    if (min >= 60) {
      hr++;
      min = 00;
    }
    if (hr >= 24) {
      hr = 00;
    }
}

//Function looks simple but saves on excessive redundant code.
void printToSerial(String data){
  Serial.println(data);
  delay(100);
}

void setup() {
  //Init serial.
  Serial.begin(9600);
  //Print to serial if unable to set OLED.
  if (!display.begin(SSD1306_SWITCHCAPVCC, 0x3C)) {
    Serial.println(F("SSD1306 allocation failed"));
    for (;;)
      ;
  }
  //Clear the oled display.
  display.clearDisplay();
  display.setTextColor(WHITE);

  //Define pin inputs.
  pinMode(8, INPUT_PULLUP);
  pinMode(9, INPUT_PULLUP);
  pinMode(10, INPUT_PULLUP);
  pinMode(11, INPUT_PULLUP);
}

void readSerialData(){
  //If serial buffer has data available.
  while (Serial.available() > 0) {
    //Char array to store serial data.
    static char serialStream[25];
    //Array position.
    static unsigned int streamPos = 0;
    //Char to store serial data.
    char checkCon = Serial.read();
    //If the serial doesn't read "\n", store the char to the array and increment position. 
    if (checkCon != '\n') {
      serialStream[streamPos] = checkCon;
      streamPos++;
    }
    //Contains "\n" which we use to define the end of the stream.
    //Read the initial char and act based on this.
    else {
    //"1" is sent from the application to let us know it has detected the module.
      if (serialStream[0] == '1') {
        connected = true;
      }
    //"0" is sent by the application upon exiting, this lets us know we have disconnected,
    //resume initial routine.
      if (serialStream[0] == '0') {
        connected = false;
      }
      //"2" is used to define the time stream, assign it to a string and break it apart using
      //substring to create our time ints
      if (serialStream[0] == '2') {
        initTime = serialStream;
        hr = initTime.substring(1, 3).toInt();
        min = initTime.substring(4, 6).toInt();
      }
      //Finished, reset stream position.
      streamPos = 0;
    }
  }
}

void loop() {
  //Clear screen on each loop otherwise it gets spammed.
  display.clearDisplay();
  //Define our buttons.
  int b1 = digitalRead(8);
  int b2 = digitalRead(9);
  int b3 = digitalRead(10);
  int b4 = digitalRead(11);

  //Read incoming serial data.
  readSerialData();
  
  //Device not connected.
  if (!connected) {
    conState = "Not Connected!";
    printToSerial("EB13378421FF4B5EA8394B41E94EEE5A");
  }

  //We are connected, display this on screen and detect button presses,
  //When receieved, just print to serial which button has been pressed to be detected by the application.
  else {
    conState = "Connected!";
    if (b1 == LOW) { printToSerial("HKB1"); }
    if (b2 == LOW) { printToSerial("HKB2"); }
    if (b3 == LOW) { printToSerial("HKB3"); }
    if (b4 == LOW) { printToSerial("HKB4"); }
    }

  //initTime has been established (thus we have the time), display it.
  if(initTime != ""){
    doTime();
  }
  
  //No initTime, we will display the default moving "face".
  else{
      defaultDisplay();
  }

  //Set our conState (Connected/Not connected).
  display.setCursor(0, 0);
  display.setTextSize(1);
  display.println(conState);
  display.display();
}