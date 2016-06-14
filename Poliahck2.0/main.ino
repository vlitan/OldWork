#define PULL 4

#define MOTOR1_PIN1 3
#define MOTOR1_PIN2 6
#define MOTOR2_PIN1 5
#define MOTOR2_PIN2 9 

#define SPEED_FACTOR  3
#define DIFF          10

#define MAXLEFT 40
#define MAXRIGHT 30


void mpu_setup(int x_off, int y_off, int z_off, int z_accel);
int update_ypr(float *yaw, float pitch, float roll);

float yaw;
float last_yaw;
float pitch;
float roll;
int   right;
int   left;
int   triggered;

void setup()
{
  //Serial.begin(115200);  
  mpu_setup(220, 76, -85, 1788);
  motors_setup();
  digitalWrite(PULL, HIGH);
  left = 0;
  right = 0;
  //Serial.begin(115200);
  triggered = 0;
}

void loop()
{/*
  last_yaw = yaw;
  update_ypr(&yaw, &pitch, &roll);
 // Serial.println(yaw_cmp(last_yaw, yaw));
  
  if (yaw_cmp(last_yaw, yaw) > 0)
  {
    triggered++;
  }
  else if (yaw_cmp(last_yaw, yaw) < 0)
  {
    triggered--;
  }
  else if (yaw_cmp(last_yaw, yaw) == 0)
  {
    if (triggered > 3)
    {
      Serial.println("<-");
    }
    else if (triggered < -3)
    {
      Serial.println("->");
    }
    else 
    {
      Serial.println("||");
    }
    triggered = 0;
  }
*/
  update_motors(&left, &right);
  go(left * SPEED_FACTOR, right * SPEED_FACTOR);
}

int  yaw_cmp(int last, int newy)
{
  if (abs (last - newy) > 5)
  {
    if (abs (last - newy) > 130)
    {
      return (newy - last);
    }
    else
      return (last - newy);
  }
  return (0);
}

void update_motors(int *left, int *right)
{
   update_ypr(&yaw, &pitch, &roll);
   *left = *right = -pitch;
   if (roll > 0)
      *left += roll;
   else
      *right -= roll;
   if (abs(*left) < 7)
      *left = 0;
   if (abs(*right) < 7)
      *right = 0;
}

void motors_setup() {
  pinMode(MOTOR1_PIN1, OUTPUT);
  pinMode(MOTOR1_PIN2, OUTPUT);
  pinMode(MOTOR2_PIN1, OUTPUT);
  pinMode(MOTOR2_PIN2, OUTPUT);
}



void go(int speedLeft, int speedRight) {
  //speedLeft = limit(speedLeft, -255, 255);
  //speedRight = limit(speedRight, -255, 255);
  if (speedLeft < 0)
    speedLeft -= DIFF;
  else if (speedLeft > 0)
    speedLeft += DIFF;
    
  if (speedLeft > 0) {
    analogWrite(MOTOR1_PIN1, speedLeft);
    analogWrite(MOTOR1_PIN2, 0);
  } 
  else {
    analogWrite(MOTOR1_PIN1, 0);
    analogWrite(MOTOR1_PIN2, -speedLeft);
  }
 
  if (speedRight > 0) {
    analogWrite(MOTOR2_PIN1, speedRight);
    analogWrite(MOTOR2_PIN2, 0);
  }else {
    analogWrite(MOTOR2_PIN1, 0);
    analogWrite(MOTOR2_PIN2, -speedRight);
  }
}

int limit(int val, int min_val, int max_val)
{
  if (val > max_val)
    return (max_val);
  if (val < min_val);
    return (min_val);
  return (val);
}
