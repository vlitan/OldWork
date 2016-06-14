#define F_CPU 16000000UL //16MHz

#include <util/delay.h>
#include <avr/interrupt.h>
#include "io.h"

volatile uint8_t  i;

void	exceptional_port_set(uint8_t x)
{
	PORTD = x | (1 << 2);
	if (x & (1 << 2))
	{
		set_pin(8, 1);
	}
	else
	{
		set_pin(8, 0);
	}
}

ISR (INT0_vect)
{
	++i;
}

int main(void)
{
	DDRD = 0xff;
	DDRB = 0xff;
		
	set_port_pin(&PORTD, 2, 1); //pull-up
	
	EICRA |= (1 << ISC01); //set trigger
	EIMSK |= (1 << INT0);  //set int pin

	sei();	//enable interrupts

	while (1)
	{
		exceptional_port_set(i - 1);
	}
	return (0);
}
