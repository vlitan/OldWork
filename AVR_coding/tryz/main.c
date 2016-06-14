#define F_CPU 16000000UL

#include <avr/io.h>
#include <util/delay.h>

int main(void)
{
	DDRD = 0xff;
	PORTD = 0;
	int i = 0;
	uint8_t poz;

	poz = 3;
	set_port_pin(&PORTD, poz, 1);

	while (1)
	{
		_delay_ms(100);
		PORTD = i++;
		if (i > 256)
			i = 0;
	}
	
}
