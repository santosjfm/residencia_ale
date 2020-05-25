int inputPin1 = 2;
int inputpin2 = 3;
void setup() {
pinMode(10,OUTPUT);
pinMode(9,OUTPUT);
Serial.begin(9600);
pinMode(inputPin1, INPUT);
pinMode(inputPin2, INPUT);
}

void loop() {
    
  if(digitalRead(inputPin1)== HIGH){
  digitalWrite(10,HIGH);
  digitalWrite(9, LOW);
  Serial.println("1");
  delay(10);
  }
  if(digitalRead(inputPin2)== HIGH){
  digitalWrite(10,LOW);
  digitalWrite(9, HIGH);
  Serial.println("0");
  delay(10);
  }
  
  }

}
