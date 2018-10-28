const int temperaturePin = 0;
void setup()
{ 
  Serial.begin(9600);
}


void loop()
{

  Serial.println(getTemperature(temperaturePin));
  delay(1000); // repeat once per second (change as you wish!)
}


float getTemperature(int pin)
{
  
  return (((analogRead(pin) * 0.004882814) - 0.5)* 100.0);

}
