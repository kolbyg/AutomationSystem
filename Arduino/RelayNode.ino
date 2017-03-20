// Libraries
#include <SPI.h>
#include <Ethernet.h>
#include <aREST.h>
#include <avr/wdt.h>

//**********Configuration**********\\
//Set ther mac address, make it unique
byte mac[] = { 0x90, 0xA2, 0xDA, 0x0E, 0xFE, 0x40 };
//Set the ports the relays are connected to and the quantity. Make the quantity matches the array you have, or weird things will happen.
const int relayPorts[] = {22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,48,49,50,51,52,53};
const int relays = 32;
//Set the node ID, if the ID is taken the automation server will not accept the node.
const int nodeID = 1;
//Set the node type, this will define how the automation server controlls this node, consult the documentation for acceptable types.
const String nodeType = "ARDRLY32ETH";
//Set the description of this node/
String description = "Relay Panel 1 Zone A";
//**********End Configuration**********\\

EthernetServer server(80);
aREST rest = aREST();

void setup(void)
{  
  pinMode(13, OUTPUT);
  rest.variable("desc",&description);
  rest.function("relay",relayControl);
  rest.set_id(String(nodeID));
  rest.set_name(nodeType);
  if (Ethernet.begin(mac) == 0) {
    tone(13,50);
    return;
  }
  server.begin();
  wdt_enable(WDTO_4S);
  for (int x = 0; x < relays; x++)
  {
    pinMode(relayPorts[x], OUTPUT);
    digitalWrite(relayPorts[x], HIGH);
  }
  tone(13,1000);
  delay(200);
  noTone(13);
  delay(200);
  tone(13,1000);
  delay(200);
  noTone(13);
  delay(200);
  tone(13,1000);
  delay(200);
  noTone(13);
  delay(200);
  tone(13,1000);
  delay(1200);
  noTone(13);
}

void loop() {  
  EthernetClient client = server.available();
  rest.handle(client);
  wdt_reset();
}

int relayControl(String command) {
  int state = command.toInt();
  digitalWrite(13,state);
  return 1;
}
