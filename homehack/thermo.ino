#define FAN_PIN 5
#define THERMO_PIN A2
#define HEAT_PIN 6
#define THERMO_TARGET 30
#define THERMO_TOLERANCE 2

#define GREATER 1
#define SMALLER 2
#define EQUAL   3

int thermo_in;
int heat_out;
int fan_out;
int thermo_target = THERMO_TARGET;


void  manage_thermo(int val)
{
  thermo_target = val;
  read_thermo();
  #ifdef DEBUG
   // log_thermo();
  #endif
  if (tolerance_cmp_t(thermo_target, thermo_in) == SMALLER)
  {
    heat_up();
  }
  else if (tolerance_cmp_t(thermo_target, thermo_in) == GREATER)
  {
    cool_down();
  }
}

void cool_down()
{
  #ifdef DEBUG
    Serial.println("cooling");
  #endif
  digitalWrite(HEAT_PIN, HIGH);
  digitalWrite(FAN_PIN, HIGH);
}
void heat_up()
{
  #ifdef DEBUG
    Serial.println("heating");
  #endif
  digitalWrite(HEAT_PIN, LOW);
  digitalWrite(FAN_PIN, LOW);
}

//a fata de ref 
int  tolerance_cmp_t(int ref, int a)
{
  if (ref + THERMO_TOLERANCE < a)
    return (GREATER);
  else if (ref - THERMO_TOLERANCE > a)
    return (SMALLER);
  return (EQUAL);
}

void  setup_thermo()
{
  pinMode(FAN_PIN, OUTPUT);
  pinMode(HEAT_PIN, OUTPUT);
  pinMode(THERMO_PIN, INPUT);
}

void  read_thermo()
{
  thermo_in = readTempInCelsius(10, THERMO_PIN);
}

int readTempInCelsius(int count, int pin) {
  float temperaturaMediata = 0;
  float sumaTemperatura = 0;
  for (int i =0; i < count; i++) {
    int reading = analogRead(pin);
    float voltage = reading * 5.0;
    voltage /= 1024.0;
    float temperatureCelsius = (voltage - 0.57) * 100 ;
    sumaTemperatura = sumaTemperatura + temperatureCelsius;
  }
  return (int) (sumaTemperatura / (float)count);
}

void  log_thermo()
{
  Serial.print("thermo:");
  Serial.println(thermo_in);
}

