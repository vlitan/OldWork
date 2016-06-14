#define F_CPU	20000000UL

#include <stdio.h>

#include <avr/io.h>
#include "uart.h"
#include <util/delay.h>


int main(void) {    

    uart_init();
    stdout = &uart_output;
    stdin  = &uart_input;
                
    char input = 'a';

    while(1) {
		_delay_ms(100);
    	uart_putchar('a', stdout);
	}
        
    return 0;
}
