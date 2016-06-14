//#define DEBUG

int  thermo_target_g;
int  lights_target_g;

void setup_lights();
void manage_lights(int val);
void setup_sws();
void manage_sws();
void setup_thermo();
void manage_thermo(int val);

void setup()
{
  Serial.begin(115200);
  setup_lights();
  setup_sws();
  setup_thermo();
}
char first;
void loop()
{
  if (Serial.available() > 1)
  {
    first = Serial.read();
    if (first == 'l')
    {
      lights_target_g = Serial.parseInt();
      Serial.print("lights: ");
      Serial.println(lights_target_g);
      //manage_lights(lights_target);
    }
    else if (first == 't')
    {
      thermo_target_g = Serial.parseInt();
      Serial.print("thermos: ");
      Serial.println(thermo_target_g);
      //manage_thermo(Serial.parseInt());
    }
  }
  manage_sws();
  delay(100);
}

