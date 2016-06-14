#this makefile works only for 1 source file w/ specified name here#
NAME = Test#	file_name
CC = avr-gcc#	compiler
DD = avrdude#	...
OBC = avr-objcopy#..
MCA = m$(MCU)p#	microcontroller id for avrdude
MCC = atmega$(MCU)p#microcontroller id for compiler
BAUD = 19200#	baud rate
PROG = atmelice_isp#	programator id
LFUSE = 0xd2#	low fuse bit
HFUSE = 0xd1#	high fuse bit
PORT = usb-0000:00:1d.0-1.2/input0
AUXS = ../utils/sources/*.c# list of auxiliary source files
AUXH = ../utils/includes# path to auxiliary headers

all : hex upload

#only object/elf/hex, assume the rest are done
Oobject : 
	$(CC) -g -Os  -mmcu=$(MCC) -c *.c #$(AUXS) -I$(AUXH)
Oelf :
	$(CC) -g -mmcu=$(MCC) -o $(NAME).elf *.o
Ohex : 
	$(OBC) -j .text -j .data -j .eeprom -O ihex $(NAME).elf $(NAME).hex

object	:	Oobject
elf		:	object Oelf
hex		:	elf Ohex clean

terminal :
	$(DD) -p $(MCA) -b $(BAUD) -c $(PROG) -P $(PORT) -t

#asume .hex exists
upload :
	$(DD) -p $(MCA) -b $(BAUD) -c $(PROG) -P $(PORT) \
	-U lfuse:w:$(LFUSE):m -U hfuse:w:$(HFUSE):m \
	-U flash:w:$(NAME).hex:i
check	:
	$(DD) -p $(MCA) -b $(BAUD) -c $(PROG) -P $(PORT)
clean	:
	rm -rf *.o
	rm -rf $(NAME).elf
fclean	: clean
	rm -rf	$(NAME).hex
re		: fclean all
 
