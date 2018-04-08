// *** SentandReceive ***

// This example expands the previous Receive example. The Arduino will now send back a status.
// It adds a demonstration of how to:
// - Handle received commands that do not have a function attached
// - Send a command with a parameter to the PC

#include <CmdMessenger.h>  // CmdMessenger

// Blinking led variables 
bool ledState                   = 0;   // Current state of Led
const int kBlinkLed             = 13;  // Pin of internal Led
const int relayPorts[] = {22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53};

// Attach a new CmdMessenger object to the default Serial port
CmdMessenger cmdMessenger = CmdMessenger(Serial);

// This is the list of recognized commands. These can be commands that can either be sent or received. 
// In order to receive, attach a callback function to these events
enum
{
  a              , // Command to request led to be set in specific state
  b              , // Command to report status
  c              , // Command to report status
  
};

// Callbacks define on which received commands we take action
void attachCommandCallbacks()
{
  // Attach callback methods
  cmdMessenger.attach(OnUnknownCommand);
  cmdMessenger.attach(a, OnSetLed);
  cmdMessenger.attach(c, OnSetRelay);
}

// Called when a received command has no attached function
void OnUnknownCommand()
{
  cmdMessenger.sendCmd(b,"Command without attached callback");
}

// Callback function that sets led on or off
void OnSetLed()
{
  // Read led state argument, interpret string as boolean
  ledState = cmdMessenger.readBoolArg();
  // Set led
  digitalWrite(kBlinkLed, ledState?HIGH:LOW);
  // Send back status that describes the led state
  cmdMessenger.sendCmd(b,(int)ledState);
}
void OnSetRelay()
{
  // Read led state argument, interpret string as boolean
  int relayID = cmdMessenger.readInt32Arg();
  int relayState = cmdMessenger.readBoolArg();
  // Set led
  digitalWrite(relayPorts[relayID], relayState?HIGH:LOW);
  // Send back status that describes the led state
  cmdMessenger.sendCmd(b,(int)relayState);
}

// Setup function
void setup() 
{
  // Listen on serial connection for messages from the PC
  Serial.begin(115200); 

  // Adds newline to every command
  cmdMessenger.printLfCr();   

  // Attach my application's user-defined callback methods
  attachCommandCallbacks();

  // Send the status to the PC that says the Arduino has booted
  // Note that this is a good debug function: it will let you also know 
  // if your program had a bug and the arduino restarted  
  cmdMessenger.sendCmd(b,"Arduino has started!");

  // set pin for blink LED
  pinMode(kBlinkLed, OUTPUT);
  for (int x=0; x<23; x++)
  {
    pinMode(relayPorts[x], OUTPUT);
  }
}

// Loop function
void loop() 
{
  // Process incoming serial data, and perform callbacks
  cmdMessenger.feedinSerialData();
}


