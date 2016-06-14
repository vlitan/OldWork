#define F_CPU 16000000UL //16MHz

#include <util/delay.h>
#include <avr/interrupt.h>
#include "io.h"

volatile uint8_t	state;

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

void	blink()
{
	exceptional_port_set(0xff);
	_delay_ms(15);
	exceptional_port_set(0);
	_delay_ms(15);
}

ISR (INT0_vect)
{
	state = !state;
}

int main(void)
{
	register uint8_t  i;
	DDRD = 0xff;
	DDRB = 0xff;
		
	set_port_pin(&PORTD, 2, 1); //pull-up
	
	EICRA |= (1 << ISC00); //set trigger
	EIMSK |= (1 << INT0);  //set int pin

	state = 0;

	sei();	//enable interrupts

	while (1)
	{
		if (state)
		{
			++i;
			exceptional_port_set(i);
			_delay_ms(30);
		}
		else
		{
			blink();
		}
	}
	return (0);
}
