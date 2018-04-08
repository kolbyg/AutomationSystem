// Demo Code for SerialCommand Library
// Steven Cogswell
// May 2011

#include <SerialCommand.h>

#define arduinoLED 13   // Arduino LED on board
#define controllerID 0
#define controllerName "Basement 1"
int channel[channels] = {22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53};

SerialCommand SCmd;

void setup()
{  
  pinMode(13,OUTPUT);
  digitalWrite(13,HIGH);
  for (int x = 0; x < channels; x++){
  pinMode(channel[x],OUTPUT);      // Configure the onboard LED for output
  digitalWrite(channel[x],LOW);    // default to LED off
  }
  Serial.begin(9600); 

  // Setup callbacks for SerialCommand commands 
  SCmd.addCommand("DON",LED_on);       // Turns LED on
  SCmd.addCommand("DOFF",LED_off);        // Turns LED off 
  SCmd.addDefaultHandler(unrecognized);  // Handler for command that isn't matched  (says "What?") 
  Serial.println("$IDENT:" + String(controllerName) + ",ID:" + String(controllerID)); 
  digitalWrite(13,LOW);

}
void loop()
{  
  SCmd.readSerial();     // We don't do much, just process serial commands
}


void LED_on()
{
  char arg = SCmd.next(); 
  if (arg != NULL) 
  {
    int aNumber=atoi(arg);
    Serial.println("DON: " + String(aNumber));
    digitalWrite(channel[aNumber],HIGH);
  }  
}

void LED_off()
{
  int aNumber;  
  char *arg; 
  arg = SCmd.next(); 
  if (arg != NULL) 
  {
    aNumber=atoi(arg);  
  Serial.println("DOFF: " + String(aNumber));
  digitalWrite(channel[aNumber],LOW);
  }
}



void process_command()    
{
  int aNumber;  
  char *arg; 

  Serial.println("We're in process_command"); 
  arg = SCmd.next(); 
  if (arg != NULL) 
  {
    aNumber=atoi(arg);    // Converts a char string to an integer
    Serial.print("First argument was: "); 
    Serial.println(aNumber); 
  } 
  else {
    Serial.println("No arguments"); 
  }

  arg = SCmd.next(); 
  if (arg != NULL) 
  {
    aNumber=atol(arg); 
    Serial.print("Second argument was: "); 
    Serial.println(aNumber); 
  } 
  else {
    Serial.println("No second argument"); 
  }

}

// This gets set as the default handler, and gets called when no other command matches. 
void unrecognized()
{
  Serial.println("What?"); 
}

