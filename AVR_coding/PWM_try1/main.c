#define F_CPU 16000000 // 16 MHz

#include <avr/io.h>
#include "io.h"
#include <util/delay.h>
# define MAX_DUTY	255

void	set_duty(uint8_t prcnt)
{
	OCR0A = (prcnt / 100.0) * 255.0;
}

int		main(void)
{
	
	uint8_t	dutyCycle = 0;

	DDRD = 0xff; //set as output

	TCCR0A = /*(1 << COM0A0) | */ (1 << COM0B1) | (1 << COM0A1) | (1 << WGM00) | (1 << WGM01);//pwm mode

	OCR0A = 200;
	OCR0B = 100;

	TCCR0B = (1 << CS01); //prescaler (no prescale)

	while (1)
	{
		//functional pwm
	}
	return (0);
}
