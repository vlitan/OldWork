//light sensor pin
#define LIGHT_SENSE A0
//h-bridge pins
#define M1_1 10
#define M1_2 11
#define MOTORS_SPEED 70
//end switches
#define END_1 2 //tray is fully in
#define END_2 3 //tray is fully out
//light stuff
#define LED_PIN 9
#define LIGHT_TARGET 600
#define LIGHT_RATE 5
//comp parameters
#define LIGHT_TOLERANCE 5
//*****
#define GREATER 1
#define SMALLER 2
#define EQUAL   3

int   sw1 = 0;
int   sw2 = 0;
int   light_target;
int   light_LED;
int   light_in;

void setup_lights()
{
  pinMode(M1_1,OUTPUT);
  pinMode(M1_2,OUTPUT);
  pinMode(END_1,INPUT);
  pinMode(END_2,INPUT);
  pinMode(LED_PIN, OUTPUT);
  
  light_LED = 15;
  light_target = LIGHT_TARGET;
}


#ifdef DEBUG
void  log_lights()
{
  Serial.print("Light level:");
  Serial.println(light_in);
}
#endif
void  manage_lights(int val)
{
  light_target = val;
  getPosition(&sw1,&sw2);
  light_in = readLight();
  #ifdef DEBUG
    log_lights();
  #endif
  if (tolerance_cmp(light_target, light_in) == SMALLER)
  {
    light_up();
    #ifdef DEBUG
      Serial.println("lights up..");
    #endif
  }
  else if (tolerance_cmp(light_target, light_in) == GREATER)
  {
    light_down();
    #ifdef DEBUG
      Serial.println("closing down..");
    #endif
  }
  else
  {
    stop_motors();
    #ifdef DEBUG
      Serial.println("nothing");
    #endif
  }
}

//a fata de ref 
int  tolerance_cmp(int ref, int a)
{
  if (ref + LIGHT_TOLERANCE < a)
    return (GREATER);
  else if (ref - LIGHT_TOLERANCE > a)
    return (SMALLER);
  return (EQUAL);
}

void  stop_motors()
{
  go(0);
}

void  light_down()
{
  close_LED_down();
  if (light_LED < LIGHT_RATE)
  {
    close_curtain();
  }
}

void  light_up()
{
  open_curtain();
  if (!sw2)
  {
    light_LED_up();
  }
}

void  close_LED_down()
{
    light_LED -= LIGHT_RATE;
    if (light_LED < 0)
      light_LED = 0;
    analogWrite(LED_PIN, light_LED);
}

void  light_LED_up()
{
    light_LED += LIGHT_RATE;
    if (light_LED > 255)
      light_LED = 255;
    analogWrite(LED_PIN, light_LED);
}

void  open_curtain()
{
    go(-MOTORS_SPEED);
}

void  close_curtain()
{
    go(MOTORS_SPEED);
}

int   readLight()
{
  int i;
  int average=0;
  for (i=0;i<5;i++)
    average += analogRead(LIGHT_SENSE);
  average /=5;
  return average;
}

void go(int _speed) //if _speed is positive, tray moves inwards
{
  if ((_speed > 0) && (sw1))
  {
    analogWrite(M1_1, _speed);
    analogWrite(M1_2, 0);
  }
  else if ((_speed < 0) && (sw2))
  { 
     analogWrite(M1_1, 0);
     analogWrite(M1_2, -_speed);
  }
  else
  {
    analogWrite(M1_1, 0);
    analogWrite(M1_2, 0);
  }
}

#ifdef DEBUG
void  log_ends()
{
  Serial.print("sws: ");
  Serial.print(sw1);
  Serial.print(" ");
  Serial.println(sw2);
}
#endif

void getPosition(int *m1,int *m2)
{
  #ifdef DEBUG
   // log_ends();
  #endif
  *m1 = digitalRead(END_1);
  *m2 = digitalRead(END_2);
}

bool dir = true;
void  bounce_tray()
{
    //this code bounces the tray***
  if (dir)
    go(70);
   else
    go(-70);
  if (!sw1)
    dir = false;
  else if (!sw2) 
    dir = true;
}
