#define F_CPU 16000000 // 16 MHz

#include <avr/io.h>
#include "io.h"
#include <util/delay.h>

int		main(void)
{
	DDRD = 0xff;
	DDRB = 0;

	set_pin(7, 1);
	_delay_ms(100);
	PORTD = 0;
	while (1)
	{
		if (read_port_pin(PINB, 1))
			set_pin(7, 1);
		else
			set_pin(7, 0);
	}	
	return (0);
}
