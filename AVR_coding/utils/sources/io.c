#include "io.h"

inline void	set_port_pin(volatile uint8_t *port, uint8_t pin_index, bool pin_val)
{
	if (pin_val)
		*port |= 1<<pin_index;
	else
		*port &= ~(1<<pin_index);
}

inline void	set_pin(uint8_t pin, bool val)
{
	if ((0 <= pin) && (pin <= 7))
		set_port_pin(&PORTD, pin, val);
	else if ((8 <= pin) && (pin <= 13))
		set_port_pin(&PORTB, pin - 8, val);
}
