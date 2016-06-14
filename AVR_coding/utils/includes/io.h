#ifndef IO_H
# define IO_H
# include <avr/io.h>
# include <inttypes.h>
# include <stdbool.h>

#define LOW 0
#define HIGH 1

#define	read_port_pin(port, pin_index) ((bool)((port) & (1<<(pin_index))))
inline void	set_port_pin(volatile uint8_t *port, uint8_t pin_index, bool pin_val);
inline void	set_pin(uint8_t pin, bool val);
#endif
