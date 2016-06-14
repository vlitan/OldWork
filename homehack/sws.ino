#define SWS_COUNT 4
#define SW_PIN0 13
#define SW_PIN1 12
#define SW_PIN2 7
#define SW_PIN3 8
#define SW_DOOR 4
#define BUZZ    A1

bool sw[4];
bool sw_door;
bool sw_last[4];
bool sw_door_last;

void setup_sws()
{
  pinMode(SW_PIN0, INPUT);
  pinMode(SW_PIN1, INPUT);
  pinMode(SW_PIN2, INPUT);
  pinMode(SW_PIN3, INPUT);
  pinMode(SW_DOOR, INPUT);
  pinMode(BUZZ, OUTPUT);
}

void manage_sws()
{
  int sw_id;
  sws_read();
  #ifdef DEBUG
    log_sws();
  #endif
  sw_id = sws_check();
  if ((sw_id != -1) && !sw_door && sw_new_state())
  {
    sws_alert(sw_id);
  }
  else
  {
    digitalWrite(BUZZ, LOW);
  }
}

void sws_alert(int sw_id)
{
  Serial.print("A");
  Serial.println(sw_id);
  digitalWrite(BUZZ, HIGH);
}

String  sw_to_string(int sw_id)
{
  switch (sw_id)
  {
    case 0: return ("circula");
    case 1: return ("moto-fierastraul");
    case 2: return ("geamul");
    case 3: return ("subiectul");
    case 4: return ("cadavrul");
    default: return ("ceva nu-i ok");
  }
}

//returns the nbr if something is open
int sws_check()
{
  for (int i = 0; i < SWS_COUNT; ++i)
  {
    if (!sw[i])
      return (i);
  }
  return (-1);
}
// returns true if the state cheanged
bool sw_new_state() 
{
  for (int i = 0; i < SWS_COUNT; ++i)
  {
    if (sw_last[i] != sw[i])
    {
      return (true);
    }
  }
  return (false);
}

void sws_read()
{
  for (int i = 0; i < SWS_COUNT; ++i)
  {
    sw_last[i] = sw[i];
  }
  sw_door_last = sw_door;
  sw[0] = digitalRead(SW_PIN0);
  sw[1] = digitalRead(SW_PIN1);
  sw[2] = digitalRead(SW_PIN2);
  sw[3] = digitalRead(SW_PIN3);
  sw_door = digitalRead(SW_DOOR);
}

void log_sws()
{
  for (int i = 0; i < SWS_COUNT; ++i)
  {
    Serial.print(i);
    Serial.print(": ");
    Serial.print(sw[i]);
    Serial.print("\t");
  }
  Serial.println(sw_door);
}
