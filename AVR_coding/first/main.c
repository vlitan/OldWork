/*
 * Test.c
 *
 * Created: 10/12/2015 11:14:17 PM
 *  Author: Stelian
 */ 
#include <avr/io.h>
#define F_CPU 16000000UL // 16 MHz
#include <util/delay.h>
#include "first.h"

int main(void)
{
	DDRD = 255;
	PORTD = 0;
    while(1)
    {
	    PORTD = 0;
       _delay_ms(50);
	   PORTD = abs(-255);
	   _delay_ms(50);
    }
}
