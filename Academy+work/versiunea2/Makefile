# **************************************************************************** #
#                                                                              #
#                                                         :::      ::::::::    #
#    Makefile                                           :+:      :+:    :+:    #
#                                                     +:+ +:+         +:+      #
#    By: msinca <marvin@42.fr>                      +#+  +:+       +#+         #
#                                                 +#+#+#+#+#+   +#+            #
#    Created: 2015/09/23 20:29:52 by msinca            #+#    #+#              #
#    Updated: 2015/09/23 20:30:12 by msinca           ###   ########.fr        #
#                                                                              #
# **************************************************************************** #

NAME = main
SRC = debug/*.c *.c proc/*.c
HEAD = includes/
IN = input.in
OUT = output.out
N = 9999
M = 9999
DENS = 12

all :
	gcc $(SRC) -o $(NAME) -I $(HEAD)
create : 
	rm $(IN)
	perl perl_s $(N) $(M) $(DENS) > $(IN)
run : 
	rm $(OUT)
	time ./$(NAME) $(IN) > $(OUT)
