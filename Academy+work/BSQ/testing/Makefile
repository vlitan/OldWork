NAME = gen
SRC  = generator.c
N = 10
M = 10
DENS = 200
COUNT = 3
INDEX = 0

comp :
	gcc $(SRC) -o $(NAME)
clean_in :
	rm *.in
fclean :
	rm $(NAME)
print :
	tail -c +0 *.in
run :
	./$(NAME) $(N) $(M) $(DENS) $(COUNT) $(INDEX)
re : clean_out run print
re_all : fclean clean_out comp 
