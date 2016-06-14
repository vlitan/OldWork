#include "debug_prints.h"
#include "BSQ.h"
#include <stdio.h>
#include "reads.h"
t_map	get_map(void);

int main(int argc, char **argv)
{
	t_map	mock_map;
	char	obst;
	char	free;
	char	full;
	int		matrix;
	(void)argc;
	(void)argv;
	get_next_input(argv[1], &obst, &free, &full);
	printf("\nend of main\n");
	return (0);
}
