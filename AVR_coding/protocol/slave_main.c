#define F_CPU 16000000 // 16 MHz
#define LS4_MASK 0xf
#define MS4_MASK 0xf0

#include "io.h"
#include <util/delay.h>

int main(void)
{
	/*variables*/
	register uint8_t last_state;
	/***********/
	
	/*setup*/
	PORTD = 0;
	PORTB = (1 << 0) | (1 << 1) | (1 << 2) | (1 << 3);
	/*******/

	last_state = 0;

	while (1)
	{
		if (PIND != last_state)
		{
			last_state = PIND;
			if (read_port_pin(PINB, 4))
				PORTB = ((PIND & MS4_MASK) >> 4);
			else
				PORTB = (PIND & LS4_MASK);
		}
	}	
	
		
	return (0);
}
