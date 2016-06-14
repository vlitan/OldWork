#include "io.h"
#define F_CPU 16000000UL // 16 MHz
#include <util/delay.h>
#define	motor1_p1	0
#define	motor1_p2	1
#define	motor2_p1	2
#define	motor2_p2	3

void	rotate(uint8_t motor, uint8_t sense)
{
	if (motor == 1)
	{
		set_pin(motor1_p1, sense);
		set_pin(motor1_p2, !sense);
	}
	else
	{
		set_pin(motor2_p1, sense);
		set_pin(motor2_p2, !sense);
	}	
}

void	stop(uint8_t motor)
{
	if (motor == 1)
	{
		set_pin(motor1_p1, 0);
		set_pin(motor1_p2, 0);
	}
	else
	{
		set_pin(motor2_p1, 0);
		set_pin(motor2_p2, 0);
	}
}

int main(void)
{
	DDRD = 255;
	DDRB = 0; 
	PORTD = 0;
    while(1)
    {
		rotate(1, 0);
		rotate(2, 1);
		_delay_ms(50);
		stop(1);
		stop(2);
		_delay_ms(10);
		rotate(1, 1);
		rotate(2, 0);
		_delay_ms(50);
		stop(1);
		stop(2);
		_delay_ms(10);
    }
	return (0);
}
