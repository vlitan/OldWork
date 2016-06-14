/*
./exec n m density count index
*/
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <fcntl.h>
#define OBSTs "o"
#define FREEs "."
#define FILLs "X"
#define MAX	   1000
int		get_next_file_desc(int index, int n, int m, int dens)
{
	char f_name[100];
	
	sprintf(f_name, "t_%d-s%d_on_%d-d_%d.in", index, n, m, dens);
	open(f_name, O_RDWR | O_CREAT, 0666);
}

time_t	t;
void	write_test(int fd, int n, int m, int dens)
{
	int		i;
	int 	j;
	char	first_line[100];
	char buff[12];

	sprintf(first_line, "%d%s%s%s\n", n, FREEs, OBSTs, FILLs);
	write(fd, first_line, strlen(first_line));
	
	for (i = 0; i < n; i++)
	{
		for (j = 0; j < m; j++)
		{
			if (rand() % MAX < dens)
				write(fd, FREEs, 1);
			else
				write(fd, OBSTs, 1);
		}
		write(fd, "\n", 1);
	}
}

int main(int argc, char **argv)
{
	int current_fd;
	int count;
	int dens;
	int n;
	int m;
	int index;
	int i;
	if (argc == 6)
	{
		n = atoi(argv[1]);
		m = atoi(argv[2]);
		count = atoi(argv[4]);
		index = atoi(argv[5]);
		dens = atoi(argv[3]);
		srand((unsigned) time(&t));
		for (i = 0; i < count; i++)
		{
			index++;
			current_fd = get_next_file_desc(index, n, m, dens);
			write_test(current_fd, n, m, dens);
		}
	}
	else
	{
		printf("wrong number of arguments, needed %d, got %d\n", 5, argc - 1);
		printf("n m density count index");	
	}
	printf("\n");
	return (0);
}

