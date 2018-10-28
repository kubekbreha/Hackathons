#include <FastLED.h>

#define DATA_PIN     6
#define NUM_LEDS    47
#define BRIGHTNESS  150
#define LED_TYPE    WS2811
#define COLOR_ORDER GRB


CRGB leds[NUM_LEDS];

char data;
int dataValue;


const int temperaturePin = 0;

byte statusLed    = 13;
byte sensorInterrupt = 0;
byte sensorPin = 2;
float calibrationFactor = 7;

volatile byte pulseCount;

float flowRate;
unsigned int flowMilliLitres;
unsigned long totalMilliLitres;
unsigned long totalMilliLitresResetValue = 0;

unsigned long oldTime;

long startTime;
long waitTime;
long waitTimeReset;
long waitTimeMiliLitres = 3000;
long waitStreamTime = 1000;

long buzzerWaitTime;
long buzzerIntervalTime = 100;
int buzzerValue = 0;
const int buzzerPin = 5;

int ledStripState =0;

void setup()
{
    Serial.begin(9600);    
    Serial.setTimeout(5);   

    FastLED.addLeds<WS2812B, DATA_PIN, RGB>(leds, NUM_LEDS);
    FastLED.setBrightness(BRIGHTNESS);


    pinMode(statusLed, OUTPUT);
    digitalWrite(statusLed, HIGH);

    pinMode(sensorPin, INPUT);
    digitalWrite(sensorPin, HIGH);

    pulseCount        = 0;
    flowRate          = 0.0;
    flowMilliLitres   = 0;
    totalMilliLitres  = 0;
    oldTime           = 0;

    attachInterrupt(sensorInterrupt, pulseCounter, FALLING);
    startTime = millis();
    waitTime = startTime + waitStreamTime;

    pinMode(buzzerPin, OUTPUT);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    fcnA();
}

void loop()
{
    if((millis() - oldTime) > 1000)    // Only process counters once per second
  { 
    // Disable the interrupt while calculating flow rate and sending the value to
    // the host
    detachInterrupt(sensorInterrupt);
        
    // Because this loop may not complete in exactly 1 second intervals we calculate
    // the number of milliseconds that have passed since the last execution and use
    // that to scale the output. We also apply the calibrationFactor to scale the output
    // based on the number of pulses per second per units of measure (litres/minute in
    // this case) coming from the sensor.
    flowRate = ((1000.0 / (millis() - oldTime)) * pulseCount) / calibrationFactor;
    
    // Note the time this processing pass was executed. Note that because we've
    // disabled interrupts the millis() function won't actually be incrementing right
    // at this point, but it will still return the value it was set to just before
    // interrupts went away.
    oldTime = millis();
    
    // Divide the flow rate in litres/minute by 60 to determine how many litres have
    // passed through the sensor in this 1 second interval, then multiply by 1000 to
    // convert to millilitres.
    flowMilliLitres = (flowRate / 60) * 1000;
    
    // Add the millilitres passed in this second to the cumulative total
    totalMilliLitres += flowMilliLitres;

    startTime = millis();
      if(startTime >= waitTimeReset)
      {
        if(totalMilliLitresResetValue == totalMilliLitres)
        {
          totalMilliLitres = 0;
        }
        totalMilliLitresResetValue = totalMilliLitres;
        waitTimeReset+=waitTimeMiliLitres;
      }
    
      
    unsigned int frac;


    /*
    // Print the flow rate for this second in litres / minute
    Serial.print("Flow rate: ");
    Serial.print(int(flowRate));  // Print the integer part of the variable
    Serial.print("L/min");
    Serial.print("\t");       // Print tab space

    // Print the cumulative total of litres flowed since starting
    Serial.print("Output Liquid Quantity: ");        
    Serial.print(totalMilliLitres);
    Serial.println("mL"); 
    Serial.print("\t");       // Print tab space
  Serial.print(totalMilliLitres/1000);
  Serial.print("L");
    
*/
    // Reset the pulse counter so we can start incrementing again
    pulseCount = 0;
    
    // Enable the interrupt again now that we've finished sending output
    attachInterrupt(sensorInterrupt, pulseCounter, FALLING);
  }

      startTime = millis();
      if(startTime >= waitTime)
      {
        Serial.print("{\"objem\":\"");
        Serial.print(totalMilliLitres); 
        Serial.print("\", \"teplota\":\"");
        Serial.print(getTemperature(temperaturePin));      
        Serial.println("\"}"); 
        waitTime+=waitStreamTime;
      }
      
  ////////////////////////////////
   if(Serial.available() > 0)      
   {
      data = Serial.read(); 

      if(data == '1')
      {
        ledStripState =1;
      }
      if(data == '2')
      {
        ledStripState =2;
      }
      if(data == '3')
      {
        ledStripState =3; 
    
        buzzerValue = 0;
        startTime = millis();
        buzzerWaitTime = startTime + buzzerIntervalTime;
      }
   }

   startTime = millis();
  
  if(startTime >= buzzerWaitTime && buzzerValue == 0)
  {
    digitalWrite(buzzerPin, HIGH);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 1)
  {
    digitalWrite(buzzerPin, LOW);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }if(startTime >= buzzerWaitTime && buzzerValue == 2)
  {
    digitalWrite(buzzerPin, HIGH);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 3)
  {
    digitalWrite(buzzerPin, LOW);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 4)
  {
    digitalWrite(buzzerPin, HIGH);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 5)
  {
    digitalWrite(buzzerPin, LOW);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }


  if(ledStripState == 1)
  {
    fcnA();
   
  }
  if(ledStripState == 2)
  {
    fcnB();
  }
  if(ledStripState == 3)
  {
    fcnC();
  }
}

/*
Insterrupt Service Routine
 */
void pulseCounter()
{
  // Increment the pulse counter
  pulseCount++;
}


float getTemperature(int pin)
{
  
  return (((analogRead(pin) * 0.004882814) - 0.5)* 100.0);

}

void fcnA(){
  FastLED.setBrightness(BRIGHTNESS);
  fill_solid(leds,NUM_LEDS,CRGB::Green);
  FastLED.show();
  
}

void fcnB(){
 FastLED.setBrightness(BRIGHTNESS);
  fill_solid(leds,NUM_LEDS,CRGB::Orange);
  FastLED.show();
}

void fcnC(){
   FastLED.setBrightness(BRIGHTNESS);
  fill_solid(leds,NUM_LEDS,CRGB::Red);
  FastLED.show();
}



