#define F_CPU 16000000 //16 MHz

#include <avr/eeprom.h>
#include <avr/io.h>
#include <util/delay.h>


int main(void)
{
	DDRD = 0xff;
	//eeprom_write_byte((uint8_t*)3, 17);
	PORTD = eeprom_read_byte((uint8_t*)3);
	return (0);
}
