#define F_CPU 16000000 // 16 MHz

#include <avr/io.h>
#include "io.h"
#include <util/delay.h>

uint8_t	dutyCycle = 0;

void	set_duty(uint8_t prcnt)
{
	OCR0A = (prcnt / 100.0) * 255.0;
}

int		main(void)
{
	DDRD = 0xff; //set as output

	TCCR0A = (1 << COM0A1) | (1 << WGM00) | (1 << WGM01);//pwm mode
	TIMSK0 = (1 << TOIE0);

	set_duty(0);
	
	TCCR0B = (1 << CS00); //prescaler

	while (1)
	{
		_delay_ms(10);
		dutyCycle += 10;
		if (TCCR0B)
			set_duty(dutyCycle);
		if (dutyCycle > 100)
		{
			set_pin(6, 0);
			TCCR0B = 0;
		}
	}
	return (0);
}
