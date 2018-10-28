#include <TimedAction.h>

#include <FastLED.h>

#define DATA_PIN     10
#define NUM_LEDS    23
#define BRIGHTNESS  150
#define LED_TYPE    WS2811
#define COLOR_ORDER GRB
CRGB leds[NUM_LEDS];


TimedAction r2l_Red_ON_Action = TimedAction(20,r2l_Red_ON);
TimedAction r2l_Red_OFF_Action = TimedAction(20,r2l_Red_OFF);
TimedAction l2r_Red_ON_Action = TimedAction(20,l2r_Red_ON);
TimedAction l2r_Red_OFF_Action = TimedAction(20,l2r_Red_OFF);
TimedAction r2l_Orange_ON_Action = TimedAction(20,r2l_Orange_ON);
TimedAction r2l_Orange_OFF_Action = TimedAction(20,r2l_Orange_OFF);
TimedAction l2r_Orange_ON_Action = TimedAction(20,l2r_Orange_ON);
TimedAction l2r_Orange_OFF_Action = TimedAction(20,l2r_Orange_OFF);
TimedAction r2l_Green_ON_Action = TimedAction(20,r2l_Green_ON);
TimedAction r2l_Green_OFF_Action = TimedAction(20,r2l_Green_OFF);
TimedAction l2r_Green_ON_Action = TimedAction(20,l2r_Green_ON);
TimedAction l2r_Green_OFF_Action = TimedAction(20,l2r_Green_OFF);


void setup() {
  Serial.begin(9600);
  FastLED.addLeds<WS2812B, DATA_PIN, RGB>(leds, NUM_LEDS);
  FastLED.setBrightness(BRIGHTNESS);
}

void loop() {  

}


void fcnA(){
  FastLED.setBrightness(BRIGHTNESS);
    l2rRed();
    r2lRed();
    redDelay();  
}

void fcnB(){
  FastLED.setBrightness(BRIGHTNESS);
    l2rOrange();
    r2lOrange();
    orangeDelay();  
}

void fcnC(){
  FastLED.setBrightness(BRIGHTNESS);
    l2rGreen();
    r2lGreen();
    greenDelay();  
}

void r2l_Red_ON(){
   
}
void r2l_Red_OFF(){
    
}

void l2r_Red_ON(){
  
}
void l2r_Red_OFF(){
  
}


void r2l_Orange_ON(){
    
}
void r2l_Orange_OFF(){
    
}


void l2r_Orange_ON(){
    
}

 void l2r_Orange_OFF(){
    
}




void r2l_Green_ON(){
    
}
void r2l_Green_OFF(){
   
}

void l2r_Green_ON(){
    
}
void l2r_Green_OFF(){
    
}


void greenDelay(){
  for(int a=1; a <= 200; a++){
    fill_solid(leds,NUM_LEDS,CRGB::Red);
    FastLED.setBrightness(a);
    FastLED.show();
    FastLED.delay(2);
  }

   for(int a=200; a > 0; a--){
    fill_solid(leds,NUM_LEDS,CRGB::Red);
    FastLED.setBrightness(a);
    FastLED.show();
    FastLED.delay(2);
  }

  fill_solid(leds,NUM_LEDS,CRGB::Black);
}

void orangeDelay(){
  for(int a=1; a <= 200; a++){
    fill_solid(leds,NUM_LEDS,CRGB::Orange);
    FastLED.setBrightness(a);
    FastLED.show();
    FastLED.delay(2);
  }

   for(int a=200; a > 0; a--){
    fill_solid(leds,NUM_LEDS,CRGB::Orange);
    FastLED.setBrightness(a);
    FastLED.show();
    FastLED.delay(2);
  }

  fill_solid(leds,NUM_LEDS,CRGB::Black);
}

void redDelay(){
  for(int a=1; a <= 200; a++){
    fill_solid(leds,NUM_LEDS,CRGB::Green);
    FastLED.setBrightness(a);
    FastLED.show();
    FastLED.delay(2);
  }

   for(int a=200; a > 0; a--){
    fill_solid(leds,NUM_LEDS,CRGB::Green);
    FastLED.setBrightness(a);
    FastLED.show();
    FastLED.delay(2);
  }

  fill_solid(leds,NUM_LEDS,CRGB::Black);
}
