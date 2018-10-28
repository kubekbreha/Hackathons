long startTime;
long buzzerWaitTime;
long buzzerIntervalTime = 100;
int buzzerValue = 0;
const int buzzerPin = 5;

void setup() {
  pinMode(5, OUTPUT);
  buzzerWaitTime = startTime + buzzerIntervalTime;
}

void loop() {
  startTime = millis();
  
  if(startTime >= buzzerWaitTime && buzzerValue == 0)
  {
    digitalWrite(5, HIGH);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 1)
  {
    digitalWrite(5, LOW);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }if(startTime >= buzzerWaitTime && buzzerValue == 2)
  {
    digitalWrite(5, HIGH);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 3)
  {
    digitalWrite(5, LOW);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 4)
  {
    digitalWrite(5, HIGH);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
  if(startTime >= buzzerWaitTime && buzzerValue == 5)
  {
    digitalWrite(5, LOW);
    buzzerWaitTime = startTime + buzzerIntervalTime;
    buzzerValue++;
  }
}
