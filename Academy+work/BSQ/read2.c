#include <unistd.h>
#include <fcntl.h>
#include <stdlib.h>
#include "reads.h"
#include <stdio.h>
#include "debug_prints.h"
#define	I 0
#define	J 1
#define LINES 2
#define COLUMNS 3
#define COUNT 4

void	get_next_input(char *argv, char *obst, char *free, char *full)
{
	int fd;
	printf("I, the get_next_input, I got called\n");
	if (*argv == '\0')
	{
		fd = 0;
	}
	else
	{
		fd = open(argv, O_RDONLY);
	}
	printf("file %s - %d opened!\n", argv, fd);
	get_items(fd, obst, free, full);
	printf("now we`re gonna analyze a full line!\n");
	int *answ;
	char *line;
	//line = ft_read_to(fd, '\n');
	line = malloc(10);
	read(fd, line, 9);
	answ = get_from_line(answ, line, 9,*obst, *free);
	print_array(answ, answ[0] + 1);
}


int		get_items(int fd, char *obst, char *free, char *full)
{
	char	*buff;
	int		size;
	int		nbr_size;

	buff = ft_read_to(fd, '\n', 20);
	size = ft_atoi(buff, &nbr_size);
	*free = buff[nbr_size];
	*obst = buff[nbr_size + 1];
	*full = buff[nbr_size + 2];
	printf("I read the int:%d\nthe free:%c\nthe obst:%c\nthe full:%c\n", size, *free, *obst, *full);
	if (!free || !obst || !full || !size || (buff[nbr_size + 3] != '\n'))
		return (-1);
	printf("and all was ok\n");
	return (size);
}


int		*get_from_line(int *answ, char *line, int size, char obst, char freec)
{
	int *answ_buff;
	int i;
	
	printf("I have to work with this: %s\n", line);
	i = 0;
	answ_buff = (int*)malloc(sizeof(int) * (size + 1));
	answ_buff[0] = 0;
	while ((i < size) && ((answ_buff[0] != -1)))
	{
		
		if (is_valid_char(line[i], obst, freec))
		{
			if (line[i] == obst)
			{
				answ_buff[0]++;
				answ_buff[answ_buff[0]] = i;
			}
		}
		else
		{
			printf("I don`t like zis one:-%c-\n", line[i]);
			answ_buff[0] = -1;
		}
		i++;
	}
	printf("oh..well, I got this in my buff of ints: ");
	print_array(answ_buff, answ_buff[0] + 1);
	answ = (int*)malloc(sizeof(int) * (answ_buff[0] + 1));
	copy_data(answ, answ_buff, answ_buff[0] + 1);
	printf("and this is my %d ints final array: ", answ[0]);
	print_array(answ, answ[0] + 1);
	free(answ_buff);
	return (answ);
}
void	read_input(int fd, char *obst, char *free, char *full)
{
}
/*
int		**read_input(int fd, char *obst, char *free, char *full)
{
	int		*vars;
	char	**buff;
	char	free;
	int		**answ;

	vars = (int*)malloc(sizeof(int) * 5);

	buff = (char**)malloc(sizeof(char*) * 2);//tbc
	buff[0] = (char*)malloc(1000);
	buff[1] = (char*)malloc(1);
	vars[LINES] = get_items(fd, obst, free, full);
	answ = (int**)malloc(sizeof(int*) * vars[LINES]);
	vars[COLUMNS] = 0;
	buff[1][0] = *free;
	while (buff[1][0] != '\n')
	{
		read(fd, buff[1], 1);
		buff[0][vars[COLUMNS]] = buff[1][0];
		vars[COLUMNS]++;
	}
	get_from_line(answ[0], buff[0], vars[COLUMNS], *obst, *free);
	vars[I] = 1;
	free(buff[0]);
	free(buff[1]);
	buff[0] = (char*)malloc(vars[COLUMNS]);
	while (vars[I] < vars[LINES])
	{
		read(fd, buff[0], vars[COLUMNS]);
		get_from_line(answ[vars[I]], buff[0], *obst, *free);
	}
	return (answ);
}*/
