
String Data = "10";
void setup()
{
    Serial.begin(9600);    
                           
    pinMode(13, OUTPUT);
     
}

void loop()
{
   if(Serial.available() > 0)      
   {
      Data = Serial.readString();  
      
      Serial.print("{\"objem\":\"");
      Serial.print(Data);       
      Serial.print("\"}");            
   }
}
