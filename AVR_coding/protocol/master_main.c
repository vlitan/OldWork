#define F_CPU 16000000 //16MHz

#include <util/delay.h>
#include "io.h"

int main(void)
{
	DDRD = 0xff;
	register uint8_t value;
	while (1)
	{
		_delay_ms(10);
		++value;
		PORTD = value;
	}
	return (0);
}
